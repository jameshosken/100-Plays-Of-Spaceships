﻿using System.Collections.Generic;
using UnityEngine;

//Like flockingBoid, but alters yvalues for terrain
public class FlockingBoidTerrain : MonoBehaviour
{

    [SerializeField] private float separationMultiplier;
    [SerializeField] private float alignmentMultiplier;
    [SerializeField] private float cohesionMultiplier;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float maxForce;
    [SerializeField] private float desiredSeparation;
    [SerializeField] private float neighbourDistance;
    private Rigidbody body;


    // Start is called before the first frame update
    private void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    public void ApplyFlockingSettings(
        float separationMultiplier,
        float alignmentMultiplier,
        float cohesionMultiplier,
        float maxSpeed,
        float maxForce,
        float desiredSeparation,
        float neighbourDistance
        )
    {
        this.separationMultiplier = separationMultiplier;
        this.alignmentMultiplier = alignmentMultiplier;
        this.cohesionMultiplier = cohesionMultiplier;
        this.maxSpeed = Random.Range(maxSpeed / 2, maxSpeed);
        this.maxForce = Random.Range(maxForce / 2, maxForce);
        this.desiredSeparation = Random.Range(desiredSeparation / 2, desiredSeparation);


    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        body.velocity = Limit(body.velocity, maxSpeed);
    }



    public void LimitBounds(Vector3 bounds, Vector3 offset)
    {
        Vector3 force = Vector3.zero;
        if (transform.position.x > bounds.x + offset.x)
        {
            force.x = (bounds.x + offset.x) - transform.position.x ;
        }
        //if (transform.position.y > bounds.y + offset.y)
        //{
        //    force.y = (bounds.y + offset.y) - transform.position.y;
        //}
        if (transform.position.z > bounds.z + offset.z)
        {
            force.z = (bounds.z + offset.z) - transform.position.z;
        }

        if (transform.position.x < offset.x - bounds.x)
        {
            force.x = ( offset.x - bounds.x) - (transform.position.x);
        }
        //if (transform.position.y <  offset.y - bounds.y)
        //{
        //    force.y = (offset.y - bounds.y) - (transform.position.y);
        //}
        if (transform.position.z < offset.z - bounds.z)
        {
            force.z = (offset.z - bounds.z) - (transform.position.z );
        }

        force = Limit(force, maxForce);
        body.AddForce(force);
    }


    public void Flock(List<FlockingBoidTerrain> boids)
    {
        Vector3[] terrainData = GetPositionFromRaycast(transform.position + transform.up * 0.1f, Vector3.down);
        Quaternion terrainOffset = Quaternion.FromToRotation(Vector3.up, terrainData[1]);   //Get rotation from up to normal

        Vector3 sep = Separation(boids);   // Separation
        Vector3 ali = Alignment(boids);      // Alignment
        Vector3 coh = Cohesion(boids);   // Cohesion
                                         // Arbitrarily weight these forces
        sep *= (separationMultiplier);
        ali *= (alignmentMultiplier);
        coh *= (cohesionMultiplier);

        // Add the force vectors to acceleration
        AddForceRelativeToTerrain(sep, terrainOffset);
        AddForceRelativeToTerrain(ali, terrainOffset);
        AddForceRelativeToTerrain(coh, terrainOffset);
    }

    public void SeekTarget(Vector3 target)
    {

        Vector3 seek = Seek(target);
        seek *= 0.01f;
        body.AddForce(seek);

    }

    void AddForceRelativeToTerrain(Vector3 force, Quaternion offsetRotation)
    {
        force.y = 0; // project force onto xz plane

        force = offsetRotation * force; // Calculate force along terrain

        body.AddForce(force);
    }

    private Vector3 Seek(Vector3 target)
    {
        Vector3 desired = target - transform.position;

        desired.Normalize();
        desired *= maxSpeed;

        // Steering = Desired minus Velocity
        Vector3 steer = desired - body.velocity;

        steer = Limit(steer, maxForce);

        return steer;
    }

    private Vector3 Separation(List<FlockingBoidTerrain> boids)
    {

        Vector3 steer = new Vector3(0, 0, 0);
        int count = 0;
        // For every boid in the system, check if it's too close
        for (int i = 0; i < boids.Count; i++)
        {
            if (boids[i] == this)
            {
                continue;
            }
            FlockingBoidTerrain other = boids[i];

            float d = Vector3.Distance(transform.position, other.transform.position);
            // If the distance is greater than 0 and less than an arbitrary amount (0 when you are yourself)
            if ((d > 0) && (d < desiredSeparation))
            {
                // Calculate vector pointing away from neighbor
                Vector3 diff = transform.position - other.transform.position;
                diff.Normalize();
                diff /= d;        // Weight by distance
                steer += diff;
                count++;            // Keep track of how many
            }
        }

        if (count > 0)
        {
            steer /= (float)count;
        }

        // As long as the vector is greater than 0
        if (steer.magnitude > 0)
        {
            // Implement Reynolds: Steering = Desired - Velocity
            steer.Normalize();
            steer *= maxSpeed;
            steer -= body.velocity;
            steer = Limit(steer, maxForce);
        }
        return steer;
    }

    private Vector3 Limit(Vector3 vector, float maxMagnitude)
    {

        if (vector.magnitude > maxMagnitude)
        {
            vector = vector.normalized * maxMagnitude;
        }
        return vector;
    }

    private Vector3 Alignment(List<FlockingBoidTerrain> boids)
    {
        Vector3 sum = Vector3.zero;
        int count = 0;

        for (int i = 0; i < boids.Count; i++)
        {
            if (boids[i] == this)
            {
                continue;
            }
            FlockingBoidTerrain other = boids[i];
            float d = Vector3.Distance(transform.position, other.transform.position);

            if ((d > 0) && (d < neighbourDistance))
            {
                sum += other.GetComponent<Rigidbody>().velocity;
                count++;
            }
        }


        if (count > 0)
        {
            sum /= (float)count;

            sum.Normalize();
            sum *= maxSpeed;
            Vector3 steer = sum - body.velocity;
            steer = Limit(steer, maxForce);
            return steer;
        }
        else
        {
            return Vector3.zero;
        }
    }

    // Cohesion
    private Vector3 Cohesion(List<FlockingBoidTerrain> boids)
    {
        Vector3 sum = Vector3.zero;   // Start with empty vector to accumulate all positions
        int count = 0;
        for (int i = 0; i < boids.Count; i++)
        {
            if (boids[i] == this)
            {
                continue;
            }

            FlockingBoidTerrain other = boids[i];
            float d = Vector3.Distance(transform.position, other.transform.position);
            if ((d > 0) && (d < neighbourDistance))
            {
                sum += other.transform.position; // Add position
                count++;
            }
        }
        if (count > 0)
        {
            sum /= count;
            return Seek(sum);  // Steer towards the position
        }
        else
        {
            return Vector3.zero;
        }
    }

    private Vector3[] GetPositionFromRaycast(Vector3 raycastPosition, Vector3 direction)
    {
        Ray ray = new Ray(raycastPosition, direction);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 200f))
        {
            Vector3[] hitInfo = new Vector3[2];
            hitInfo[0] = hit.point;
            hitInfo[1] = hit.normal;

            return hitInfo;
        }
        else return null;
    }

}

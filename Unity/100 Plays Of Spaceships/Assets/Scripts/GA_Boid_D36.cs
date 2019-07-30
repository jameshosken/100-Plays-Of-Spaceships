using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GA_Boid_D36 : MonoBehaviour
{
    Transform endGoal;
    [SerializeField] Transform detectionSphere;
    [SerializeField] float detectionSphereRadius;
    [SerializeField] float rotationSpeed;
    [SerializeField] float dangerMultiplier; // Amount to multiply rotation by when avoiding

    [SerializeField] float acceleration;
    [SerializeField] float maxSpeed;
    [SerializeField] float overShootMultiplier;

    [SerializeField] bool ignoreOtherBoids;
    [SerializeField] Color myColour;

    Renderer[] renderers;

    Rigidbody body;

    Vector3 forceDirection;
    
    // Start is called before the first frame update
    void Start()
    {
        endGoal = GameObject.Find("End Goal").transform;
        body = GetComponent<Rigidbody>();

    }

    public void ApplyDNASequence(DNASequence genes)
    {

        this.forceDirection = genes.forceDirection;
        this.detectionSphereRadius = genes.detectionSphereRadius;
        this.rotationSpeed = genes.rotationSpeed;
        this.dangerMultiplier = genes.dangerMultiplier;
        this.acceleration = genes.acceleration;
        this.maxSpeed = genes.maxSpeed;
        this.overShootMultiplier = genes.overShootMultiplier;

        this.ignoreOtherBoids = genes.ignoreOtherBoids;
        this.myColour = genes.colour;

        detectionSphere.localScale = Vector3.one * detectionSphereRadius;

        if (ignoreOtherBoids)
        {
            Physics.IgnoreLayerCollision(gameObject.layer, gameObject.layer);
        }

        renderers = GetComponentsInChildren<Renderer>();

        for (int i = 0; i < renderers.Length; i++)
        {
            Material mat = renderers[i].materials[0];
            mat.SetColor("_BaseColor", myColour);
        }
    }



    // Update is called once per frame
    void Update()
    {

        //SeekTarget(endGoal.position);
        ApplyForceDirection();
        ApplyAcceleration();
        LimitVelocity();
        

    }

    private void ApplyForceDirection()
    {
        body.AddForce(forceDirection * acceleration * Time.deltaTime * 0.1f);
    }

    private void ApplyAcceleration()
    {
        body.AddForce(transform.forward * acceleration * Time.deltaTime);
    }

    private void LimitVelocity()
    {
        if (body.velocity.magnitude > maxSpeed)
        {
            body.velocity = body.velocity.normalized * maxSpeed;
        }
    }

    private void SeekTarget(Vector3 targetPosition)
    {
        //Seek target, velocity dependent
        Vector3 lookVector = (targetPosition - body.velocity*overShootMultiplier) - transform.position;

        lookVector = lookVector.normalized;

        body.AddForce(lookVector * acceleration * Time.deltaTime);

        return;

    }

    void AvoidTarget(Vector3 targetPosition)
    {

        float dist = Vector3.Distance(targetPosition, transform.position);

        float multiplier = detectionSphereRadius / (dist + 0.00001f);

        //Avoid specific location, velocity agnostic
        Vector3 lookVector = transform.position - targetPosition;

        lookVector = lookVector.normalized * multiplier;

        body.AddForce(lookVector * acceleration * dangerMultiplier * Time.deltaTime);

    }

    private void OnTriggerStay(Collider other)
    {
        Vector3 point =  other.ClosestPoint(transform.position);
        AvoidTarget(point);
    }
}


using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class ECSFlockingSystem : ComponentSystem
{


    EntityManager entityManager;


    protected override void OnUpdate()
    {


        EntityQuery boidsQuery = GetEntityQuery(ComponentType.ReadOnly<ECSFlockingComponent>());
        NativeArray<Entity> boids = boidsQuery.ToEntityArray(Allocator.TempJob);

        entityManager = World.Active.EntityManager;

        Entities.ForEach((ref Translation translation, ref ECSFlockingComponent flocking) =>
        {
            //Add to Accel
            Flock(boids, ref translation, ref flocking);

            // Add to Vel
            flocking.velocity += flocking.acceleration;
            flocking.velocity = LimitF3(flocking.velocity, flocking.maximumVelocity);

            //Add to Position
            translation.Value += flocking.velocity;

            flocking.acceleration = float3.zero;

        });

        boids.Dispose();
    }

    private void Flock(NativeArray<Entity> boids, ref Translation translation, ref ECSFlockingComponent flocking)
    {
        float3[] flockValues = GetFlockValues(boids, ref translation, ref flocking);
        float3 sep = flockValues[0];   // Separation
        float3 coh = flockValues[1];
        float3 ali = flockValues[2];

        sep *= (1.1f);
        coh *= (1f);
        ali *= (.9f);

        
        float3 bounds = Bounds(ref translation, ref flocking);
        bounds *= 0.5f;

        ApplyForce(ref flocking, sep);
        ApplyForce(ref flocking, coh);
        ApplyForce(ref flocking, ali);

        ApplyForce(ref flocking, bounds);

    }

    
    void ApplyForce(ref ECSFlockingComponent flocking, float3 force)
    {
        force = LimitF3(force, flocking.maximumForce);
        flocking.acceleration += force * Time.deltaTime;
    }

    


    private float3[] GetFlockValues(NativeArray<Entity> boids, ref Translation translation, ref ECSFlockingComponent flocking)
    {
        float3 separation = float3.zero;
        int sepCount = 0;

        float3 alignment = float3.zero;
        int aliCount = 0;

        float3 cohesion = float3.zero;
        int cohCount = 0;

        float sep = flocking.desiredSparation;
        float coh = flocking.cohesionDistance;

        for (int i = 0; i < boids.Length; i++)
        {
            Entity other = boids[i];
            Translation otherTranslation = entityManager.GetComponentData<Translation>(other);
            ECSFlockingComponent otherFlocking = entityManager.GetComponentData<ECSFlockingComponent>(other);

            float d = math.distance(translation.Value, otherTranslation.Value);

            // If the distance is greater than 0 and less than an arbitrary amount (0 when you are yourself)
            if ((d > 0.1f) && (d < sep))
            {

                // Calculate vector pointing away from neighbor
                float3 diff = translation.Value - otherTranslation.Value;
                diff = math.normalize(diff);
                diff /= d;        // Weight by distance
                separation = separation + diff;
                sepCount++;            // Keep track of how many
            }

            if ((d > 0) && (d < coh))
            {
                //ALI
                if (Magnitude(otherFlocking.velocity) > 0)
                {
                    alignment += otherFlocking.velocity;
                    alignment++;
                }

                //COH
                cohesion += otherTranslation.Value; // Add position
                cohCount++;
            }
        }


        ///SEP
        
        if (sepCount > 0)
        {
            separation /= sepCount;
        }

        // As long as the vector is greater than 0
        if (Magnitude(separation) > 0)
        {

            // Implement Reynolds: Steering = Desired - Velocity
            separation = math.normalize(separation);
            separation *= flocking.maximumVelocity;
            //steer = LimitF3(steer, flocking.maximumForce);
        }
        else
        {
            separation = float3.zero;
        }


        ///ALI
        if (aliCount > 0)
        {
            alignment /= (float)aliCount;
            alignment = math.normalizesafe(alignment);
            alignment *= flocking.maximumVelocity;

            alignment = alignment - flocking.velocity;
            //steer = LimitF3(steer, flocking.maximumForce);
            
        }
        else
        {
            alignment = float3.zero;
        }


        ///COH

        if (cohCount > 0)
        {
            cohesion /= (float)cohCount;
            cohesion = Seek(cohesion, ref translation, ref flocking);  // Steer towards the position
        }
        else
        {
            cohesion = float3.zero;
        }

        float3[] payload = new float3[3];
        payload[0] = separation;
        payload[1] = alignment;
        payload[2] = cohesion;
        return payload;
    }

    //private float3 Alignment(NativeArray<Entity> boids, ref Translation translation, ref ECSFlockingComponent flocking)
    //{
    //    float3 sum = float3.zero;
    //    int count = 0;
    //    float coh = flocking.cohesionDistance;

    //    for (int i = 0; i < boids.Length; i++)
    //    {
    //        Entity other = boids[i];
    //        Translation otherTranslation = entityManager.GetComponentData<Translation>(other);
    //        ECSFlockingComponent otherFlocking = entityManager.GetComponentData<ECSFlockingComponent>(other);

            
    //    }


    //    if (count > 0)
    //    {
    //        sum /= (float)count;
    //        sum = math.normalizesafe(sum);
    //        sum *= flocking.maximumVelocity;

    //        float3 steer = sum - flocking.velocity;
    //        //steer = LimitF3(steer, flocking.maximumForce);
    //        return steer;
    //    }
    //    else
    //    {
    //        return float3.zero;
    //    }
    //}

    //private float3 Cohesion(NativeArray<Entity> boids, ref Translation translation, ref ECSFlockingComponent flocking)
    //{
    //    float3 sum = float3.zero;   // Start with empty vector to accumulate all positions
    //    int count = 0;

    //    float coh = flocking.cohesionDistance;

    //    for (int i = 0; i < boids.Length; i++)
    //    {
    //        Entity other = boids[i];
    //        Translation otherTranslation = entityManager.GetComponentData<Translation>(other);
    //        ECSFlockingComponent otherFlocking = entityManager.GetComponentData<ECSFlockingComponent>(other);

    //        float d = math.distance(translation.Value, otherTranslation.Value);

    //        if ((d > 0) && (d < coh))
    //        {
    //            sum += otherTranslation.Value; // Add position
    //            count++;
    //        }
    //    }
    //    if (count > 0)
    //    {
    //        sum /= (float)count;
    //        return Seek(sum, ref translation, ref flocking);  // Steer towards the position
    //    }
    //    else
    //    {
    //        return float3.zero;
    //    }
    //}

    private float3 Seek(float3 target, ref Translation translation, ref ECSFlockingComponent flocking)
    {
        float3 desired = target - translation.Value;

        desired = math.normalize(desired);
        desired *= flocking.maximumVelocity;

        // Steering = Desired minus Velocity
        float3 steer = desired - flocking.velocity;

        //steer = LimitF3(steer, flocking.maximumForce);

        return steer;
    }

    private float3 Bounds(ref Translation translation, ref ECSFlockingComponent flocking)
    {
        float3 force = float3.zero;
        float3 bounds = flocking.moveBounds;

        if (translation.Value.x > bounds.x)
        {
            force.x = (bounds.x) - translation.Value.x;
        }
        else if (translation.Value.x < -bounds.x)
        {
            force.x = (-bounds.x) - (translation.Value.x);
        }

        if (translation.Value.y > bounds.y)
        {
            force.y = (bounds.y) - translation.Value.y;
        }
        else if (translation.Value.y < -bounds.y)
        {
            force.y = (-bounds.y) - (translation.Value.y);
        }
        if (translation.Value.z > bounds.z)
        {
            force.z = (bounds.z) - translation.Value.z;
        }
        else if (translation.Value.z < -bounds.z)
        {
            force.z = (-bounds.z) - (translation.Value.z);
        }
        return force;
    }

    float Magnitude(float3 input)
    {
        return math.sqrt(input.x * input.x + input.y * input.y + input.z * input.z);
    }



    //private float3 Separation(ref Translation translation, ref ECSFlockingComponent flocking)
    //{

    //    Vector3 position = Float3ToVec3(translation.Value);

    //    Vector3 steer = new Vector3(0, 0, 0);
    //    int count = 0;

    //    float sep = flocking.desiredSparation;

    //    Entities.ForEach((ref Translation otherTranslation, ref ECSFlockingComponent otherFlocking) =>
    //    {

    //        Vector3 otherPosition = Float3ToVec3(otherTranslation.Value);
    //        float d = Vector3.Distance(position, otherPosition);


    //        // If the distance is greater than 0 and less than an arbitrary amount (0 when you are yourself)
    //        if ((d > 0) && (d < sep))
    //        {
    //            // Calculate vector pointing away from neighbor
    //            Vector3 diff = position - otherPosition;
    //            diff.Normalize();
    //            diff /= d;        // Weight by distance
    //            steer += diff;
    //            count++;            // Keep track of how many
    //        }
    //    });

    //    if (count > 0)
    //    {
    //        steer /= (float)count;
    //    }

    //    // As long as the vector is greater than 0
    //    if (steer.magnitude > 0)
    //    {
    //        // Implement Reynolds: Steering = Desired - Velocity
    //        steer.Normalize();
    //        steer *= flocking.maximumVelocity;
    //        steer -= Float3ToVec3(flocking.velocity);
    //        steer = LimitV3(steer, flocking.maximumForce);
    //    }
    //    return steer;
    //}



    /// <summary>
    /// HELPER FUNCTIONS
    /// </summary>

    //private Vector3 Float3ToVec3(float3 input)
    //{
    //    return new Vector3(input.x, input.y, input.z);
    //}

    //private float3 Vec3ToFloat3(Vector3 input)
    //{
    //    return new float3(input.x, input.y, input.z);
    //}

    //private Vector3 LimitV3(Vector3 vector, float maxMagnitude)
    //{

    //    if (vector.magnitude > maxMagnitude)
    //    {
    //        vector = vector.normalized * maxMagnitude;
    //    }
    //    return vector;
    //}

    private float3 LimitF3(float3 vector, float maxMagnitude)
    {

        if (Magnitude(vector)> maxMagnitude)
        {
            vector = math.normalize(vector) * maxMagnitude;
        }
        return vector;
    }


}

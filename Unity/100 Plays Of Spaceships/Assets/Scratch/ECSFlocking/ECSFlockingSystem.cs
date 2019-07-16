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
            //flocking.acceleration = float3.zero;

            //We won't be modifying the 'other' data:

            FlockArray(boids, ref translation, ref flocking);
            //Flock(translation, ref flocking);
            flocking.velocity += flocking.acceleration;
            translation.Value += Vec3ToFloat3( flocking.velocity);

            //this.velocity.limit(this.maxSpeed);

            flocking.acceleration *= 0;


        });

        boids.Dispose();
    }

    private void FlockArray(NativeArray<Entity> boids, ref Translation translation, ref ECSFlockingComponent flocking)
    {
        Vector3 sep = SeparationArray(boids, ref translation, ref flocking);   // Separation

        sep *= (1f);
        
        ApplyForce(ref flocking, sep);

    }

    private Vector3 SeparationArray(NativeArray<Entity> boids, ref Translation translation, ref ECSFlockingComponent flocking)
    {

        float3 steer = float3.zero;

        int count = 0;

        float sep = flocking.desiredSparation;

        for (int i = 0; i < boids.Length; i++)
        {
            Entity other = boids[i];

            ECSFlockingComponent otherFlocking = entityManager.GetComponentData<ECSFlockingComponent>(other);
            Translation otherTranslation = entityManager.GetComponentData<Translation>(other);


            float d = math.distance(translation.Value, otherTranslation.Value);


            // If the distance is greater than 0 and less than an arbitrary amount (0 when you are yourself)
            if ((d > 0) && (d < sep))
            {

                // Calculate vector pointing away from neighbor
                float3 diff = translation.Value - otherTranslation.Value;
                diff = math.normalize(diff);
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
        if (math.length(steer) > 0)
        {
            // Implement Reynolds: Steering = Desired - Velocity
            steer = math.normalize(steer);
            steer *= flocking.maximumVelocity;
            steer = 
            steer = LimitF3(steer, flocking.maximumForce);
        }
        return steer;
    }

    public void Flock(Translation translation, ref ECSFlockingComponent flocking)
    {
        
        float3 sep = Separation(ref translation, ref flocking);   // Separation
        //Vector3 ali = Alignment(boids);      // Alignment
        //Vector3 coh = Cohesion(boids);   // Cohesion
                                                                  
        sep *= (1f);
        //ali *= (alignmentMultiplier);
        //coh *= (cohesionMultiplier);

        // Add the force vectors to acceleration
        ApplyForce(ref flocking, sep);
        //body.AddForce(ali);
        //body.AddForce(coh);
    }

    void ApplyForce(ref ECSFlockingComponent flocking, float3 force)
    {
        flocking.acceleration += force * Time.deltaTime;
    }

    private float3 Separation(ref Translation translation, ref ECSFlockingComponent flocking)
    {

        Vector3 position = Float3ToVec3(translation.Value);

        Vector3 steer = new Vector3(0, 0, 0);
        int count = 0;

        float sep = flocking.desiredSparation;

        Entities.ForEach((ref Translation otherTranslation, ref ECSFlockingComponent otherFlocking) =>
        {
            
            Vector3 otherPosition = Float3ToVec3(otherTranslation.Value);
            float d = Vector3.Distance(position, otherPosition);

            
            // If the distance is greater than 0 and less than an arbitrary amount (0 when you are yourself)
            if ((d > 0) && (d < sep))
            {
                
                // Calculate vector pointing away from neighbor
                Vector3 diff = position - otherPosition;
                diff.Normalize();
                diff /= d;        // Weight by distance
                steer += diff;
                count++;            // Keep track of how many
            }
        });

        if (count > 0)
        {
            steer /= (float)count;
        }

        // As long as the vector is greater than 0
        if (steer.magnitude > 0)
        {
            // Implement Reynolds: Steering = Desired - Velocity
            steer.Normalize();
            steer *= flocking.maximumVelocity;
            steer -= Float3ToVec3(flocking.velocity);
            steer = LimitV3(steer, flocking.maximumForce);
        }
        return steer;
    }



    /// <summary>
    /// HELPER FUNCTIONS
    /// </summary>
    
    private Vector3 Float3ToVec3(float3 input)
    {
        return new Vector3(input.x, input.y, input.z);
    }

    private float3 Vec3ToFloat3(Vector3 input)
    {
        return new float3(input.x, input.y, input.z);
    }

    private Vector3 LimitV3(Vector3 vector, float maxMagnitude)
    {

        if (vector.magnitude > maxMagnitude)
        {
            vector = vector.normalized * maxMagnitude;
        }
        return vector;
    }

    private float3 LimitF3(float3 vector, float maxMagnitude)
    {

        if (math.length(vector) > maxMagnitude)
        {
            vector = math.normalize(vector) * maxMagnitude;
        }
        return vector;
    }


}

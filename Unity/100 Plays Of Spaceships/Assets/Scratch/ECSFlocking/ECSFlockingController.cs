using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
using Unity.Rendering;

public class ECSFlockingController : MonoBehaviour
{

    [SerializeField] Mesh mesh;
    [SerializeField] Material material;
    [SerializeField] int numberOfEntities = 100;

    [SerializeField] Unity.Mathematics.float3 startBounds;

    [SerializeField] float maxVel = 10f;
    [SerializeField] float maxForce = 10f;
    [SerializeField] float desiredSep = 5f;
    [SerializeField] float cohesionDist = 10f;




    private void Start()
    {
        EntityManager entityManager = World.Active.EntityManager;

        EntityArchetype archetype = entityManager.CreateArchetype(
            typeof(Translation),
            typeof(RenderMesh),
            typeof(LocalToWorld),
            typeof(ECSFlockingComponent)
            );

        NativeArray<Entity> entityArray = new NativeArray<Entity>(numberOfEntities, Allocator.Temp);
        entityManager.CreateEntity(archetype, entityArray);

        for (int i = 0; i < entityArray.Length; i++)
        {
            //Create entity to populate with data
            Entity entity = entityArray[i];

            ECSFlockingComponent moverSpeed = new ECSFlockingComponent
            {
                maximumVelocity     = UnityEngine.Random.Range(maxVel / 3f, maxVel),
                maximumForce        = UnityEngine.Random.Range(maxForce / 3f, maxForce),
                cohesionDistance    = UnityEngine.Random.Range(cohesionDist/3f, cohesionDist),
                desiredSparation    = UnityEngine.Random.Range(desiredSep / 2f, desiredSep),
                velocity            = float3.zero,
                acceleration        = float3.zero,
                moveBounds          = startBounds
            };

            Translation translation = new Translation
            {
                Value = new Unity.Mathematics.float3(
                    UnityEngine.Random.Range(-startBounds.x, startBounds.x),
                    UnityEngine.Random.Range(-startBounds.y, startBounds.y),
                    UnityEngine.Random.Range(-startBounds.z, startBounds.z)
                    )
            };
              


            RenderMesh renderMesh = new RenderMesh
            {
                mesh = mesh,
                material = material
            };

            
            entityManager.SetComponentData(entity, moverSpeed);
            entityManager.SetComponentData(entity, translation);
            entityManager.SetSharedComponentData(entity, renderMesh);

        }

        entityArray.Dispose();
        
    }
}

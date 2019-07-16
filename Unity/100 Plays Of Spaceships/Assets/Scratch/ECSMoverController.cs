using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
using Unity.Rendering;

public class ECSMoverController : MonoBehaviour
{

    [SerializeField] Mesh mesh;
    [SerializeField] Material material;
    [SerializeField] int numberOfEntities = 100;

    [SerializeField] Unity.Mathematics.float3 startBounds;
    [SerializeField] float speedRange = 2f;
     
    private void Start()
    {
        EntityManager entityManager = World.Active.EntityManager;

        EntityArchetype archetype = entityManager.CreateArchetype(
            typeof(LevelComponentTest),
            typeof(Translation),
            typeof(Rotation),
            typeof(RenderMesh),
            typeof(LocalToWorld),
            typeof(ECSMoveSpeedComponentTest)
            );

        NativeArray<Entity> entityArray = new NativeArray<Entity>(numberOfEntities, Allocator.Temp);
        entityManager.CreateEntity(archetype, entityArray);

        for (int i = 0; i < entityArray.Length; i++)
        {
            //Create entity to populate with data
            Entity entity = entityArray[i];

            //Instantiation method 1: Constructor
            LevelComponentTest level = new LevelComponentTest(Random.Range(10f, 20f));

            //Instantiation method 2: no constructor
            ECSMoveSpeedComponentTest moverSpeed = new ECSMoveSpeedComponentTest {
                moveSpeed = Random.Range(-speedRange, speedRange),
                moveBounds = startBounds
            };

            Translation translation = new Translation
            {
                Value = new Unity.Mathematics.float3(
                    Random.Range(-startBounds.x, startBounds.x),
                    Random.Range(-startBounds.y, startBounds.y),
                    Random.Range(-startBounds.z, startBounds.z)
                    )
            };

            

            RenderMesh renderMesh = new RenderMesh
            {
                mesh = mesh,
                material = material
            };


            entityManager.SetComponentData(entity, level);
            entityManager.SetComponentData(entity, moverSpeed);
            entityManager.SetComponentData(entity, translation);
            entityManager.SetSharedComponentData(entity, renderMesh);

        }

        entityArray.Dispose();
        
    }
}

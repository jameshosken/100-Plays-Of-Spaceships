using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RandomSpawnOnTerrain))]
public class FlockingControllerTerrain : MonoBehaviour
{

    [SerializeField] GameObject boidTemplate;
    [SerializeField] int numberOfBoids = 10;
    [SerializeField] float startRadius;
    [SerializeField] Vector3 bounds;

    [Header("Flocking Vars")]

    [SerializeField] private float separationMultiplier;
    [SerializeField] private float alignmentMultiplier;
    [SerializeField] private float cohesionMultiplier;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float maxForce;
    [SerializeField] private float desiredSeparation;
    [SerializeField] private float neighbourDistance;

    RandomSpawnOnTerrain spawner;

    List<GameObject> boids = new List<GameObject>();
    List<FlockingBoidTerrain> boidsScripts = new List<FlockingBoidTerrain>();
    Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        spawner = GetComponent<RandomSpawnOnTerrain>();

        boids = spawner.CreateObjects(boidTemplate, numberOfBoids, startRadius, startRadius, true);

        foreach (GameObject boid in boids)
        {
            boidsScripts.Add(boid.GetComponent<FlockingBoidTerrain>());
        }
    
    }

    private void Update()
    {
        pos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
         
        for (int i = 0; i < boids.Count; i++)
        {
            boidsScripts[i].SeekTarget(pos);
            boidsScripts[i].Flock(boidsScripts);
            boidsScripts[i].LimitBounds(bounds, pos);
        }
    }

    private void OnValidate()
    {
        
        for (int i = 0; i < boidsScripts.Count; i++)
        {
            boidsScripts[i].ApplyFlockingSettings(
               separationMultiplier,
               alignmentMultiplier,
               cohesionMultiplier,
               maxSpeed,
               maxForce,
               desiredSeparation,
               neighbourDistance);
        }
    }
}

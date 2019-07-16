using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingController : MonoBehaviour
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

    List<GameObject> boids = new List<GameObject>();
    List<FlockingBoid> boidsScripts = new List<FlockingBoid>();
    Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numberOfBoids; i++)
        {
            GameObject boid = Instantiate(boidTemplate) as GameObject;
            Vector3 random = new Vector3(
                Random.Range(-startRadius, startRadius),
                Random.Range(-startRadius, startRadius),
                Random.Range(-startRadius, startRadius)
                );

            Vector3 randomRotation = new Vector3(
                Random.Range(-180, 180),
                Random.Range(-180, 180),
                Random.Range(-180, 180)
                );

            boid.transform.position = random + transform.position;
            boid.transform.Rotate(randomRotation);

            FlockingBoid boidsScript = boid.GetComponent<FlockingBoid>();

            boidsScript.ApplyFlockingSettings(
                separationMultiplier,
                alignmentMultiplier,
                cohesionMultiplier,
                maxSpeed,
                maxForce,
                desiredSeparation,
                neighbourDistance);

            boids.Add(boid);
            boidsScripts.Add(boidsScript);

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

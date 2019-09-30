using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputeFlockingHandler : MonoBehaviour
{
    [SerializeField] GameObject boidTemplate;
    [SerializeField] int numBoids = 5;
    [SerializeField] Vector3 startPosBounds;
    [SerializeField] Vector3 starVelBounds;
    [SerializeField] Vector3 bounds;

    [Space]
    [SerializeField] float desiredSeparation;
    [SerializeField] float neighbourDistance;
    [SerializeField] float maxSpeed;
    [SerializeField] float maxForce;
    [SerializeField] Vector3 sepAliCoh;
    [SerializeField] Vector2 sepAliCohRandomness;

    [Space]
    [SerializeField] float noiseAmplitude;
    [SerializeField] float noiseScale;

    [Space]
    [ColorUsage(true, true)]
    [SerializeField] Color[] boidColours;


    public ComputeShader shader;

    Boid[] inputBoids;

    GameObject[] boidObjects;


    private void Start()
    {
        boidObjects = new GameObject[numBoids];
        inputBoids = new Boid[numBoids];

        SetupShader();
        SetupBoids();

        //RunShader();

    }

   

    private void Update()
    {
        RunShader();
    }
    private void SetupShader()
    {
        shader.SetInt("numBoids", numBoids);
        shader.SetFloat("desiredSeparation", desiredSeparation);
        shader.SetFloat("neighbourDistance", neighbourDistance);
        shader.SetFloat("maxSpeed", maxSpeed);
        shader.SetFloat("maxForce", maxForce);
        shader.SetFloats("bounds", new float[3] { bounds.x, bounds.y, bounds.z });
        shader.SetFloats("noiseSettings", new float[2] { noiseScale, noiseAmplitude });
    }
    void SetupBoids()
    {


        for (int i = 0; i < inputBoids.Length; i++)
        {

            inputBoids[i].pos = new Vector3(
                UnityEngine.Random.Range(-startPosBounds.x, startPosBounds.x),
                UnityEngine.Random.Range(-startPosBounds.y, startPosBounds.y),
                UnityEngine.Random.Range(-startPosBounds.z, startPosBounds.z)
            );

            inputBoids[i].vel = new Vector3(
                UnityEngine.Random.Range(-starVelBounds.x, starVelBounds.x),
                UnityEngine.Random.Range(-starVelBounds.y, starVelBounds.y),
                UnityEngine.Random.Range(-starVelBounds.z, starVelBounds.z)
            );
            inputBoids[i].multipliers = new Vector3(
                UnityEngine.Random.Range(sepAliCoh.x, sepAliCoh.y) * sepAliCoh.x,
                UnityEngine.Random.Range(sepAliCoh.x, sepAliCoh.y) * sepAliCoh.y,
                UnityEngine.Random.Range(sepAliCoh.x, sepAliCoh.y) * sepAliCoh.z
            );


            boidObjects[i] = Instantiate(boidTemplate, inputBoids[i].pos, Quaternion.identity) as GameObject;

            int colourChoice = (int)UnityEngine.Random.Range(0, boidColours.Length);
            Color col = boidColours[colourChoice];
            print(col);
            boidObjects[i].GetComponent<Renderer>().materials[0].SetColor("_EmissionColor", col);
        }


    }

    void RunShader()
    {
        // The size in bytes is simply the number of float values 
        // we are storing multiplied by the size of a float (4 bytes).

        Boid[] outputBoids = new Boid[numBoids];

        ComputeBuffer buffer = new ComputeBuffer(inputBoids.Length, 9 * 4);

        buffer.SetData(inputBoids);
        int kernel = shader.FindKernel("Flock");
        shader.SetBuffer(kernel, "dataBuffer", buffer);
        shader.Dispatch(kernel, inputBoids.Length, 1, 1);

        buffer.GetData(outputBoids);

        DrawBoids(outputBoids);

    }

    private void DrawBoids(Boid[] outputBoids)
    {
        for (int i = 0; i < outputBoids.Length; i++)
        {
            boidObjects[i].transform.position = outputBoids[i].pos;
            boidObjects[i].transform.rotation = Quaternion.LookRotation(outputBoids[i].vel);
        }

        inputBoids = outputBoids;
    }


    private void OnValidate()
    {
        SetupShader();
    }

    struct Boid
    {
        public Vector3 pos;
        public Vector3 vel;
        public Vector3 multipliers;
    }
}

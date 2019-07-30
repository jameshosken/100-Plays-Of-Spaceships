using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimplePopulationControl : MonoBehaviour
{

    [SerializeField] SimpleDNASequence minStartGenes;
    [SerializeField] SimpleDNASequence maxStartGenes;

    [SerializeField] GameObject boidTemplate;
    [SerializeField] float radius = 5f;

    [SerializeField] float populationNumber = 100;
    [SerializeField] float crossoverChance = 0.5f;
    [SerializeField] float mutationChance = 0.005f;
    [SerializeField] float minMutation;
    [SerializeField] float maxMutation;

    [SerializeField] float timePerGeneration;

    [SerializeField] Text generationCountText;
    float checkpoint = 0;
    int generationCounter = 0;

    List<GameObject> currentPopulation = new List<GameObject>();

    List<SimpleDNASequence> currentGenePool = new List<SimpleDNASequence>();


    // Start is called before the first frame update
    void Start()
    {
        CreatePopFromRandom();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - checkpoint > timePerGeneration)
        {
            print("TIME'S UP!");
            checkpoint = Time.time;
            CreateNewGeneration();
        }
    }

    private void CreateNewGeneration()
    {

        //Fitness Function

        List<float> fitnesses = new List<float>();

        Vector3 goal = GameObject.Find("EndGoal").transform.position;

        float maxDist = Vector3.Distance(transform.position, goal);

        for (int i = 0; i < currentPopulation.Count; i++)
        {
            float dist = Vector3.Distance(currentPopulation[i].transform.position, goal);

            float fit = Remap(dist, 0, maxDist, 100, 1);
            if(fit < 1)
            {
                fit = 1;
            }
            fitnesses.Add(fit);
        }

        print("FITNESS COUNT: " + fitnesses.Count.ToString());

        //GenePool Generation

        List<SimpleDNASequence> genePool = new List<SimpleDNASequence>();

        //Create large pool of genes to select from
        for (int i = 0; i < fitnesses.Count; i++)
        {
            for (int c = 0; c < fitnesses[i]; c++)
            {
                genePool.Add(currentGenePool[i]);
            }
        }

        genePool = Crossover(genePool);

        genePool = Mutation(genePool);


        CreateGenerationFromGenePool(genePool);

        generationCounter++;
        generationCountText.text = "Generation: " + generationCounter.ToString();
    }

    private void CreateGenerationFromGenePool(List<SimpleDNASequence> genePool)
    {
        print("Removing current pop");
        for (int i = currentPopulation.Count - 1; i >= 0; i--)
        {
            GameObject toRemove = currentPopulation[i];
            currentPopulation.RemoveAt(i);
            GameObject.Destroy(toRemove);
        }

        currentPopulation.Clear();
        currentGenePool.Clear();

        print("Creating new pop");
        for (int i = 0; i < populationNumber; i++)
        {
            print("Select genes from pool");

            SimpleDNASequence gene = SelectRandomFromGenePool(genePool);
            print("Add to current genes");
            currentGenePool.Add(gene);

            print("Instantiate");
            GameObject boid = Instantiate(boidTemplate) as GameObject;

            Vector3 startPos = new Vector3(
                 UnityEngine.Random.Range(-radius, radius),
                 0,
                 UnityEngine.Random.Range(-radius, radius));

            Vector3 startRot = new Vector3(
                0,
                UnityEngine.Random.Range(-180, 180),
                0);

            boid.transform.position = startPos;
            boid.transform.rotation = Quaternion.Euler(startRot);

            SimpleBoid boidScript = boid.GetComponent<SimpleBoid>();

            boidScript.ApplyDNASequence(gene);

            print("Add to pop");
            currentPopulation.Add(boid);

        }
    }

    private List<SimpleDNASequence> Mutation(List<SimpleDNASequence> genePool)
    {
        for (int i = 0; i < genePool.Count; i++)
        {


            if (UnityEngine.Random.Range(0f, 1.0f) < mutationChance)
            {
                print("Mutation Acc");
                genePool[i].acceleration *= UnityEngine.Random.Range(minMutation, maxMutation);
            }
            if (UnityEngine.Random.Range(0f, 1.0f) < mutationChance)
            {
                print("Mutation detectionRad");
                genePool[i].detectionRadius *= UnityEngine.Random.Range(minMutation, maxMutation);
            }
            if (UnityEngine.Random.Range(0f, 1.0f) < mutationChance)
            {
                print("Mutation maxspeed");
                genePool[i].maxSpeed *= UnityEngine.Random.Range(minMutation, maxMutation);
            }
            if (UnityEngine.Random.Range(0f, 1.0f) < mutationChance)
            {
                print("Mutation rotation");
                genePool[i].rotationSpeed *= UnityEngine.Random.Range(minMutation, maxMutation);
            }

        }

        return genePool;
    }

    private List<SimpleDNASequence> Crossover(List<SimpleDNASequence> genePool)
    {
        List<SimpleDNASequence> copy = genePool;

        copy.Reverse();

        for (int i = 0; i < genePool.Count; i++)
        {


            if (UnityEngine.Random.Range(0f, 1f) < crossoverChance)
            {
                print("Crossover Acc");
                genePool[i].acceleration = copy[i].acceleration;
            }

            if (UnityEngine.Random.Range(0f, 1f) < crossoverChance)
            {
                print("Crossover detectionRad");
                genePool[i].detectionRadius = copy[i].detectionRadius;
            }

            if (UnityEngine.Random.Range(0f, 1f) < crossoverChance)
            {
                print("Crossover maxspeed");
                genePool[i].maxSpeed = copy[i].maxSpeed;
            }

            if (UnityEngine.Random.Range(0f, 1f) < crossoverChance)
            {
                print("Crossover rotation");
                genePool[i].rotationSpeed = copy[i].rotationSpeed;
            }

        }

        return genePool;
    }


    void CreatePopFromRandom()
    {
        for (int i = 0; i < populationNumber; i++)
        {
            currentGenePool.Add(GenerateRandomGenes());

            GameObject boid = Instantiate(boidTemplate) as GameObject;

            Vector3 startPos = new Vector3(
                UnityEngine.Random.Range(-radius, radius),
                0,
                UnityEngine.Random.Range(-radius, radius));

            Vector3 startRot = new Vector3(
                0,
                UnityEngine.Random.Range(-180, 180),
                0);

            boid.transform.position = startPos;
            boid.transform.rotation = Quaternion.Euler(startRot);

            SimpleBoid boidScript = boid.GetComponent<SimpleBoid>();

            boidScript.ApplyDNASequence(currentGenePool[i]);

            currentPopulation.Add(boid);

        }
    }

    private SimpleDNASequence GenerateRandomGenes()
    {
        float detectionRadius = UnityEngine.Random.Range(minStartGenes.detectionRadius, maxStartGenes.detectionRadius);
        float rotationSpeed = UnityEngine.Random.Range(minStartGenes.rotationSpeed, maxStartGenes.rotationSpeed);
        float acceleration = UnityEngine.Random.Range(minStartGenes.acceleration, maxStartGenes.acceleration);
        float maxSpeed = UnityEngine.Random.Range(minStartGenes.maxSpeed, maxStartGenes.maxSpeed);

        SimpleDNASequence dnaSequence = new SimpleDNASequence(rotationSpeed, acceleration, maxSpeed, detectionRadius);

        return dnaSequence;
    }

    private SimpleDNASequence SelectRandomFromGenePool(List<SimpleDNASequence> genePool)
    {

        int selection = UnityEngine.Random.Range(0, genePool.Count);
        print("Selection: " + selection.ToString());

        return (genePool[selection]);
    }

    public static float Remap( float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}

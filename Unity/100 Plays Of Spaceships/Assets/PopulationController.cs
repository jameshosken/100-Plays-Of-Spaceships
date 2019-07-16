using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulationController : MonoBehaviour
{
    [SerializeField] DNASequence minStartGenes;

    [SerializeField] DNASequence maxStartGenes;
    [SerializeField] GameObject boidTemplate;
    [SerializeField] int populationNumber;

    [SerializeField] float startRadius;

    [SerializeField] float timePerGeneration;
    float checkpoint = 0;

    List<GameObject> currentPopulation = new List<GameObject>();

    List<DNASequence> currentPopulationGenes = new List<DNASequence>();

    [SerializeField] Text generationCount;

    [SerializeField] float crossoverChance = 0.2f;
    [SerializeField] float mutationChance = 0.01f;
    [SerializeField] float minMutation = 0f;
    [SerializeField] float maxMutation = 2f;

    int generationCounter = 0;

    Color[] colours = { Color.blue, Color.red, Color.cyan, Color.green, Color.yellow, Color.magenta };


    // Start is called before the first frame update
    void Start()
    {

        //First Wave
        for (int i = 0; i < populationNumber; i++)
        {
            currentPopulationGenes.Add(GenerateRandomGenes());

            GameObject boid = Instantiate(boidTemplate) as GameObject;

            Vector3 startPos = new Vector3(
                UnityEngine.Random.Range(-startRadius, startRadius),
                UnityEngine.Random.Range(-startRadius, startRadius),
                UnityEngine.Random.Range(-startRadius, startRadius));

            Vector3 startRot = new Vector3(
                UnityEngine.Random.Range(-180, 180),
                UnityEngine.Random.Range(-180, 180),
                UnityEngine.Random.Range(-180, 180));

            boid.transform.position = startPos;
            boid.transform.rotation = Quaternion.Euler(startRot);
            
            GA_Boid_D36 boidScript = boid.GetComponent<GA_Boid_D36>();

            boidScript.ApplyDNASequence(currentPopulationGenes[i]);

            currentPopulation.Add(boid);

        }
    }

    private DNASequence GenerateRandomGenes()
    {
        Vector3 forceDirection = new Vector3(
                UnityEngine.Random.Range(-180, 180),
                UnityEngine.Random.Range(-180, 180),
                UnityEngine.Random.Range(-180, 180));
        float detectionSphereRadius = UnityEngine.Random.Range(minStartGenes.detectionSphereRadius, maxStartGenes.detectionSphereRadius);
        float rotationSpeed= UnityEngine.Random.Range(minStartGenes.rotationSpeed, maxStartGenes.rotationSpeed);
        float dangerMultiplier = UnityEngine.Random.Range(minStartGenes.dangerMultiplier, maxStartGenes.dangerMultiplier);
        float acceleration = UnityEngine.Random.Range(minStartGenes.acceleration, maxStartGenes.acceleration);
        float maxSpeed = UnityEngine.Random.Range(minStartGenes.maxSpeed, maxStartGenes.maxSpeed);
        float overShootMultiplier = UnityEngine.Random.Range(minStartGenes.overShootMultiplier, maxStartGenes.overShootMultiplier);
        bool ignoreOtherBoids = maxStartGenes.ignoreOtherBoids;
        Color colour = Color.Lerp(minStartGenes.colour, maxStartGenes.colour, UnityEngine.Random.Range(0f, 1f));

        DNASequence genes = new DNASequence(
            forceDirection,
            detectionSphereRadius,
            rotationSpeed,
            dangerMultiplier,
            acceleration,
            maxSpeed,
            overShootMultiplier,
            ignoreOtherBoids,
            colour);

        return genes;
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
        print("GENERATING NEW GENERATION");
        ///////////
        //FITNESS//
        ///////////
        
        GameObject goal = GameObject.Find("End Goal");

        List<float> fitnesses = new List<float>();

        float minFit = float.MaxValue;

        for (int i = 0; i < currentPopulation.Count; i++)
        {
            float dist = Vector3.Distance(currentPopulation[i].transform.position, goal.transform.position);
            float fit = 1 / dist;

            if (fit < minFit)
            {
                minFit = fit;
            }

            fitnesses.Add(fit);
        }

        print("<<<<< MIN FIT: " + minFit.ToString());
        print("----------------------");
        print("----------------------");
        print("----------------------");

        List<int> fitnessNormalised = new List<int>();
        ///////////////// Normalising does not do anything atm
        for (int i = 0; i < fitnesses.Count; i++)
        {

            //Convert fitnesses to integers with lowest fitness = 1;
            float normalised = Mathf.Pow((fitnesses[i] / minFit), 1.2f);

            fitnessNormalised.Add((int)normalised);
            print("FITNESS OF " + i.ToString() + ": " + (int)normalised);
        }

        print("----------------------");
        print("----------------------");
        print("----------------------");

        List<DNASequence> genePool = new List<DNASequence>();

        //Create large pool of genes to select from
        for (int i = 0; i < fitnessNormalised.Count; i++)
        {
            for (int c = 0; c < fitnessNormalised[i]; c++)
            {
                genePool.Add(currentPopulationGenes[i]);
            }
        }


        /////////////
        //SELECTION//
        /////////////

        genePool = Crossover(genePool);

        genePool = Mutation(genePool);

        CreateGenerationFromGenePool(genePool);

        generationCounter++;
        generationCount.text = "Generation: " + generationCounter.ToString();
    }

    private List<DNASequence> Crossover(List<DNASequence> genePool)
    {
        
        List<DNASequence> copy = genePool;

        copy.Reverse();

        for (int i = 0; i < genePool.Count; i++)
        {

            if (UnityEngine.Random.Range(0f, 1f) < crossoverChance)
            {
                print("Crossover Vec");
                genePool[i].forceDirection = copy[i].forceDirection;
            }

            if (UnityEngine.Random.Range(0f, 1f) < crossoverChance)
            {
                print("Crossover Acc");
                genePool[i].acceleration = copy[i].acceleration;
            }
            //if (UnityEngine.Random.Range(0f, 1f) < crossoverChance)
            //{
            //    print("Crossover col");
            //    genePool[i].colour = copy[i].colour;
            //}

            if (UnityEngine.Random.Range(0f, 1f) < crossoverChance)
            {
                print("Crossover danger");
                genePool[i].dangerMultiplier = copy[i].dangerMultiplier;
            }
            if (UnityEngine.Random.Range(0f, 1f) < crossoverChance)
            {
                print("Crossover detectionRad");
                genePool[i].detectionSphereRadius = copy[i].detectionSphereRadius;
            }
            if (UnityEngine.Random.Range(0f, 1f) < crossoverChance)
            {
                print("Crossover ignor");
                genePool[i].ignoreOtherBoids = copy[i].ignoreOtherBoids;
            }
            if (UnityEngine.Random.Range(0f, 1f) < crossoverChance)
            {
                print("Crossover maxspeed");
                genePool[i].maxSpeed = copy[i].maxSpeed;
            }

            if (UnityEngine.Random.Range(0f, 1f) < crossoverChance)
            {
                print("Crossover overshoot");
                genePool[i].overShootMultiplier = copy[i].overShootMultiplier;
            }

            if (UnityEngine.Random.Range(0f, 1f) < crossoverChance)
            {
                print("Crossover rotation");
                genePool[i].rotationSpeed = copy[i].rotationSpeed;
            }

        }

        return genePool;
    }

    private List<DNASequence> Mutation(List<DNASequence> genePool)
    {


        for (int i = 0; i < genePool.Count; i++)
        {

            if (UnityEngine.Random.Range(0f, 1.0f) < mutationChance)
            {
                print("Mutation Vec");
                genePool[i].forceDirection = new Vector3(
                UnityEngine.Random.Range(-180, 180),
                UnityEngine.Random.Range(-180, 180),
                UnityEngine.Random.Range(-180, 180)); 
            }

            if (UnityEngine.Random.Range(0f, 1.0f) < mutationChance)
            {
                print("Mutation Acc");
                genePool[i].acceleration *= UnityEngine.Random.Range(minMutation, maxMutation) ;
            }
            if (UnityEngine.Random.Range(0f, 1.0f) < mutationChance)
            {
                print("Mutation col");
                int index = UnityEngine.Random.Range(0, colours.Length);
                genePool[i].colour = colours[index];
            }

            if (UnityEngine.Random.Range(0f, 1.0f) < mutationChance)
            {
                print("Mutation danger");
                genePool[i].dangerMultiplier *= UnityEngine.Random.Range(minMutation, maxMutation);
            }
            if (UnityEngine.Random.Range(0f, 1.0f) < mutationChance)
            {
                print("Mutation detectionRad");
                genePool[i].detectionSphereRadius *= UnityEngine.Random.Range(minMutation, maxMutation);
            }
            if (UnityEngine.Random.Range(0f, 1.0f) < mutationChance)
            {
                print("Mutation ignor");
                genePool[i].ignoreOtherBoids = !genePool[i].ignoreOtherBoids;
            }
            if (UnityEngine.Random.Range(0f, 1.0f) < mutationChance)
            {
                print("Mutation maxspeed");
                genePool[i].maxSpeed *= UnityEngine.Random.Range(minMutation, maxMutation);
            }

            if (UnityEngine.Random.Range(0f, 1.0f) < mutationChance)
            {
                print("Mutation overshoot");
                genePool[i].overShootMultiplier *= UnityEngine.Random.Range(minMutation, maxMutation);
            }

            if (UnityEngine.Random.Range(0f, 1.0f) < mutationChance)
            {
                print("Mutation rotation");
                genePool[i].rotationSpeed *= UnityEngine.Random.Range(minMutation, maxMutation);
            }

        }

        return genePool;
    }

    void CreateGenerationFromGenePool(List<DNASequence> genePool)
    {
        print("Removing current pop");
        for (int i = currentPopulation.Count-1; i >= 0; i--)
        {
            GameObject toRemove = currentPopulation[i];
            currentPopulation.RemoveAt(i);
            GameObject.Destroy(toRemove);
        }

        currentPopulation.Clear();
        currentPopulationGenes.Clear();

        print("Creating new pop");
        for (int i = 0; i < populationNumber; i++)
        {
            print("Select genes from pool");

            DNASequence gene = SelectRandomFromGenePool(genePool);
            print("Add to current genes");
            currentPopulationGenes.Add(gene);

            print("Instantiate");
            GameObject boid = Instantiate(boidTemplate) as GameObject;

            Vector3 startPos = new Vector3(
                UnityEngine.Random.Range(-startRadius, startRadius),
                UnityEngine.Random.Range(-startRadius, startRadius),
                UnityEngine.Random.Range(-startRadius, startRadius));

            Vector3 startRot = new Vector3(
                UnityEngine.Random.Range(-180, 180),
                UnityEngine.Random.Range(-180, 180),
                UnityEngine.Random.Range(-180, 180));

            boid.transform.position = startPos;
            boid.transform.rotation = Quaternion.Euler(startRot);

            GA_Boid_D36 boidScript = boid.GetComponent<GA_Boid_D36>();

            boidScript.ApplyDNASequence(gene);

            print("Add to pop");
            currentPopulation.Add(boid);

        }
    }

    private DNASequence SelectRandomFromGenePool(List<DNASequence> genePool)
    {
        
        int selection = UnityEngine.Random.Range(0, genePool.Count);
        print("Selection: " + selection.ToString());

        return (genePool[selection]);
    }
    
}

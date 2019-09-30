using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateSolarSystem : MonoBehaviour
{

    [SerializeField] GameObject EarthHandlerObj;
    [SerializeField] GameObject GasHandlerObj;
    [SerializeField] GameObject SunObj;

    [SerializeField] Vector2 numPlanetsRange = new Vector2(2, 5);
    [SerializeField] float gasChance = 0.5f;
    [SerializeField] Vector2 sunSize = new Vector2(0.2f, 0.8f);

    [SerializeField] Vector2 minMaxSolarDistance = new Vector2(80f, 180f);

    GameObject center;
    GameObject sun;
    List<GameObject> planets = new List<GameObject>();



    // Start is called before the first frame update
    void Start()
    {
        GenerateNewSolarSystem();
    }


    public void GenerateNewSolarSystem()
    {
        
        
        GenerateCenter();
        GenerateSun();


        int numPlanets = (int)UnityEngine.Random.Range(numPlanetsRange.x, numPlanetsRange.y);
        for(int i = 0; i < numPlanets; i++)
        {
            float radius = 0;
            Vector3 newPlanetPosition = new Vector3(
                UnityEngine.Random.Range(-minMaxSolarDistance.x, minMaxSolarDistance.x),
                0, 
                UnityEngine.Random.Range(-minMaxSolarDistance.y, minMaxSolarDistance.y)
            );

            radius = Vector3.Distance(Vector3.zero, newPlanetPosition);
            while (radius < minMaxSolarDistance.x && radius > minMaxSolarDistance.y)
            {
                newPlanetPosition = new Vector3(
                UnityEngine.Random.Range(-minMaxSolarDistance.x, minMaxSolarDistance.x),
                0,
                UnityEngine.Random.Range(-minMaxSolarDistance.y, minMaxSolarDistance.y)
                );

                radius = Vector3.Distance(Vector3.zero, newPlanetPosition);
            }

            //Pos = newPlanetPosition;
            if(UnityEngine.Random.Range(0f,1f) < gasChance)
            {
                GenerateGasHandler(newPlanetPosition);
               

            }
            else
            {
                GenerateEarthHandler(newPlanetPosition);
            }
            
        }
    }

    private void GenerateCenter()
    {
        center = new GameObject("SolarCenter");
        center.transform.parent = transform;
        
    }

    private void GenerateSun()
    {
        sun = Instantiate(SunObj);

        sun.transform.parent = center.transform;
        center.transform.localPosition = Vector3.zero;
        sun.transform.localScale = Vector3.one * UnityEngine.Random.Range(sunSize.x, sunSize.y);

    }

    private void GenerateEarthHandler(Vector3 newPlanetPosition)
    {
        GameObject earth = Instantiate(EarthHandlerObj);
        earth.transform.parent = center.transform;
        earth.transform.localPosition = newPlanetPosition;
    }

    private void GenerateGasHandler(Vector3 newPlanetPosition)
    {
        GameObject gas = Instantiate(GasHandlerObj);
        gas.transform.parent = center.transform;
        gas.transform.localPosition = newPlanetPosition;
    }

    public Vector3 GetCenter()
    {
        return center.transform.position;
    }
}

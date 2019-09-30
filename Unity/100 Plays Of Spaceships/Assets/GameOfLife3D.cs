using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOfLife3D : MonoBehaviour
{

    [SerializeField] GameObject cubeTemplate;
    [SerializeField] int[] size;
    [SerializeField] float startActiveChance = 1f;
    [SerializeField] float delay = 0.1f;

    [ColorUsage(true, true)]
    [SerializeField] Color birthColour;

    [ColorUsage(true, true)]
    [SerializeField] Color DeathColour;

    [ColorUsage(true, true)]
    [SerializeField] Color surviveColour;

    [SerializeField] int[] survivalNeighbours;
    [SerializeField] int[] birthNeighbours;

    GameObject[,,] map;
    int cycle = 0;

    [SerializeField] float randomKillPercent = 50f;
    [SerializeField] float randomKillInterval = 2f;

    // Start is called before the first frame update
    void Start()
    {

        SetupMap();

        ActivateRandomCubes();

        InvokeRepeating("RandomKill", randomKillInterval, randomKillInterval);
    }


    void RandomKill()
    {
        for (int x = 0; x < size[0]; x++)
        {
            for (int y = 0; y < size[1]; y++)
            {
                for (int z = 0; z < size[2]; z++)
                {
                    if (map[x, y, z].activeInHierarchy)
                    {
                        if (UnityEngine.Random.Range(0, 100) < randomKillPercent)
                        {
                            map[x, y, z].SetActive(false);
                        }
                    }
                }
            }
        }
    }
  
    private void SetupMap()
    {

        map = new GameObject[size[0], size[1], size[2]];

        for (int x = 0; x < size[0]; x++)
        {
            for (int y = 0; y < size[1]; y++)
            {
                for (int z = 0; z < size[2]; z++)
                {
                    GameObject clone = Instantiate(cubeTemplate);
                    clone.transform.position = new Vector3(x - size[0] / 2, y - size[1] / 2, z - size[2] / 2 );
                    clone.SetActive(false);


                    map[x, y, z] = clone;
                }
            }
        }
    }
    private void ActivateRandomCubes()
    {
        for (int x = 0; x < size[0]; x++)
        {
            for (int y = 0; y < size[1]; y++)
            {
                for (int z = 0; z < size[2]; z++)
                {
                    if (UnityEngine.Random.Range(0f, 100f) < startActiveChance)
                    {
                        map[x, y, z].SetActive(true);
                    }
                }
            }
        }

        StartCoroutine(Cycler());

    }
    IEnumerator Cycler()
    {

        //For a space that is 'populated':
        //  Each cell with one or no neighbors dies, as if by solitude.
        //  Each cell with four or more neighbors dies, as if by overpopulation.
        //  Each cell with two or three neighbors survives.
        //For a space that is 'empty' or 'unpopulated'
        //  Each cell with three neighbors becomes populated.
        while (true)
        {
            print("Cycle: " + cycle);
            bool[,,] activationMap = GetActivationMapFromMap();

            int dead = 0;
            for (int x = 0; x < size[0]; x++)
            {
                for (int y = 0; y < size[1]; y++)
                {
                    for (int z = 0; z < size[2]; z++)
                    {
                        int neighbours = GetNeighbours(x, y, z);

                        if (map[x, y, z].activeInHierarchy)
                        {
                            //////////////
                            /// RULES ///
                            /////////////
                            ///
                            //Alive
                            if (Array.IndexOf(survivalNeighbours, neighbours) >= 0)
                            {
                                activationMap[x, y, z] = true; // Survive
                                map[x, y, z].GetComponent<Renderer>().materials[0].SetColor("_EmissionColor", surviveColour);
                                
                            }
                            else
                            {
                                activationMap[x, y, z] = false;    //Kill
                            }
                        }
                        else
                        {
                            dead++;
                            //Dead
                            if (Array.IndexOf(birthNeighbours, neighbours) >= 0)
                            {
                                activationMap[x, y, z] = true; // Birth
                                map[x, y, z].GetComponent<Renderer>().materials[0].SetColor("_EmissionColor", birthColour);
                            }
                            else
                            {
                                activationMap[x, y, z] = false;
                            }
                        }
                    }
                }
            }

            SetMapFromActivation(activationMap);
            cycle += 1;
            
            yield return new WaitForSeconds(delay);
        }
    }

    

    private bool[,,] GetActivationMapFromMap()
    {
        bool[,,] activeMap = new bool[size[0], size[1], size[2]];

        for (int x = 0; x < size[0]; x++)
        {
            for (int y = 0; y < size[1]; y++)
            {
                for (int z = 0; z < size[2]; z++)
                {
                    if (map[x, y, z].activeInHierarchy)
                    {
                        activeMap[x, y, z] = true;
                    }
                    else
                    {
                        activeMap[x, y, z] = false;
                    }
                }
            }
        }
        return activeMap;

    }

    private void SetMapFromActivation(bool[,,] activationMap)
    {

        for (int x = 0; x < size[0]; x++)
        {
            for (int y = 0; y < size[1]; y++)
            {
                for (int z = 0; z < size[2]; z++)
                {

                    map[x, y, z].SetActive(activationMap[x, y, z]);
                }
            }
        }

    }

    private int GetNeighbours(int x, int y, int z)
    {
        int neighbours = 0; 
        for (int xOff = -1; xOff <= 1; xOff++)
        {
            for (int yOff = -1; yOff <= 1; yOff++)
            {
                for (int zOff = -1; zOff <= 1; zOff++)
                {
                        if (xOff == 0 && yOff == 0)
                    {
                        continue; // Ignore self;
                    }
                    int neighbourX = xOff + x;
                    int neighbourY = yOff + y;
                    int neighbourZ = zOff + z;

                    if (IsWithinBounds(neighbourX, neighbourY, neighbourZ))
                    {
                        if (map[neighbourX, neighbourY, neighbourZ].activeInHierarchy)
                        {
                            neighbours++;
                        }
                    }
                }
            }
        }

        return neighbours; 
    }

    private bool IsWithinBounds(int x, int y, int z)
    {
        if(x < 0 || x >= size[0] || y < 0 || y >= size[1] || z < 0 || z >= size[2])
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}



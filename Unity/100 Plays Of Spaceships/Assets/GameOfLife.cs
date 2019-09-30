using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOfLife : MonoBehaviour
{

    [SerializeField] GameObject cubeTemplate;
    [SerializeField] int size;
    [SerializeField] float startActiveChance = 1f;
    [SerializeField] float delay = 0.1f;

    [ColorUsage(true, true)]
    [SerializeField] Color birthColour;

    [ColorUsage(true, true)]
    [SerializeField] Color DeathColour;

    [ColorUsage(true, true)]
    [SerializeField] Color surviveColour;

    GameObject[,] map;
    int cycle = 0;

    // Start is called before the first frame update
    void Start()
    {

        SetupMap();

        ActivateRandomCubes();
    }

  
    private void SetupMap()
    {

        map = new GameObject[size, size];

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                GameObject clone = Instantiate(cubeTemplate);
                clone.transform.position = new Vector3(x - size/2, 0, y-size/2);
                clone.SetActive(false);

                
                map[x, y] = clone;
            }
        }
    }
    private void ActivateRandomCubes()
    {
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                if(UnityEngine.Random.Range(0f,100f) < startActiveChance)
                {
                    map[x, y].SetActive(true);
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
            bool[,] activationMap = GetActivationMapFromMap();

            int dead = 0;
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    int neighbours = GetNeighbours(x, y);

                    if (map[x, y].activeInHierarchy)
                    {
                        //Alive
                        if (neighbours <= 1 || neighbours >= 4)
                        {
                            activationMap[x, y] = false;    //Kill
                        }
                        else
                        {
                            activationMap[x, y] = true; // Survive
                            map[x, y].GetComponent<Renderer>().materials[0].SetColor("_EmissionColor", surviveColour);
                        }
                    }
                    else
                    {
                        dead++;
                        //Dead
                        if (neighbours == 3)
                        {
                            activationMap[x, y] = true; // Birth
                            map[x, y].GetComponent<Renderer>().materials[0].SetColor("_EmissionColor", birthColour);
                        }
                        else
                        {
                            activationMap[x, y] = false;
                        }
                    }
                }
            }

            SetMapFromActivation(activationMap);
            cycle += 1;
            
            yield return new WaitForSeconds(delay);
        }
    }

    

    private bool[,] GetActivationMapFromMap()
    {
        bool[,] activeMap = new bool[size, size];

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                if (map[x, y].activeInHierarchy)
                {
                    activeMap[x, y] = true;
                }
                else
                {
                    activeMap[x, y] = false;
                }
            }
        }
        return activeMap;

    }

    private void SetMapFromActivation(bool[,] activationMap)
    {

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {

                map[x, y].SetActive(activationMap[x, y]);
            }
        }

    }

    private int GetNeighbours(int x, int y)
    {
        int neighbours = 0; 
        for (int xOff = -1; xOff <= 1; xOff++)
        {
            for (int yOff = -1; yOff <= 1; yOff++)
            {
                if(xOff == 0 && yOff == 0)
                {
                    continue; // Ignore self;
                }
                int neighbourX = xOff + x;
                int neighbourY = yOff + y;

                if (IsWithinBounds(neighbourX, neighbourY))
                {
                    if (map[neighbourX, neighbourY].activeInHierarchy)
                    {
                        neighbours++;
                    }
                }
            }
        }

        return neighbours; 
    }

    private bool IsWithinBounds(int x, int y)
    {
        if(x < 0 || x >= size || y < 0 || y >= size){
            return false;
        }
        else
        {
            return true;
        }
    }
}



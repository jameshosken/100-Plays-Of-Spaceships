using System;
using System.Collections.Generic;
using UnityEngine;

public class HexTileSpawnerGeneric : MonoBehaviour
{

    [SerializeField] private GameObject hexTile;        //Tile prefab
    [SerializeField] private float tileRadius = 1;       //Radius of tile

    [SerializeField] private Transform viewer;
    [SerializeField] private float newTileThreshold;    //How far the player must move before evaluating new tiles
    [SerializeField] private float spawnRadius;         //Radius within which to create new tiles
    [SerializeField] float removeRadius;                //Radius to remove tiles

    private List<HexTileBase> hexTileBases = new List<HexTileBase>();   //List of all tiles created
    private Vector3 previousViewerPosition = Vector3.zero;              //Keep track of how far the player has moved

    // Start is called before the first frame update
    private void Start()
    {

        //Start with a small grid of tiles
        for (int y = -2; y < 2; y++)
        {
            for (int x = -2; x < 2; x++)
            {
                GenerateTile(x, y);
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        float movedDistance = Vector3.Distance(previousViewerPosition, viewer.position);

        if (movedDistance > newTileThreshold)
        {
            previousViewerPosition = viewer.position;
            GenerateTilesAroundLocation(viewer.position);
            RemoveTilesAroundLocation(viewer.position);
        }
    }

 

    private void GenerateTile(int x, int y)
    {
        //https://www.redblobgames.com/grids/hexagons/

        // Size = middle to corner = 1;
        // w = sqrt(3) * size and height h = 2 * size

        GameObject tile = Instantiate(hexTile) as GameObject;   

        // Get the hexTile handler script on the tile object:
        HexTileBase hexTileBase = tile.GetComponent<HexTileBase>();
        hexTileBase.SetCoords(x, y);

        //Set tile's position in the world based on coordinates:
        tile.transform.position = CoordToPoint(x, y);

        //Name the tile for the outliner
        string name = x.ToString() + ", " + y.ToString();
        tile.name = name;

        hexTileBases.Add(hexTileBase);

    }

    private void GenerateTilesAroundLocation(Vector3 location)
    {

        //Convert player location to nearest grid coordinates:
        int[] coords = PointToGridCoords(location);

        //Setarbitrarily large grid within which to generate tiles:
        int bounds = 10;


        for (int x = -bounds; x < bounds; x++)
        {
            for (int y = -bounds; y < bounds; y++)
            {
                int xOff = coords[0] + x;
                int yOff = coords[1] + y;

                float dist = CoordinateDistance(xOff, yOff, coords[0], coords[1]);

                //If point is within threshold for new tile:
                if (dist < spawnRadius)
                {
                    //Check if a tile exists at this coordinate:
                    if (CheckTile(xOff, yOff))
                    {
                        //If tile exists, do nothing
                    }
                    else
                    {
                        GenerateTile(xOff, yOff);
                    }
                }
            }
        }
    }



    private void RemoveTilesAroundLocation(Vector3 position)
    {

        //This function removes tiles further than 'removeRadius' from a certain world position

        //Check through all tiles in the list:
        for (int i = hexTileBases.Count-1; i >=0; i--)
        {
            //If distance is greater than remove threshold, remove the tile:
            if (Vector3.Distance(viewer.position, hexTileBases[i].gameObject.transform.position) > removeRadius)
            {
                HexTileBase tileBase = hexTileBases[i];
                hexTileBases.RemoveAt(i);
                GameObject.Destroy(tileBase.gameObject);
            }
        }
    }

    private bool CheckTile(int x, int y)
    {
        //This function checks if a tile exists at a coordinate 

        int[] coord = { x, y };

        for (int i = 0; i < hexTileBases.Count; i++)
        {
            int[] c = hexTileBases[i].GetCoords();
            int cx = c[0];
            int cy = c[1];

            //If a tile exists at this X and Y coord, return true
            if(cx == x && cy == y)
            {
                return true;
            }

        }
        return false;

    }

    private float CoordinateDistance(int x1, int y1, int x2, int y2)
    {
        Vector2 p1 = new Vector2(x1, y1);
        Vector2 p2 = new Vector2(x2, y2);

        return Vector2.Distance(p1, p2);
    }

    private Vector3 CoordToPoint(int x, int y)
    {
        //This function converts a grid coordinate into a world space position

        float w = tileRadius * Mathf.Sqrt(3);
        float h = tileRadius * 2f;

        float offset = ((y % 2) + 1) * .5f * w;

        float trueX = (float)x * w + offset;
        float trueY = (float)y * h * 3 / 4;

        return new Vector3(trueX, 0, trueY);
    }

    private int[] PointToGridCoords(Vector3 point)
    {
        //This function converts any world space to the closest grid coordinate.

        float w = tileRadius * Mathf.Sqrt(3);
        float h = tileRadius * 2f;

        float trueX = point.x;
        float trueY = point.z;

        float coordY = trueY / (h * 3 / 4);

        float offset = ((coordY % 2) + 1) * .5f * w;

        float coordX = (trueX / w) - offset;
        int[] coords = { (int)coordX, (int)coordY };

        return coords;

    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

public class HexTileSpawnerGeneric : MonoBehaviour
{

    [SerializeField] private GameObject hexTile;

    //[SerializeField] private int gridHeight;
    //[SerializeField] private int gridWidth;

    [SerializeField] private float spawnRadius;
    [SerializeField] float removeRadius;
    [SerializeField] private float tileRadius = 1;

    // For later (dynamics)
    [SerializeField] private Transform viewer;
    [SerializeField] private float newTileThreshold;
    private List<HexTileBase> hexTileBases = new List<HexTileBase>();

    private List<GameObject> tiles = new List<GameObject>();
    private Vector3 previousViewerPosition = Vector3.zero;
    private List<int[]> tileCoordinates = new List<int[]>();

    // Start is called before the first frame update
    private void Start()
    {
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
            GenerateTilesAroundLocation(viewer.position, 5f);
            RemoveTilesAroundLocation(viewer.position, 5f);
        }
    }

 

    private void GenerateTile(int x, int y)
    {
        //https://www.redblobgames.com/grids/hexagons/

        // Size = middle to corner = 1;
        // w = sqrt(3) * size and height h = 2 * size

        GameObject tile = Instantiate(hexTile) as GameObject;

        HexTileBase hexTileBase = tile.GetComponent<HexTileBase>();
        hexTileBase.SetCoords(x, y);

        tile.transform.position = CoordToPoint(x, y);

        string name = x.ToString() + ", " + y.ToString();
        tile.name = name;

        hexTileBases.Add(hexTileBase);
        tileCoordinates.Add(new int[] { x, y });

    }

    private void GenerateTilesAroundLocation(Vector3 location, float radius)
    {

        //Convert point to grid coords:
        int[] coords = PointToGridCoords(location);

        int bounds = 10;
        for (int x = -bounds; x < bounds; x++)
        {
            for (int y = -bounds; y < bounds; y++)
            {
                int xOff = coords[0] + x;
                int yOff = coords[1] + y;

                float dist = CoordinateDistance(xOff, yOff, coords[0], coords[1]);

                if (dist < spawnRadius)
                {
                    if (checkTile(xOff, yOff))
                    {

                    }
                    else
                    {
                        
                        GenerateTile(xOff, yOff);
                    }
                }
            }
        }
    }



    private void RemoveTilesAroundLocation(Vector3 position, float v)
    {
        for (int i = hexTileBases.Count-1; i >=0; i--)
        {

            if (Vector3.Distance(viewer.position, hexTileBases[i].gameObject.transform.position) > removeRadius)
            {
                HexTileBase tileBase = hexTileBases[i];
                hexTileBases.RemoveAt(i);
                tileCoordinates.RemoveAt(i);
                GameObject.Destroy(tileBase.gameObject);
            }
        }
    }

    private bool checkTile(int x, int y)
    {

        int[] coord = { x, y };

        for (int i = 0; i < hexTileBases.Count; i++)
        {
            int[] c = hexTileBases[i].GetCoords();
            int cx = c[0];
            int cy = c[1];

            if(cx == x && cy == y)
            {
                return true;
            }

        }
        return false;

        if (tileCoordinates.Contains(coord))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private float CoordinateDistance(int x1, int y1, int x2, int y2)
    {
        Vector2 p1 = new Vector2(x1, y1);
        Vector2 p2 = new Vector2(x2, y2);

        return Vector2.Distance(p1, p2);
    }

    private Vector3 CoordToPoint(int x, int y)
    {

        float w = tileRadius * Mathf.Sqrt(3);
        float h = tileRadius * 2f;

        float offset = ((y % 2) + 1) * .5f * w;

        float trueX = (float)x * w + offset;
        float trueY = (float)y * h * 3 / 4;

        return new Vector3(trueX, 0, trueY);
    }

    private int[] PointToGridCoords(Vector3 point)
    {

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

using System.Collections.Generic;
using UnityEngine;


public class MarchingCubeGeneratorEndless : MonoBehaviour
{
    private enum WorldType { Noise, Sphere, SphereNoise }

    [Header("General")]
    [SerializeField] private GameObject chunkTemplate;

    [Space()]
    [Header("Map")]
    [SerializeField] private WorldType worldType;
    [SerializeField] private int mapDataChunkSize;      //Use this for grid calculations

    [SerializeField] private float threshold;
    [SerializeField] private float noiseScale = 0.9f;
    [SerializeField] private float timeDelay = 0.05f;

    [Space()]
    [Header("Brush")]
    [SerializeField] private float brushRadius = 3f;
    [SerializeField] private float brushIncrement = 0.01f;
    [SerializeField] private GameObject addBrush;
    [SerializeField] private GameObject removeBrush;

    [Space()]
    [Header("Geometry and Materials")]
    [SerializeField] private Color zeroColour;
    [SerializeField] private Color fullColour;

    [Space()]
    [Header("Player")]
    [SerializeField] private Transform player;
    [SerializeField] private float radius = 3;
    [SerializeField] private float turnOffRadius = 5;
    [SerializeField] private float checkMoveThreshold = 3f;

    private Camera cam;
    private Vector3 prevPlayerPos;

    //Store map data with coordinates
    private Dictionary<int[], float[,,]> endlessMapDataChunksDictionary = new Dictionary<int[], float[,,]>(new MyEqualityComparer());

    //Store marching cube scripts with coordinates
    private Dictionary<int[], EndlessMarchingCubeChunk> endlessMarchingCubeChunksDictionary = new Dictionary<int[], EndlessMarchingCubeChunk>(new MyEqualityComparer());

    private void Start()
    {
        float startTime = Time.realtimeSinceStartup;
        cam = Camera.main;

       // GenerateMapDataAroundPlayer();

        Debug.Log("Scene generated in: " + (Time.realtimeSinceStartup - startTime).ToString() + " seconds");

        addBrush.transform.localScale = Vector3.one * brushRadius * 2;
        removeBrush.transform.localScale = Vector3.one * brushRadius * 2;


        prevPlayerPos = player.position;
    }

    private void Update()
    {

        if (Vector3.Distance(prevPlayerPos, player.position) > 3f)
        {
            GenerateMapDataAroundPlayer();
            TurnOffChunksAroundPlayer();
            prevPlayerPos = player.position;
        }


        bool change = false;
        if (Input.GetMouseButtonDown(0))
        {
            addBrush.SetActive(true);

        }

        if (Input.GetMouseButton(0))
        {
            HandleMousePress(1);
            change = true;
        }


        if (Input.GetMouseButtonUp(0))
        {
            addBrush.SetActive(false);
        }

        if (Input.GetMouseButtonDown(1))
        {
            removeBrush.SetActive(true);
        }

        if (Input.GetMouseButton(1))
        {
            HandleMousePress(-1);
            change = true;
        }


        if (Input.GetMouseButtonUp(1))
        {
            removeBrush.SetActive(false);
        }

        if (change)
        {
            //MarchingCubes.ConstructMeshFromVoxelMap(mesh, cubeWorld, threshold, 0, Vector3.zero);
        }
    }

    private void TurnOffChunksAroundPlayer()
    {
        foreach (KeyValuePair<int[], EndlessMarchingCubeChunk> chunk in endlessMarchingCubeChunksDictionary)
        {
            Vector3 chunkPos = chunk.Value.transform.position;
            if (Vector3.Distance(chunkPos, player.position) > turnOffRadius)
            {
                chunk.Value.gameObject.SetActive(false);
            }
        }
    }

    private void GenerateMapDataAroundPlayer()
    {
        Vector3 playerPos = player.position;
        int[] playerCoords = GetGridCoordFromWorldPosition(playerPos);
        Debug.Log(playerCoords);


        int search = 5;

        for (int x = -search; x <= radius; x++)
        {
            for (int y = -search; y <= search; y++)
            {
                for (int z = -search; z <= search; z++)
                {

                    if (Vector3.Distance(
                        new Vector3(x + playerCoords[0], y + playerCoords[1], z + playerCoords[2]),
                        new Vector3(playerCoords[0], playerCoords[1], playerCoords[2])) < radius)
                    {


                        int[] offsetCoord = {
                            playerCoords[0] + x,
                            playerCoords[1] + y,
                            playerCoords[2] + z
                        };

                        //If data exists there,
                        if (DataContainsKeyArray(endlessMapDataChunksDictionary, offsetCoord))
                        {
                            //print("OLD MAP: " + offsetCoord[0] + ", " + offsetCoord[1] + ", " + offsetCoord[2]);
                            endlessMarchingCubeChunksDictionary[offsetCoord].gameObject.SetActive(true);
                            //if (ChunkContainsKeyArray(endlessMarchingCubeChunksDictionary, offsetCoord))
                            //{
                            //}
                        }
                        //Otherwise create more data!
                        else
                        {
                            //print("NEW MAP: " + offsetCoord[0] + ", " + offsetCoord[1] + ", " + offsetCoord[2]);
                            float[,,] mapDataChunk = GenerateMapDataChunk(offsetCoord);
                            endlessMapDataChunksDictionary.Add(offsetCoord, mapDataChunk);
                            GenerateMarchingCubeChunk(mapDataChunk, offsetCoord);
                        }

                    }
                }
            }
        }
    }



    private float[,,] GenerateMapDataChunk(int[] offsetCoord)
    {

        float[,,] mapDataChunk = new float[mapDataChunkSize + 1, mapDataChunkSize + 1, mapDataChunkSize + 1];

        //Unlike non-endless generators, we can't use dynamic max and min vals to normalise so we have to estimate them. 
        float maxVal = 0.7f;
        float minVal = 0.3f;

        for (int x = 0; x < mapDataChunkSize + 1; x++)
        {
            for (int y = 0; y < mapDataChunkSize + 1; y++)
            {
                for (int z = 0; z < mapDataChunkSize + 1; z++)
                {

                    //add offsets to coords
                    float xOff = x + offsetCoord[0] * mapDataChunkSize;
                    float yOff = y + offsetCoord[1] * mapDataChunkSize;
                    float zOff = z + offsetCoord[2] * mapDataChunkSize;

                    //Generate noise based on offset
                    mapDataChunk[x, y, z] = PerlinNoise3D.Perlin3D(xOff * noiseScale, yOff * noiseScale, zOff * noiseScale);

                }
            }
        }

        for (int x = 0; x < mapDataChunkSize + 1; x++)
        {
            for (int y = 0; y < mapDataChunkSize + 1; y++)
            {
                for (int z = 0; z < mapDataChunkSize + 1; z++)
                {
                    mapDataChunk[x, y, z] = Map(mapDataChunk[x, y, z], minVal, maxVal, 0f, 1f);
                }
            }
        }

        return mapDataChunk;
    }

    private void GenerateMarchingCubeChunk(float[,,] mapDataChunk, int[] offset)
    {
        //Debug.Log("Adding cube at: " + offset[0].ToString() + ", " + offset[1].ToString() + ", " + offset[2].ToString() + ", ");

        GameObject chunk = Instantiate(chunkTemplate) as GameObject;
        chunk.transform.parent = transform;

        EndlessMarchingCubeChunk chunkHandler = chunk.GetComponent<EndlessMarchingCubeChunk>();
        chunkHandler.SetChunkData();

        Vector3 offsetPosition = new Vector3(offset[0] * mapDataChunkSize, offset[1] * mapDataChunkSize, offset[2] * mapDataChunkSize);
        chunkHandler.AddMapChunk(offsetPosition, mapDataChunkSize);


        chunkHandler.UpdateMesh(mapDataChunk, threshold);

        //Add this chunk to the dictionary
        endlessMarchingCubeChunksDictionary.Add(offset, chunkHandler);
    }




    private void HandleMousePress(int amount)
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            removeBrush.transform.position = hit.point;
            addBrush.transform.position = hit.point;

            IncreaseDensityAroundPoint(hit.point, amount);
        }
    }

    private void IncreaseDensityAroundPoint(Vector3 point, int amount)
    {

        List<int[]> chunksModified = new List<int[]>();
        int[] worldCoord = GetGridCoordFromWorldPosition(point);

        for (int x = (int)-brushRadius + (int)point.x; x <= brushRadius + (int)point.x; x++)
        {
            for (int y = (int)-brushRadius + (int)point.y; y <= brushRadius + (int)point.y; y++)
            {
                for (int z = (int)-brushRadius + (int)point.z; z <= (int)brushRadius + (int)point.z; z++)
                {


                    //Make a sphere
                    if (Vector3.Distance(new Vector3(x, y, z), point) < brushRadius)
                    {
                        // No longler checking bounds here. Could cause issues with index out of bounds,
                        // but hopefully the buffer zone is big enough to avoid checking at edges

                        //Here we find the world position of the corner of this cube
                        int[] chunkOrigin =
                        {
                            worldCoord[0] * mapDataChunkSize,
                            worldCoord[1] * mapDataChunkSize,
                            worldCoord[2] * mapDataChunkSize
                        };

                        //Determine where brush overlaps with neighbours
                        bool[] overlaps =
                        {
                            x < chunkOrigin[0],
                            y < chunkOrigin[1],
                            z < chunkOrigin[2],
                            x >= chunkOrigin[0] + mapDataChunkSize,
                            y >= chunkOrigin[1] + mapDataChunkSize,
                            z >= chunkOrigin[2] + mapDataChunkSize
                        };

                        //Offset chunks if brush overlaps
                        int[] offsets = { 0, 0, 0 };

                        if (overlaps[0]) offsets[0] = -1;
                        if (overlaps[1]) offsets[1] = -1;
                        if (overlaps[2]) offsets[2] = -1;
                        if (overlaps[3]) offsets[0] = 1;
                        if (overlaps[4]) offsets[1] = 1;
                        if (overlaps[5]) offsets[2] = 1;

                        int[] chunkCoord = { x + offsets[0], y + offsets[1], z + offsets[2] };

                        if (endlessMapDataChunksDictionary.ContainsKey(chunkCoord))
                        {
                            if (!chunksModified.Contains(chunkCoord))
                            {
                                chunksModified.Add(chunkCoord);
                            }

                            // Normalise since all chunks start at 0,0,0
                            int[] pointToModify =
                            {

                                x % mapDataChunkSize,
                                y % mapDataChunkSize,
                                z % mapDataChunkSize
                            };

                            endlessMapDataChunksDictionary[chunkCoord][pointToModify[0], pointToModify[1], pointToModify[2]] += brushIncrement * Time.deltaTime * amount;
                        }


                    }
                }
            }
        }
        for (int i = 0; i < chunksModified.Count; i++)
        {
            int[] coordKey = chunksModified[i];

            //Check if chunk exists at location
            if (endlessMarchingCubeChunksDictionary.ContainsKey(coordKey))
            {
                //If so, update with new data.
                endlessMarchingCubeChunksDictionary[coordKey].UpdateMesh(endlessMapDataChunksDictionary[coordKey], threshold);

            }

        }

    }

    /// <summary>
    /// Helper Functions
    /// </summary>
    /// 
    public float Map(float val, float OldMin, float OldMax, float NewMin, float NewMax)
    {

        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((val - OldMin) * NewRange) / OldRange) + NewMin;

        return (NewValue);
    }

    private int[] GetGridCoordFromWorldPosition(Vector3 position)
    {
        int[] coord = new int[3];

        coord[0] = (int)(position.x / (float)mapDataChunkSize);
        coord[1] = (int)(position.y / (float)mapDataChunkSize);
        coord[2] = (int)(position.z / (float)mapDataChunkSize);

        

        return coord;

    }

    private void OnValidate()
    {
        addBrush.transform.localScale = Vector3.one * brushRadius * 2;
        removeBrush.transform.localScale = Vector3.one * brushRadius * 2;

    }

    private bool DataContainsKeyArray(Dictionary<int[], float[,,]> dict, int[] offsetCoord)
    {
        MyEqualityComparer eq = new MyEqualityComparer();
        foreach (KeyValuePair<int[], float[,,]> chunk in dict)
        {

            if (eq.Equals(chunk.Key, offsetCoord))
            {
                return true;
            }

        }
        return false;
    }

    private bool ChunkContainsKeyArray(Dictionary<int[], EndlessMarchingCubeChunk> dict, int[] offsetCoord)
    {
        MyEqualityComparer eq = new MyEqualityComparer();
        foreach (KeyValuePair<int[], EndlessMarchingCubeChunk> chunk in dict)
        {
            if(eq.Equals(chunk.Key, offsetCoord))
            {
                return true;
            }
        
        }
        return false;
    }

}


//https://stackoverflow.com/questions/14663168/an-integer-array-as-a-key-for-dictionary
public class MyEqualityComparer : IEqualityComparer<int[]>
{
    public bool Equals(int[] x, int[] y)
    {
        if (x.Length != y.Length)
        {
            return false;
        }
        for (int i = 0; i < x.Length; i++)
        {
            if (x[i] != y[i])
            {
                return false;
            }
        }
        return true;
    }

    public int GetHashCode(int[] obj)
    {
        int result = 17;
        for (int i = 0; i < obj.Length; i++)
        {
            unchecked
            {
                result = result * 23 + obj[i];
            }
        }
        return result;
    }
}
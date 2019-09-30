﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;


[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class EndlessMarchingCubeChunk : MonoBehaviour
{
    [SerializeField] private int LOD = 0;
    private Mesh mesh;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;
    private Vector3 worldPosition;

    //private float[,,] mapChunk;

    //For ienumerator;


    private bool meshComplete = false;
    private bool isWaiting = false;

    private void Start()
    {

    }

    public void SetChunkData()
    {
        mesh = new Mesh();
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.sharedMesh = mesh;
        meshCollider = GetComponent<MeshCollider>();
        meshCollider.sharedMesh = meshFilter.sharedMesh;
    }


    public void AddMapChunk(Vector3 offsetPosition, int chunkSize)
    {

        transform.position = offsetPosition;


    }

    public void UpdateMesh(float[,,] localMap, float threshold)
    {
        ConstructMeshFromVoxelMap(mesh, localMap, threshold, LOD, worldPosition);
        
    }


    public void ConstructMeshFromVoxelMap(Mesh mesh, float[,,] map, float threshold, int lod, Vector3 offset)
    {

        MarchingCubes.ConstructMeshFromVoxelMap(mesh, map, threshold, lod, worldPosition);
        
       //StartCoroutine(CalculateMesh(mesh, map, threshold, lod, worldPosition));

    }



    public  IEnumerator CalculateMesh(Mesh mesh, float[,,] map, float threshold, int lod, Vector3 offset)
    {
        //Pause randomly
        yield return new WaitForSeconds(Random.Range(0, .5f));
        List<Vector3> vertices = new List<Vector3>();

        int mul = (int)Mathf.Pow(2, lod);

        Vector3[] corners = new Vector3[8];
        Vector3[] vertList = new Vector3[12];
        float[] cornerValues = new float[8];


        //Try +1 to add the edges between chunks


        int[] mapSize = { map.GetLength(0), map.GetLength(1), map.GetLength(2) };

        for (int i = 0; i < corners.Length; i++)
        {
            corners[i] = new Vector3();
        }

        //Loop through each dimension of the map
        for (int x = 0; x < mapSize[0] - 1; x += mul)
        {
            for (int y = 0; y < mapSize[1] - 1; y += mul)
            {
                for (int z = 0; z < mapSize[2] - 1; z += mul)
                {
                    int cubeindex = 0;

                    corners[0] = new Vector3(x, y, z);
                    corners[1] = new Vector3(x + mul, y, z);
                    corners[2] = new Vector3(x + mul, y, z + mul);
                    corners[3] = new Vector3(x, y, z + mul);
                    corners[4] = new Vector3(x, y + mul, z);
                    corners[5] = new Vector3(x + mul, y + mul, z);
                    corners[6] = new Vector3(x + mul, y + mul, z + mul);
                    corners[7] = new Vector3(x, y + mul, z + mul);

                    for (int i = 0; i < corners.Length; i++)
                    {
                        cornerValues[i] = map[(int)corners[i].x, (int)corners[i].y, (int)corners[i].z];
                    }

                    //Add bitwise value to cubeIndex (for lookup)
                    if (cornerValues[0] < threshold) cubeindex |= 1;
                    if (cornerValues[1] < threshold) cubeindex |= 2;
                    if (cornerValues[2] < threshold) cubeindex |= 4;
                    if (cornerValues[3] < threshold) cubeindex |= 8;
                    if (cornerValues[4] < threshold) cubeindex |= 16;
                    if (cornerValues[5] < threshold) cubeindex |= 32;
                    if (cornerValues[6] < threshold) cubeindex |= 64;
                    if (cornerValues[7] < threshold) cubeindex |= 128;

                    // no triangles if it is surrounded by air or surrounded by blocks
                    if (MarchTable.edges[cubeindex] == 0 || MarchTable.edges[cubeindex] == 255) continue;

                    // Find the vertices where the surface intersects the cube
                    if ((MarchTable.edges[cubeindex] & 1) == 1)
                        vertList[0] = MarchingCubes.vertexInterpolation(threshold, corners[0], corners[1], cornerValues[0], cornerValues[1]);
                    if ((MarchTable.edges[cubeindex] & 2) == 2)
                        vertList[1] = MarchingCubes.vertexInterpolation(threshold, corners[1], corners[2], cornerValues[1], cornerValues[2]);
                    if ((MarchTable.edges[cubeindex] & 4) == 4)
                        vertList[2] = MarchingCubes.vertexInterpolation(threshold, corners[2], corners[3], cornerValues[2], cornerValues[3]);
                    if ((MarchTable.edges[cubeindex] & 8) == 8)
                        vertList[3] = MarchingCubes.vertexInterpolation(threshold, corners[3], corners[0], cornerValues[3], cornerValues[0]);
                    if ((MarchTable.edges[cubeindex] & 16) == 16)
                        vertList[4] = MarchingCubes.vertexInterpolation(threshold, corners[4], corners[5], cornerValues[4], cornerValues[5]);
                    if ((MarchTable.edges[cubeindex] & 32) == 32)
                        vertList[5] = MarchingCubes.vertexInterpolation(threshold, corners[5], corners[6], cornerValues[5], cornerValues[6]);
                    if ((MarchTable.edges[cubeindex] & 64) == 64)
                        vertList[6] = MarchingCubes.vertexInterpolation(threshold, corners[6], corners[7], cornerValues[6], cornerValues[7]);
                    if ((MarchTable.edges[cubeindex] & 128) == 128)
                        vertList[7] = MarchingCubes.vertexInterpolation(threshold, corners[7], corners[4], cornerValues[7], cornerValues[4]);
                    if ((MarchTable.edges[cubeindex] & 256) == 256)
                        vertList[8] = MarchingCubes.vertexInterpolation(threshold, corners[0], corners[4], cornerValues[0], cornerValues[4]);
                    if ((MarchTable.edges[cubeindex] & 512) == 512)
                        vertList[9] = MarchingCubes.vertexInterpolation(threshold, corners[1], corners[5], cornerValues[1], cornerValues[5]);
                    if ((MarchTable.edges[cubeindex] & 1024) == 1024)
                        vertList[10] = MarchingCubes.vertexInterpolation(threshold, corners[2], corners[6], cornerValues[2], cornerValues[6]);
                    if ((MarchTable.edges[cubeindex] & 2048) == 2048)
                        vertList[11] = MarchingCubes.vertexInterpolation(threshold, corners[3], corners[7], cornerValues[3], cornerValues[7]);

                    for (int i = 0; MarchTable.triangles[cubeindex][i] != -1; i += 3)
                    {
                        //vertices.Add(createVertex(chunk, mul, vertList[MarchTable.triangles[cubeindex][i + 2]]));
                        //vertices.Add(createVertex(chunk, mul, vertList[MarchTable.triangles[cubeindex][i + 1]]));
                        //vertices.Add(createVertex(chunk, mul, vertList[MarchTable.triangles[cubeindex][i]]));

                        vertices.Add(MarchingCubes.createVertex(mapSize, offset, vertList[MarchTable.triangles[cubeindex][i + 2]]));
                        vertices.Add(MarchingCubes.createVertex(mapSize, offset, vertList[MarchTable.triangles[cubeindex][i + 1]]));
                        vertices.Add(MarchingCubes.createVertex(mapSize, offset, vertList[MarchTable.triangles[cubeindex][i]]));

                    }
                }
                
            }
            yield return null;
        }


        int[] tris = new int[vertices.Count];
        for (int i = 0; i < vertices.Count; i++)
        {
            tris[i] = i;
        }

        mesh.Clear();
        mesh.SetVertices(vertices); 
        mesh.SetTriangles(tris, 0); 
        mesh.RecalculateNormals(); 
        meshFilter.sharedMesh = mesh; 
        meshCollider.sharedMesh = meshFilter.sharedMesh;
        yield return null;
    }

}

﻿using UnityEngine;


//Tutorial: Procedural Landmass Generation ep 5
public static class MeshGenerator
{
    public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplier, AnimationCurve _heightCurve, int levelOfDetail)
    {
        int meshSimplificationIncrement = levelOfDetail * 2;
        if (meshSimplificationIncrement == 0)
        {
            meshSimplificationIncrement = 1;
        }

        int borderedSize = heightMap.GetLength(0);
        int meshSize = borderedSize - 2*meshSimplificationIncrement;
        int mesSizeUnsimplified = borderedSize - 2;
        
        
        float topLeftX = (mesSizeUnsimplified - 1f) / -2f;
        float topLeftZ = (mesSizeUnsimplified - 1f) / 2f;

        AnimationCurve heightCurve = new AnimationCurve(_heightCurve.keys);


        

        
        int verticesPerLine = (meshSize - 1) / (meshSimplificationIncrement) + 1;


        MeshData meshData = new MeshData(verticesPerLine);

        int[,] vertexIndicesMap = new int[borderedSize, borderedSize];

        int meshVertexIndex = 0;
        int borderVertexIndex = -1;

        for (int y = 0; y < borderedSize; y += meshSimplificationIncrement)
        {
            for (int x = 0; x < borderedSize; x += meshSimplificationIncrement)
            {
                bool isBorderIndex = y == 0 || y == borderedSize - 1 || x == 0 || x == borderedSize - 1;
                if (isBorderIndex)
                {
                    vertexIndicesMap[x, y] = borderVertexIndex;
                    borderVertexIndex--;
                }
                else
                {
                    vertexIndicesMap[x, y] = meshVertexIndex;
                    meshVertexIndex++;
                }
            }
        }

        for (int y = 0; y < borderedSize; y += meshSimplificationIncrement)
        {
            for (int x = 0; x < borderedSize; x += meshSimplificationIncrement)
            {
                int vertexIndex = vertexIndicesMap[x, y];

                Vector2 percent = new Vector2((x - meshSimplificationIncrement) / (float)meshSize, (y - meshSimplificationIncrement) / (float)meshSize);
                float height = heightCurve.Evaluate(heightMap[x, y]) * heightMultiplier;
                Vector3 vertexPosition = new Vector3(topLeftX + percent.x * mesSizeUnsimplified, height, topLeftZ - percent.y * mesSizeUnsimplified);

                meshData.AddVertex(vertexPosition, percent, vertexIndex);
                
                //Ignore map edges
                if (x < borderedSize - 1 && y < borderedSize - 1)
                {
                    int a = vertexIndicesMap[x, y];
                    int b = vertexIndicesMap[x + meshSimplificationIncrement, y];
                    int c = vertexIndicesMap[x, y+ meshSimplificationIncrement];
                    int d = vertexIndicesMap[x+meshSimplificationIncrement, y+meshSimplificationIncrement];

                    meshData.AddTriangle(a,d,c);
                    meshData.AddTriangle(d,a,b);
                }
                vertexIndex++;
            }

        }
        return meshData;

    }
}



public class MeshData
{
    Vector3[] vertices;
    int[] triangles;
    Vector2[] uvs;

    Vector3[] bordervertices;
    int[] borderTriangles;

    
    private int triangleIndex;
    int borderTriangleIndex;
    public MeshData(int verticesPerLine)
    {
        vertices = new Vector3[verticesPerLine * verticesPerLine];
        triangles = new int[(verticesPerLine - 1) * (verticesPerLine - 1) * 6];
        uvs = new Vector2[verticesPerLine * verticesPerLine];

        bordervertices = new Vector3[verticesPerLine*4+4];
        borderTriangles = new int[24*verticesPerLine];
    }

    public void AddVertex(Vector3 vertexPos, Vector2 uv, int vertexIntex)
    {
        if(vertexIntex < 0)
        {
            bordervertices[-vertexIntex - 1] = vertexPos;
        }
        else
        {
            vertices[vertexIntex] = vertexPos;
            uvs[vertexIntex] = uv;
        }
    }

    public void AddTriangle(int a, int b, int c)
    {
        if(a < 0 || b < 0 || c < 0)
        {
            borderTriangles[borderTriangleIndex] = a;
            borderTriangles[borderTriangleIndex + 1] = b;
            borderTriangles[borderTriangleIndex + 2] = c;
            borderTriangleIndex += 3;
        }
        else
        {
            triangles[triangleIndex] = a;
            triangles[triangleIndex + 1] = b;
            triangles[triangleIndex + 2] = c;
            triangleIndex += 3;
        }

        
    }

    private Vector3[] CalculateNormals()
    {
        Vector3[] vertexNormals = new Vector3[vertices.Length];
        int triangleCount = triangles.Length / 3;
        for (int i = 0; i < triangleCount; i++)
        {
            int normalTriangleIndex = i * 3;
            int vertexIndexA = triangles[normalTriangleIndex];
            int vertexIndexB = triangles[normalTriangleIndex + 1];
            int vertexIndexC = triangles[normalTriangleIndex + 2];

            Vector3 triangleNormal = SurfaceNormalFromIndices(vertexIndexA, vertexIndexB, vertexIndexC);
            vertexNormals[vertexIndexA] += triangleNormal;
            vertexNormals[vertexIndexB] += triangleNormal;
            vertexNormals[vertexIndexC] += triangleNormal;

        }

        int borderTriCount = borderTriangles.Length / 3;
        for (int i = 0; i < borderTriCount; i++)
        {
            int normalTriangleIndex = i * 3;
            int vertexIndexA = borderTriangles[normalTriangleIndex];
            int vertexIndexB = borderTriangles[normalTriangleIndex + 1];
            int vertexIndexC = borderTriangles[normalTriangleIndex + 2];

            Vector3 triangleNormal = SurfaceNormalFromIndices(vertexIndexA, vertexIndexB, vertexIndexC);
            if(vertexIndexA >= 0)
            {

                vertexNormals[vertexIndexA] += triangleNormal;
            }
            if (vertexIndexB >= 0)
            {

                vertexNormals[vertexIndexB] += triangleNormal;
            }
            if (vertexIndexC >= 0)
            {

                vertexNormals[vertexIndexC] += triangleNormal;
            }

        }


        for (int i = 0; i < vertexNormals.Length; i++)
        {
            vertexNormals[i].Normalize();
        }

        return vertexNormals;
    }

    private Vector3 SurfaceNormalFromIndices(int a, int b, int c)
    {

        Vector3 pointA = (a < 0) ? bordervertices[-a - 1] : vertices[a];
        Vector3 pointB = (b < 0) ? bordervertices[-b - 1] : vertices[b];
        Vector3 pointC = (c < 0) ? bordervertices[-c - 1] : vertices[c];

        Vector3 sideAB = pointB - pointA;
        Vector3 sideAC = pointC - pointA;
        return Vector3.Cross(sideAB, sideAC).normalized;
    }
    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.normals = CalculateNormals();
        return mesh;
    }

}
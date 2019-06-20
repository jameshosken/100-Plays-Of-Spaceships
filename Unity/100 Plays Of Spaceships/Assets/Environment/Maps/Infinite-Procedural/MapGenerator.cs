using System;
using System.Collections.Generic;
//using System.Threading;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    public enum DrawMode { NoiseMap, ColourMap, DrawMesh };
    public DrawMode drawMode;

    public Noise.NormaliseMode normaliseMode;
    public float normaliseEstimation = 1f;

    public const int mapChunkSize = 59; // Nicely divisibe for LODs
    [Range(0, 6)]
    public int editorPreviewLOD;
    public float noiseScale;
    public int seed;
    public int octaves;
    [Range(0, 1)]
    public float persistence;
    public float lacunatiry;

    public Vector2 offset;
    public float heightMultiplier = 10f;
    public AnimationCurve heightCurve;

    public TerrainType[] regions;

    public bool autoUpdate = true;
    private Queue<MapThreadInfo<MapData>> mapDataThreadInfoQueue = new Queue<MapThreadInfo<MapData>>();
    private Queue<MapThreadInfo<MeshData>> meshDataThreadInfoQueue = new Queue<MapThreadInfo<MeshData>>();

    public void RequestMapData(Action<MapData> callback, Vector2 center)
    {

        StartCoroutine(MapCoroutine(callback, center));

        //ThreadStart threadStart = delegate
        //{
        //    MapDataThread(callback, center);
        //};
        //new Thread(threadStart).Start();
    }

    private System.Collections.IEnumerator MapCoroutine(Action<MapData> callback, Vector2 center)
    {
        MapData mapData = GenerateMapData(center);

        lock (mapDataThreadInfoQueue)
        {
            mapDataThreadInfoQueue.Enqueue(new MapThreadInfo<MapData>(callback, mapData));
        }

        yield return null;
    }

    //IEnumerator MapDataCoroutine(Action<MapData> callback, Vector2 center)
    //{
    //    MapData mapData = GenerateMapData(center);
    //    lock (mapDataThreadInfoQueue)
    //    {
    //        mapDataThreadInfoQueue.Enqueue(new MapThreadInfo<MapData>(callback, mapData));
    //    }

    //    yield return null;
    //}

    private void MapDataThread(Action<MapData> callback, Vector2 center)
    {
        MapData mapData = GenerateMapData(center);
        lock (mapDataThreadInfoQueue)
        {
            mapDataThreadInfoQueue.Enqueue(new MapThreadInfo<MapData>(callback, mapData));
        }
    }

    private System.Collections.IEnumerator MeshCoroutine(MapData mapData, int lod, Action<MeshData> callback)
    {


        MeshData meshData = MeshGenerator.GenerateTerrainMesh(mapData.heightMap, heightMultiplier, heightCurve, lod);
        lock (meshDataThreadInfoQueue)
        {
            meshDataThreadInfoQueue.Enqueue(new MapThreadInfo<MeshData>(callback, meshData));
        }

        yield return null;
    }



    public void RequestMeshData(MapData mapData, int lod, Action<MeshData> callback)
    {
        StartCoroutine(MeshCoroutine(mapData, lod, callback));

        //ThreadStart threadStart = delegate
        //{
        //    MeshDataThread(mapData, lod, callback);
        //};
        //new Thread(threadStart).Start();
    }

    private void MeshDataThread(MapData mapData, int lod, Action<MeshData> callback)
    {
        MeshData meshData = MeshGenerator.GenerateTerrainMesh(mapData.heightMap, heightMultiplier, heightCurve, lod);
        lock (meshDataThreadInfoQueue)
        {
            meshDataThreadInfoQueue.Enqueue(new MapThreadInfo<MeshData>(callback, meshData));
        }
    }



    private void Update()
    {
        if (mapDataThreadInfoQueue.Count > 0)
        {
            for (int i = 0; i < mapDataThreadInfoQueue.Count; i++)
            {
                MapThreadInfo<MapData> threadInfo = mapDataThreadInfoQueue.Dequeue();

                threadInfo.callback(threadInfo.parameter);
            }
        }

        if (meshDataThreadInfoQueue.Count > 0)
        {
            for (int i = 0; i < meshDataThreadInfoQueue.Count; i++)
            {
                MapThreadInfo<MeshData> threadInfo = meshDataThreadInfoQueue.Dequeue();
                threadInfo.callback(threadInfo.parameter);
            }
        }
    }

    private MapData GenerateMapData(Vector2 center)
    {
        //Compensate for border
        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize + 2, mapChunkSize + 2, noiseScale, seed, octaves, persistence, lacunatiry, center + offset, normaliseMode, normaliseEstimation);


        Color[] colourMap = new Color[mapChunkSize * mapChunkSize];    //Instead of storing BW noise values, convert to colour based on regions    

        for (int y = 0; y < mapChunkSize; y++)
        {
            for (int x = 0; x < mapChunkSize; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight <= regions[i].height)
                    {
                        colourMap[y * mapChunkSize + x] = regions[i].colour;
                        break;
                    }
                }
            }
        }
        return new MapData(noiseMap, colourMap);

    }
    public void DrawMapInEditor()
    {
        MapData mapData = GenerateMapData(Vector2.zero);
        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (drawMode == DrawMode.NoiseMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(mapData.heightMap));
        }
        else if (drawMode == DrawMode.ColourMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromColourMap(mapData.colourMap, mapChunkSize, mapChunkSize));
        }
        else if (drawMode == DrawMode.DrawMesh)
        {
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(mapData.heightMap, heightMultiplier, heightCurve, editorPreviewLOD), TextureGenerator.TextureFromColourMap(mapData.colourMap, mapChunkSize, mapChunkSize));
        }
    }


    private void OnValidate()
    {

        if (lacunatiry < 1)
        {
            lacunatiry = 1;
        }
        if (octaves < 0)
        {
            octaves = 0;
        }


    }

    private struct MapThreadInfo<T>
    {
        public readonly Action<T> callback;
        public readonly T parameter;

        public MapThreadInfo(Action<T> callback, T parameter)
        {
            this.callback = callback;
            this.parameter = parameter;
        }
    }
}

[System.Serializable]
public struct TerrainType
{
    public float height;
    public Color colour;
    public string name;
}

public struct MapData
{
    public readonly float[,] heightMap;
    public readonly Color[] colourMap;

    public MapData(float[,] _heightMap, Color[] _colourMap)
    {
        this.heightMap = _heightMap;
        this.colourMap = _colourMap;
    }

}

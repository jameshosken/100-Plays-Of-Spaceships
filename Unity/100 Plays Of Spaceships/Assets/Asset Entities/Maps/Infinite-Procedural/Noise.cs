using UnityEngine;

public static class Noise
{

    public enum NormaliseMode { Local, Global };

    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, float scale, int seed, int octaves, float persistence, float lacunarity, Vector2 offset, NormaliseMode normaliseMode, float normaliseEstimation)
    {

        
        float[,] noiseMap = new float[mapWidth, mapHeight];

        float amplitude = 1;
        float freq = 1;

        float maxGlobalHeight = 0;

        System.Random prng = new System.Random(seed); //Pseudo Random Number Generator

        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) - offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);


            maxGlobalHeight += amplitude;
            amplitude *= persistence;
        }

        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;

        //Clamp scale above 0
        if (scale <= 0)
        {
            scale = 0.0001f;
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                amplitude = 1;
                freq = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (x - halfWidth + octaveOffsets[i].x) / scale * freq ;   //Heigher the freq, the further apart the sample points
                    float sampleY = (y - halfHeight + octaveOffsets[i].y) / scale * freq ;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;

                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistence;   // Amplitude decreases per cycle (0 < a < 1);
                    freq *= lacunarity;         // Freq increases per cycles     (1 > f)
                }

                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }
                noiseMap[x, y] = noiseHeight;

            }
        }

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            { if(normaliseMode == NormaliseMode.Local)
                {
                    //Local
                    noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
                }
                else
                {
                    //Global
                    float normalisedHeight = (noiseMap[x, y] + 1) / (2f * maxGlobalHeight * normaliseEstimation);
                    noiseMap[x, y] = normalisedHeight;
                    noiseMap[x, y] = Mathf.Clamp(noiseMap[x, y], 0, 1) ;
                }
            }
        }

        return noiseMap;
    }
}

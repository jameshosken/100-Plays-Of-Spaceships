using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicPaintCanvas : MonoBehaviour
{

    [SerializeField] int canvasSize;
    [SerializeField] int brushRadius;
    [SerializeField] float opacity = 0.1f;

    float[,] textureValues;

    Texture2D texture;

    Material canvas;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponent<Renderer>().materials[0];

        textureValues = new float[canvasSize, canvasSize];

        for (int x = 0; x < canvasSize; x++)
        {
            for (int y = 0; y < canvasSize; y++)
            {
                textureValues[x, y] = 0;

            }
        }
        

        texture = TextureGenerator.TextureFromHeightMap(textureValues);

        canvas.SetTexture("_PaintTex", texture);
        
    }

    public void PaintOnUVPosition(float uvX, float uvY)
    {

        int x = (int) ((float)canvasSize * uvX);
        int y = (int)((float)canvasSize * uvY);

        textureValues[x, y] = 1;

        for (int bX = -brushRadius; bX < brushRadius; bX++)
        {
            for (int bY = -brushRadius; bY < brushRadius; bY++)
            {

                float dist = Mathf.Sqrt( Mathf.Pow(((float)bX), 2f) + Mathf.Pow(((float)bY), 2f));

                if(dist > brushRadius)
                {
                    continue;
                }

                dist = dist / (float)brushRadius;

                SetValue(x + bX, y + bY, 1- dist);
            }
        }

        UpdateTexture();
    }

    private void SetValue(int x, int y, float val)
    {
        if(x < 0 || x >= canvasSize || y < 0 || y >= canvasSize)
        {
            return;
        }

        textureValues[x, y] = Mathf.Clamp01(textureValues[x, y] + val*opacity * Time.deltaTime);

    }

    private void UpdateTexture()
    {

        TextureGenerator.UpdateTextureFromHeightMap(texture, textureValues);


    }
}

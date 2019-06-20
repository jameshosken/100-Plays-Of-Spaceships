﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapDisplay : MonoBehaviour
{
    public Renderer textureRenderer;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;

    public void DrawTexture(Texture2D tex)
    {
        
        textureRenderer.sharedMaterial.SetTexture("_BaseMap", tex);
        textureRenderer.transform.localScale = new Vector3(tex.width, 1, tex.height);
    }

    public void DrawMesh(MeshData meshData, Texture2D texture)
    {
        meshFilter.sharedMesh = meshData.CreateMesh();
        meshRenderer.sharedMaterial.SetTexture("_BaseMap", texture);
    }
    
}

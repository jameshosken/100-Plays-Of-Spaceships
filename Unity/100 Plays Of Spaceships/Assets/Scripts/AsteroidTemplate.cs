using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidTemplate : MonoBehaviour
{

    [SerializeField] Mesh[] meshes;
    [SerializeField] float[] sizeRange;

    [SerializeField] GameObject deathFX;

    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    MeshCollider meshCollider;

    Mesh mesh;

    // Start is called before the first frame update
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();

        mesh = ChooseMesh(meshes);

        meshFilter.sharedMesh = mesh;
        meshCollider.sharedMesh = meshFilter.mesh;

        transform.localScale = Vector3.one * UnityEngine.Random.Range(sizeRange[0], sizeRange[1]);
    }

    private Mesh ChooseMesh(Mesh[] meshes)
    {
        int length = meshes.Length;

        return meshes[(int)UnityEngine.Random.Range(0, length)];
    }




}

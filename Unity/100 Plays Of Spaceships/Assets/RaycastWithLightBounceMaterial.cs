using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastWithLightBounceMaterial : MonoBehaviour
{
    
    [Range(-0f,4f)]
    [SerializeField] float refraction = 1;

    public float GetRefraction()
    {
        return refraction;
    }
}

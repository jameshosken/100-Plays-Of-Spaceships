using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitationalAttractor : MonoBehaviour
{
    [SerializeField] float mass = 100f;
    
    public float GetMass()
    {
        return mass;
    }
}

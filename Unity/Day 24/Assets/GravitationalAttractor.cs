using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitationalAttractor : MonoBehaviour
{
    [SerializeField] float mass = 100f;
    [SerializeField] float sphereOfInfluenceMultiplier;

    public float GetMass()
    {
        return mass;
    }

    public float GetSOI()
    {
        return sphereOfInfluenceMultiplier;
    }
}

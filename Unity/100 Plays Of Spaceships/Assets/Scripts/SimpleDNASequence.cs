using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SimpleDNASequence
{
    public float rotationSpeed;
    public float acceleration;
    public float maxSpeed;
    public float detectionRadius;

    public SimpleDNASequence(float rotationSpeed, float acceleration, float maxSpeed, float detectionRadius)
    {
        this.rotationSpeed = rotationSpeed;
        this.acceleration = acceleration;
        this.maxSpeed = maxSpeed;
        this.detectionRadius = detectionRadius;
    }
}
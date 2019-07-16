using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Allow variables to appear in inspector
[System.Serializable]
public class DNASequence
{
    public Vector3 forceDirection;
    public float detectionSphereRadius;
    public float rotationSpeed;
    public float dangerMultiplier;
    public float acceleration;
    public float maxSpeed;
    public float overShootMultiplier;
    public Color colour;

    public bool ignoreOtherBoids;

    public DNASequence(
        Vector3 forceDirection,
        float detectionSphereRadius, 
        float rotationSpeed, 
        float dangerMultiplier, 
        float acceleration, 
        float maxSpeed, 
        float overShootMultiplier,
        bool ignoreOtherBoids,
        Color colour)
        
    {
        this.forceDirection = forceDirection;
        this.detectionSphereRadius = detectionSphereRadius;
        this.rotationSpeed = rotationSpeed;
        this.dangerMultiplier = dangerMultiplier;
        this.acceleration = acceleration;
        this.maxSpeed = maxSpeed;
        this.overShootMultiplier = overShootMultiplier;
        this.ignoreOtherBoids = ignoreOtherBoids;
        this.colour = colour;
    }

    

}

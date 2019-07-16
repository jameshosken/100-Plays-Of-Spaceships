using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public struct ECSFlockingComponent : IComponentData
{
    public float maximumVelocity;
    public float maximumForce;
    public float desiredSparation;
    public float3 velocity;
    public float3 acceleration;
    public Unity.Mathematics.float3 moveBounds;
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct ECSMoveSpeedComponentTest : IComponentData
{
    public float moveSpeed;
    public Unity.Mathematics.float3 moveBounds;


}

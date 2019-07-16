using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct LevelComponentTest : IComponentData
{
    public float level;

    public LevelComponentTest(float level)
    {
        this.level = level;
    }
}

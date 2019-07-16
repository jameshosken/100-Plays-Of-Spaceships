using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class LevelUpSystemTest : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref LevelComponentTest lct) =>
        {
            lct.level += 1f * Time.deltaTime;
        });

    }

}

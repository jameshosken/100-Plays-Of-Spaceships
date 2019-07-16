using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

public class ECSMoverSystemTest : ComponentSystem
{

    protected override void OnUpdate()
    {
        Entities.ForEach((ref Translation translation, ref Rotation rotation, ref ECSMoveSpeedComponentTest moveSpeed) => {

            translation.Value.y += moveSpeed.moveSpeed * Time.deltaTime;

            if(translation.Value.y > moveSpeed.moveBounds.y || translation.Value.y < -moveSpeed.moveBounds.y)
            {
                moveSpeed.moveSpeed = -moveSpeed.moveSpeed;
            }

        });
    }
}

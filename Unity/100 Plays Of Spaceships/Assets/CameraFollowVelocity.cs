using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowVelocity : MonoBehaviour
{
    [SerializeField] Rigidbody target;
    [SerializeField] float followSpeed = 0.8f;
    [SerializeField] float turnSpeed = 0.9f;

    [SerializeField] Vector3 axisMultipliers;
    
    
    // Update is called once per frame
    void FixedUpdate()
    {
        //transform.position = target.position;
        transform.position = Vector3.Lerp(transform.position, target.position, followSpeed);

        Vector3 lookDirection = new Vector3(
                target.velocity.x * axisMultipliers.x,
                target.velocity.y * axisMultipliers.y,
                target.velocity.z * axisMultipliers.z
            );

        Quaternion targetRot = Quaternion.LookRotation(lookDirection);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, turnSpeed);
    }
}

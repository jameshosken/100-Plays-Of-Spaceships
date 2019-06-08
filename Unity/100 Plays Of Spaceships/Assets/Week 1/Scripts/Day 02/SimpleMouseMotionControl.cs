using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMouseMotionControl : MonoBehaviour
{
    [SerializeField] float thrustMultipier;
    [SerializeField] float lateralMultipier;
    [SerializeField] float dampeners;
    [SerializeField] float turnSpeed;
    [SerializeField] float maxSpeed;


    Rigidbody body;
    public float inverter = 1f;

    void Start()
    {
        body = GetComponentInChildren<Rigidbody>();
    }

    private void FixedUpdate()
    {
        LimitVelocity();
    }

    private void LimitVelocity()
    {
        if (body.velocity.sqrMagnitude > maxSpeed * maxSpeed)
        {
            body.velocity = body.velocity.normalized * maxSpeed;
        }
    }

    public void HandleDampeners()
    {
        
        BroadcastMessage("ApplyThrust", body.velocity.magnitude);
        body.velocity *= dampeners;
        body.angularVelocity *= dampeners;

    }

    public void HandleThrust(float thrust)
    {
        
        BroadcastMessage("ApplyThrust", thrust);
        body.AddForce(transform.forward * thrust * thrustMultipier);
        
    }

    public void HandlePRY(float pitch, float roll, float yaw)
    {

        body.AddRelativeTorque(pitch * inverter * turnSpeed, yaw  * turnSpeed, roll * -1f * turnSpeed);

    }

    public void HandleXYZ(float x, float y, float z)
    {

        body.AddRelativeForce(x* lateralMultipier, y * lateralMultipier, z * lateralMultipier);
        BroadcastMessage("ApplyLateral", x);
    }


}

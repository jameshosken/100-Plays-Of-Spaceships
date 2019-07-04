using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MoveToTargetSimple : MonoBehaviour
{

    [SerializeField] GameObject targetObject;
    [SerializeField] GameObject thrustTarget;
    [SerializeField] Rigidbody body;
    [SerializeField] float maxAcceleration;
    [SerializeField] float maxRotation;
    [SerializeField] float maxVelocity;
    [SerializeField] float rotationDampener;
    [SerializeField] float targetRadius = 2;
    [SerializeField] float targetDampener = 0.8f;


    float thrustTargetPositionMultiplier = 1;
    float minThrustAngle = 25;
    float maxSpinAngle = 60f;
    

    Transform target;
    Vector3 thrustVector;
    Vector3 thrustTargetPosition;
    Vector3 acc;
    float emission = 0;


    private bool isFacingThrustVector = false;
    // Start is called before the first frame update
    private void Start()
    {
        targetObject = Instantiate(targetObject) as GameObject;
        thrustTarget = Instantiate(thrustTarget) as GameObject;
        target = targetObject.transform;
        target.position = transform.position;

        
    }


    private void FixedUpdate()
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        if (distanceToTarget < targetRadius)       // Slow down
        {   
            body.velocity *= targetDampener;
            body.angularVelocity *= targetDampener;

            body.position = Vector3.Lerp(body.position, target.transform.position, 0.01f);
            body.rotation = Quaternion.Slerp(body.rotation, target.transform.rotation, 0.01f);
            thrustTarget.SetActive(false);
        }
        else
        {
            //
            
            DetermineThrustVector();
            DetermineRotation(thrustVector);
            DetermineAcceleration(thrustTargetPosition, thrustVector);
            thrustTarget.SetActive(true);
        }

        HandleEmission();
    }

    private void HandleEmission()
    {
        emission = Mathf.Lerp(emission, 0, 0.1f);
        BroadcastMessage("ApplyThrust", emission);
    }

    private void DetermineThrustVector()
    {

        thrustTargetPosition = Vector3.Lerp(thrustTargetPosition, (target.position - body.velocity * thrustTargetPositionMultiplier), 0.2f);

        thrustTarget.transform.position = thrustTargetPosition;

        thrustVector = (thrustTargetPosition - transform.position);
    }

    private void DetermineRotation(Vector3 thrustVector)
    {

        Vector3 avoidDirection = thrustVector;

        // Create a quaternion (rotation) based on looking down the vector from the player avoid dir
        Quaternion newRotation = Quaternion.LookRotation(avoidDirection * Time.deltaTime);


        //get the angle between transform.forward and target delta
        float angleDiff = Vector3.Angle(transform.forward, avoidDirection);

        // get its cross product, which is the axis of rotation to
        // get from one vector to the other
        Vector3 cross = Vector3.Cross(transform.forward, avoidDirection);

        // apply torque along that axis according to the magnitude of the angle.

        body.AddRelativeTorque(Vector3.forward * angleDiff, ForceMode.Force);

        if (Vector3.Angle(thrustVector, transform.forward) > 170)
        {
            Debug.Log("CONTACT");
            //body.AddRelativeTorque(Vector3.left * maxRotation, ForceMode.Impulse);

        }
        else if (Vector3.Angle(thrustVector, transform.forward) < minThrustAngle)
        {
            body.angularVelocity *= rotationDampener;
            body.rotation = Quaternion.Slerp(body.rotation, Quaternion.LookRotation(thrustVector), 0.1f);

        }
        else
        {
            body.AddTorque(cross * angleDiff * maxRotation, ForceMode.Force);
        }


        if (Vector3.Angle(transform.forward, thrustVector) < minThrustAngle)
        {
            isFacingThrustVector = true;
        }
        else
        {
            isFacingThrustVector = false;
        }

    }


    void DetermineAcceleration(Vector3 thrustTargetPosition, Vector3 thrustVector)
    {
        if (!isFacingThrustVector)
        {
            //Do not accelerate
            

            return;
        }
        


        //Are we travelling away from the target?
        //OR, are we pointing away from our velocity
        if (Vector3.Dot(transform.forward, body.velocity) < 0)
        {
            ApplyAcceleration(1);
        }

        //float distantToThrustTarget = Vector3.Distance(thrustTargetPosition, transform.position);
        float distantToTarget = Vector3.Distance(target.position, transform.position);

        Vector3 vel = body.velocity;

        float timeToTarget = distantToTarget / vel.magnitude;

        float requiredAcceleration = vel.magnitude / timeToTarget;

        
        if(requiredAcceleration < maxAcceleration)
        {
            ApplyAcceleration(1);   //Accelerate
        }
        else
        {
            ApplyAcceleration(1); // Decelerate
        }
    }

    private void ApplyAcceleration(int direction)
    {
        emission = Mathf.Lerp(emission, 10, 0.02f);

        Vector3 acceleration = transform.forward * maxAcceleration * direction;
        acc = acceleration;
        body.AddForce(acceleration);
        
        
        if (body.velocity.magnitude > maxVelocity)
        {
            body.velocity = body.velocity.normalized * maxVelocity;

        }
    }


    public void SetTarget(Transform t)
    {
        target.position = t.position;
        //End up facing reverse!
        target.rotation = Quaternion.LookRotation( transform.position - target.position);
    }


    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, thrustVector, Color.green);
        Debug.DrawRay(transform.position, transform.forward * acc.magnitude, Color.blue);
    }

    void ApplyThrust(float f)
    {
        //Nothing
    }
}

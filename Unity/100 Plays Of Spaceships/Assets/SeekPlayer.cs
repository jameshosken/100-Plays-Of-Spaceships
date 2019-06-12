using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekPlayer : MonoBehaviour
{
    [SerializeField] float maxForce;
    [SerializeField] float maxVelocity;
    //[SerializeField] float turnRate;
    [SerializeField] Transform target;
    Rigidbody body;

    Vector3 turngoal;

    void Start()
    {
        body = GetComponent<Rigidbody>();

        target = GameObject.FindGameObjectWithTag("Player").transform;

    }


    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void FixedUpdate()
    {
        if (target == null)
        {
            return;
        }


        Rigidbody targetBody = target.GetComponent<Rigidbody>();

        Vector3 desiredVelocity = (target.position - transform.position);


        desiredVelocity = desiredVelocity.normalized;
        desiredVelocity *= maxVelocity;

        Vector3 steeringForce = desiredVelocity - body.velocity;


        Vector3 estimatedTarget = target.position + desiredVelocity;

        if (steeringForce.magnitude > maxForce)
        {
            steeringForce = steeringForce.normalized * maxForce;

        }

        body.AddForce(steeringForce);

        transform.forward = Vector3.Lerp(transform.forward, body.velocity, Time.deltaTime);
    }
        
}

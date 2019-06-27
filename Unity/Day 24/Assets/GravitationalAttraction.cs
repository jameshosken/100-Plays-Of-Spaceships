using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitationalAttraction : MonoBehaviour
{
    [SerializeField] GravitationalAttractor attractor;
    [SerializeField] Vector3 initial;
    [SerializeField] float gravitationalConstant = .1f;


    Rigidbody body;

    Vector3 forceVectorForLine = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        body.velocity = initial;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        HandleInput();

        Vector3 force = GetForceAtPosition(transform.position);
    
        body.AddForce(force);

        forceVectorForLine = force;
    }

    private void HandleInput()
    {
        float acc = Input.GetAxis("Vertical");

        Vector3 force = body.velocity.normalized * acc;

        body.AddForce(force);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + forceVectorForLine);
    }

    public Vector3 GetForceAtPosition(Vector3 position)
    {
        Vector3 forceVector = attractor.transform.position - position;

        forceVector = forceVector.normalized;

        float dist = Vector3.Distance(position, attractor.transform.position);

        float force = gravitationalConstant * (body.mass * attractor.GetMass() / (dist * dist));
        forceVector *= force;

        return forceVector;
    }

    

    public Vector3 GetAttractorPosition()
    {
        return attractor.transform.position;
    }
}

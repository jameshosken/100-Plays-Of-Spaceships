using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitationalAttraction : MonoBehaviour
{
    [SerializeField] GravitationalAttractor[] attractors;
    [SerializeField] Vector3 initial;
    [SerializeField] float gravitationalConstant = .1f;

    GravitationalAttractor currentAttractor = null;

    Rigidbody body;

    Vector3 forceVectorForLine = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        body.velocity = initial;
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

        float minDist = 10000;
        int index = -1;
        for (int i = 0; i < attractors.Length; i++)
        {
            float checkDist = Vector3.Distance(position, attractors[i].transform.position);
            float checkDistSOI = checkDist * attractors[i].GetSOI();

            if (checkDist < minDist)
            {
                minDist = checkDist;
                index = i;
            }
        }

        currentAttractor = attractors[index];

        Vector3 forceVector = currentAttractor.transform.position - position;

        forceVector = forceVector.normalized;

        float dist = minDist;

        float force = gravitationalConstant * (body.mass * currentAttractor.GetMass() / (dist * dist));
        forceVector *= force;

        return forceVector;
    }

    

    public Vector3 GetClosestAttractor(Vector3 position)
    {

        float minDist = 10000;
        int index = -1;
        for (int i = 0; i < attractors.Length; i++)
        {
            float checkDist = Vector3.Distance(position, attractors[i].transform.position);
            float checkDistSOI = checkDist * attractors[i].GetSOI();

            if (checkDistSOI < minDist)
            {
                minDist = checkDistSOI;
                index = i;
            }
        }

        return attractors[index].transform.position;
    }
}

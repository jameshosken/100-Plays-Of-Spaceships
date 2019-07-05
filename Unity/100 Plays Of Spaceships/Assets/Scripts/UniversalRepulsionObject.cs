using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalRepulsionObject : MonoBehaviour
{

    UniversalRepulsionForce repulsor;
    Rigidbody body;
    // Start is called before the first frame update
    void Start()
    {
        repulsor = FindObjectOfType<UniversalRepulsionForce>();
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 force = repulsor.GetRepulsionForce(transform.position);

        body.AddForce(force);
    }
}

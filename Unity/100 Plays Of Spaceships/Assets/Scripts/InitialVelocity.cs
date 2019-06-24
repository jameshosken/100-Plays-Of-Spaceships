using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class InitialVelocity : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float maxVel = 5;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(Random.Range(-maxVel, maxVel), Random.Range(-maxVel, maxVel), Random.Range(-maxVel, maxVel));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

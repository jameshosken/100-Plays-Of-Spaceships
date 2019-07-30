using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBoatController : MonoBehaviour
{
    Rigidbody body;

    [SerializeField] float thrust = 1f;
    [SerializeField] float agility = 1f;

    
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float fwd = Input.GetAxis("Vertical");
        float turn = Input.GetAxis("Horizontal");

        body.AddRelativeForce(Vector3.forward * fwd * thrust);
        body.AddRelativeTorque(Vector3.up * turn * agility);
    }
}

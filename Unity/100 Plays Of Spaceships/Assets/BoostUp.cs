using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostUp : MonoBehaviour
{

    [SerializeField] float force;
    Rigidbody body;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            body.AddForce(Vector3.up * force);
        }
    }
}

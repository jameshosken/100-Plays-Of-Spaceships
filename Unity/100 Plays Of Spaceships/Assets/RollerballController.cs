using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollerballController : MonoBehaviour
{

    [SerializeField] float acceleration;

    [SerializeField] Transform camRig;

    Rigidbody body;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Mathf.Clamp01(Input.GetAxis("Vertical"));

        Vector3 force = new Vector3(x, 0, z);

        force = camRig.rotation * force;

        body.AddForce( force * acceleration);


        //Braking
        if (Input.GetKey(KeyCode.S))
        {
            body.velocity *= 0.98f;
        }
    }
}

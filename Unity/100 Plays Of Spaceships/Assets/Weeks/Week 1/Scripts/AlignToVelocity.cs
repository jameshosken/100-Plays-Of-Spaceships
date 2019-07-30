using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignToVelocity : MonoBehaviour
{
    [SerializeField] Rigidbody target;
    [SerializeField] float sensitivity = 1f;

    //[SerializeField] float rotationSpeed = 0.1f;
    // Start is called before the first frame update
    void Start()
    { 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //transform.forward = Vector3.Lerp(transform.forward, target.velocity, rotationSpeed);

        target.AddTorque(Vector3.Cross(transform.forward, target.velocity) * sensitivity, ForceMode.Force);
    }
}

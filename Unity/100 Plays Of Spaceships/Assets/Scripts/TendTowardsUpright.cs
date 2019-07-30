using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TendTowardsUpright : MonoBehaviour
{
    [SerializeField] float sensitivity = .5f;
    Rigidbody body;
    private void Start()
    {
        body = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //Quaternion rotation = Quaternion.FromToRotation(transform.up, Vector3.up);
        //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, sensitivity);

        body.AddTorque(Vector3.Cross(transform.up, Vector3.up) * sensitivity, ForceMode.Force);
    }
}

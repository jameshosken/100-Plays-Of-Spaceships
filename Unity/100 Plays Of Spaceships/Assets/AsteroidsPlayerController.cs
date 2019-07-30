using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidsPlayerController : MonoBehaviour
{

    [SerializeField] float turnRate = 0.1f;
    [SerializeField] float thrustRate = 1f;

    Rigidbody body;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        float turn = Input.GetAxis("Horizontal") * turnRate * Time.deltaTime;

        float thrust = Input.GetAxis("Vertical") * thrustRate * Time.deltaTime;


        Vector3 rotationVec = transform.up * turn;

        Vector3 thrustVec = transform.forward * thrust;


        body.AddTorque(rotationVec);
        body.AddForce(thrustVec);

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{

    [SerializeField] Transform barrel;
    [SerializeField] Transform firePoint;

    [SerializeField] GameObject cannonball;

    [SerializeField] float rotationSpeed;

    [SerializeField] float max = 90f;
    [SerializeField] float min = -20f;

    [SerializeField] float force = 100f;

    Vector3 originalRot;

    // Start is called before the first frame update
    void Start()
    {
        originalRot = barrel.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        HandleControls();
    }

    private void HandleControls()
    {
        float rotation = Input.GetAxis("Vertical") * rotationSpeed ;

        if (barrel.localEulerAngles.x + rotation < originalRot.x + max && barrel.localEulerAngles.x + rotation > originalRot.x + min)
        {
            barrel.Rotate(new Vector3(rotation, 0, 0), Space.Self);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Fire();
        }

    }

    private void Fire()
    {
        GameObject ballObj =  Instantiate(cannonball, firePoint.position, Quaternion.identity);

        Rigidbody ball = ballObj.GetComponent<Rigidbody>();

        ball.AddForce(barrel.up * force);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoBotMovementControl : MonoBehaviour
{

    [SerializeField] float speed;
    [SerializeField] float turnSpeed;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        float forwardMotion = Input.GetAxis("Vertical");

        float turn = Input.GetAxis("Horizontal");

        transform.Rotate(Vector3.up, turn * turnSpeed);
        transform.Translate(Vector3.forward * forwardMotion * speed);
    }
}

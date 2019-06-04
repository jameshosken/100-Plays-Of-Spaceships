// Input Handler for Day 01 / WASDQE control

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleInput : MonoBehaviour
{
    SimpleMotionControl motionControl;
    ControlFeedbackDisplay controls;
    void Start()
    {
        motionControl = GetComponent<SimpleMotionControl>();
        controls = GetComponent<ControlFeedbackDisplay>();
    }

    void FixedUpdate()
    {
        controls.SetThrust(0);
        HandlePRY();
        HandleXYZ();
        HandleThrust();
        HandleDampeners();
        HandleCameraSwitcher();
    }

    private void HandleCameraSwitcher()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            GetComponent<SwitchCameras>().CycleCameras();
        }
    }

    private void HandleThrust()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            controls.SetThrust(-1);
            motionControl.HandleThrust();
        }
    }

    private void HandleDampeners()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            controls.SetThrust(1);
            motionControl.HandleDampeners();
        }
    }

    void HandlePRY()
    {
        float roll = Input.GetAxis("Horizontal") ;
        float pitch = Input.GetAxis("Vertical") ;
        float yaw = GetQEAxis() ;

        controls.SetPitch((int) Input.GetAxisRaw("Vertical"));
        controls.SetYaw((int)yaw);
        controls.SetRoll((int)Input.GetAxisRaw("Horizontal"));

        motionControl.HandlePRY(pitch, roll, yaw);

    }

    float GetQEAxis()
    {
        float output = 0;
        if (Input.GetKey(KeyCode.Q))
        {
            output -= 1;        //Minus to turn CCW
        }
        if (Input.GetKey(KeyCode.E))
        {
            output += 1;        //Plus to turn CW
        }

        return output;
    }

    private void HandleXYZ()
    {
        float x = GetJLAxis();
        float y = GetIKAxis();

        controls.SetX((int)x);
        controls.SetY((int)y);

        motionControl.HandleXYZ(x, y*-1, 0);
    }

    float GetIKAxis()
    {
        float output = 0;
        if (Input.GetKey(KeyCode.I))
        {
            output -= 1;        //Minus to turn CCW
        }
        if (Input.GetKey(KeyCode.K))
        {
            output += 1;        //Plus to turn CW
        }

        return output;
    }

    float GetJLAxis()
    {
        float output = 0;
        if (Input.GetKey(KeyCode.J))
        {
            output -= 1;        //Minus to turn CCW
        }
        if (Input.GetKey(KeyCode.L))
        {
            output += 1;        //Plus to turn CW
        }

        return output;
    }


}

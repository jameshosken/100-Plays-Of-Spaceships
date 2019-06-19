// Input Handler for Day 01 / WASDQE control

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleMouseInput : MonoBehaviour
{
    SimpleMouseMotionControl motionControl;
    //ControlFeedbackDisplay controls;

    [SerializeField] bool displayArrows = false;

    void Start()
    {
        motionControl = GetComponent<SimpleMouseMotionControl>();
        //controls = GetComponent<ControlFeedbackDisplay>();
    }


    void FixedUpdate()
    {
        HandleMouseLock();
        HandlePRY();
        HandleXYZ();
        HandleThrust();
        HandleDampeners();
        HandleCameraSwitcher();
    }

    private void HandleMouseLock()
    {

        if (Input.GetMouseButtonDown(0)) { 

            if (Cursor.lockState != CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
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
        float thrust = Input.GetAxis("Vertical");
        
        motionControl.HandleThrust(thrust);





    }

    private void HandleDampeners()
    {
        if (Input.GetKey(KeyCode.X))
        {
            motionControl.HandleDampeners();
        }
    }

    void HandlePRY()
    {
        float yaw = Input.GetAxis("Mouse X") ;
        float pitch = Input.GetAxis("Mouse Y") ;
        float roll = Input.GetAxis("Horizontal");
        motionControl.HandlePRY(pitch, roll, yaw);

    }

        private void HandleXYZ()
        {
            float x = GetQEAxis();
            //float y = GetIKAxis();

            motionControl.HandleXYZ(x, 0, 0);
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

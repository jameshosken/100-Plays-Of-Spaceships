using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDragRotate : MonoBehaviour
{

    [SerializeField] float damp = 0.1f;


    // Update is called once per frame
    void LateUpdate()
    {

        if (Input.GetMouseButton(1))
        {
            if(Cursor.lockState != CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            
            float x = Input.GetAxis("Mouse X");
            float y = Input.GetAxis("Mouse Y");


            transform.Rotate(new Vector3(0, x, 0));
            transform.Rotate(new Vector3 (-y, 0,0));
            
        }
        else
        {
            if (Cursor.lockState != CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, transform.parent.rotation, damp);
        }


    }   
}

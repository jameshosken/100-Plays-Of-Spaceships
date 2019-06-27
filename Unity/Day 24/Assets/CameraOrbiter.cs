using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbiter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {

            float x = Input.GetAxis("Mouse X");
            float y = Input.GetAxis("Mouse Y");

            transform.Rotate(Vector3.up, x);
            transform.Rotate(Vector3.left, y);
        }
    }
}

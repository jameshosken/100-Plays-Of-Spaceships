using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRTSCamera : MonoBehaviour
{
    [SerializeField] float multiplier = 0.5f;
    [SerializeField] float minY = -10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Mouse ScrollWheel")*-5;
        float z = Input.GetAxis("Vertical");

        if (transform.position.y < minY)
        {
            y = .1f;
        }

        transform.Translate(new Vector3(x, y, z), Space.Self);
        //transform.position += new Vector3(x, y, z);

        if (Input.GetKey(KeyCode.Q)){
            transform.Rotate(Vector3.up, 1f);
        }

        if (Input.GetKey(KeyCode.E)){
            transform.Rotate(Vector3.up, -1f);
        }

    }
}

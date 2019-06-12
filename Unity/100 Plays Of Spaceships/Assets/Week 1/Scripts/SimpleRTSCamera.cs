using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRTSCamera : MonoBehaviour
{
    [SerializeField] float multiplier = 0.5f;
    [SerializeField] float minY = -10;
    [SerializeField] float damp = 0.1f;

    Camera cam;

    Vector3 shift = Vector3.zero;
    float rShift = 0;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal") * multiplier;
        float y = Input.GetAxis("Mouse ScrollWheel")*-5;
        float z = Input.GetAxis("Vertical") * multiplier;

        if (transform.position.y < minY)
        {
            y = .1f;
        }
        shift = Vector3.Lerp(shift, new Vector3(x, y, z), damp);

        transform.Translate(new Vector3(shift.x, 0, shift.z), Space.Self);
        cam.transform.localPosition += new Vector3(0, 0, -shift.y);

        //transform.position += new Vector3(x, y, z);

        float rot = 0;
        if (Input.GetKey(KeyCode.Q)){
            rot = 10;
        }

        if (Input.GetKey(KeyCode.E)){
            rot = -10;
        }

        rShift = Mathf.Lerp(rShift, rot, damp);
        transform.Rotate(Vector3.up, rShift * multiplier);

    }
}

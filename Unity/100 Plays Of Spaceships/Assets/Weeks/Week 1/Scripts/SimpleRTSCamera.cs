using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRTSCamera : MonoBehaviour
{
    [SerializeField] float multiplier = 0.5f;
    [SerializeField] float minY = -10;
    [SerializeField] float damp = 0.1f;
    [SerializeField] float zoomMultiplier = 1f;

    [SerializeField] Transform camPivot;

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
        float y = Input.GetAxis("Mouse ScrollWheel")* -10 * zoomMultiplier;
        float z = Input.GetAxis("Vertical") * multiplier;

        if (transform.position.y < minY)
        {
            y = .1f;
        }
        shift = Vector3.Lerp(shift, new Vector3(x, y, z), damp * Time.deltaTime);

        transform.Translate(new Vector3(shift.x, 0, shift.z), Space.Self);
        cam.transform.localPosition += new Vector3(0, 0, -shift.y);

        //transform.position += new Vector3(x, y, z);

        float rot = 0;
        if (Input.GetKey(KeyCode.Q)){
            rot = 50;
        }

        if (Input.GetKey(KeyCode.E)){
            rot = -50;
        }

        rShift = Mathf.Lerp(rShift, rot, damp * Time.deltaTime);
        transform.Rotate(Vector3.up, rShift * multiplier * Time.deltaTime);

        if (Input.GetMouseButtonDown(2))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (Input.GetMouseButtonUp(2))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }


        if (Input.GetMouseButton(2))
        {
            Vector3 rotateX = new Vector3(Input.GetAxis("Mouse Y"), 0, 0);
            Vector3 rotateY = new Vector3(0, -Input.GetAxis("Mouse X"), 0);
            camPivot.Rotate(rotateX);
            transform.Rotate(rotateY);




        }
    }
}

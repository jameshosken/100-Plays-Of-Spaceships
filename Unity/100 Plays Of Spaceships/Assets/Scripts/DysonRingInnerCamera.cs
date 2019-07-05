using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DysonRingInnerCamera : MonoBehaviour
{
    [SerializeField] Transform pivot;

    [SerializeField] float heightMultiplier;
    [SerializeField] float rotationMultiplier;
    [SerializeField] float zoomMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float rotation = Input.GetAxis("Horizontal") * rotationMultiplier * Time.deltaTime;
        float height = Input.GetAxis("Vertical") * heightMultiplier * Time.deltaTime;

        float y = Input.GetAxis("Mouse ScrollWheel") * zoomMultiplier;


        transform.Rotate(0, rotation, 0);


        transform.Translate(0, height, 0);

        pivot.Translate(Vector3.forward * y, Space.Self);
    }
}

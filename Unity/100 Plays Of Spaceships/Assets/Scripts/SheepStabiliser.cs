using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepStabiliser : MonoBehaviour
{
    [SerializeField] float stabilityMultiplier = 1;
    Rigidbody body;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Ray ray = new Ray(transform.position + transform.up*.1f, transform.up * -1f);

        RaycastHit hit;

        Vector3[] hitInfo = new Vector3[2];
        if (Physics.Raycast(ray, out hit, 200f))
        {
            
            hitInfo[0] = hit.point;
            hitInfo[1] = hit.normal;

        }
        else
        {
            hitInfo[0] = transform.position;
            hitInfo[1] = transform.up;
        }


        Vector3 axis = Vector3.Cross(hitInfo[1], transform.up);

        float angle = Vector3.Angle(hitInfo[1], transform.up);

        transform.Rotate(axis, angle * 0.1f);

    }
}

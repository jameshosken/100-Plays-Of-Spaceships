using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreenCamera : MonoBehaviour
{
    [SerializeField] float translateScale;
    [SerializeField] float rotateScale;
    [SerializeField] float speed;

    [SerializeField] bool pos = true;
    [SerializeField] bool rot = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Mouse X");

        if (pos)
        {
            transform.Translate(new Vector3(x * translateScale, 0, 0), Space.World);

            transform.position = Vector3.Lerp(transform.position, Vector3.zero, speed);
        }
        if (rot)
        {
            transform.Rotate(new Vector3(0, 0, x * rotateScale), Space.Self);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, speed);
        }
    }
}

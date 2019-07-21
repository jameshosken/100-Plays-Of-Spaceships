using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float followSpeed = 0.8f;
    [SerializeField] float turnSpeed = 0.9f;
    // Start is called before the first frame update


    // Update is called once per frame
    void LateUpdate()
    {
        //transform.position = target.position;
        transform.position = Vector3.Lerp(transform.position, target.position, followSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, turnSpeed);
    }
}

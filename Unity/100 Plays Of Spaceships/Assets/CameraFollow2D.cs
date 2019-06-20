using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    //Place on camera rig parent. Start parent at location of target.

    [SerializeField] Transform target;
    [SerializeField] float followSpeed = .1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        transform.position = Vector3.Lerp(transform.position, target.position, followSpeed);
    }
}

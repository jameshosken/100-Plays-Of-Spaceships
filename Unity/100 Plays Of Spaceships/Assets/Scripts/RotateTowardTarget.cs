using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RotateTowardTarget : MonoBehaviour
{
    //Incomplete
    [SerializeField] GameObject target;

    void Update()
    {
        Vector3 targetPoint = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z) - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(-targetPoint, transform.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2.0f);

    }
}

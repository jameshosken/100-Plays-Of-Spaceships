using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RotateTowardTargetConstrained : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] bool rotateX;
    [SerializeField] bool rotateY;
    [SerializeField] bool rotateZ;

    float turnRate = 1;
    Transform parent;
    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent;

    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 worldRotationAxis = new Vector3(
        //    parent.forward.x * localRotationAxis.x,
        //    parent.forward.y * localRotationAxis.y,
        //    parent.forward.z * localRotationAxis.z);


        Vector3 worldRotationAxis = Vector3.zero;

        if (rotateX)
        {
            worldRotationAxis = parent.right;
        }
        else if (rotateY)
        {
            worldRotationAxis = parent.up;
        }
        else if (rotateZ)
        {
            worldRotationAxis = parent.forward;
        }
        else
        {
            return;
        }
        Vector3 rotationTarget = Vector3.ProjectOnPlane(target.position - transform.position, worldRotationAxis) + transform.position ;

        //transform.LookAt(rotationTarget, parent.up);

        Vector3 targetPoint = rotationTarget - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetPoint, transform.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnRate);

    }

    public void SetTurnRate(float r)
    {
        turnRate = r;
    }
}

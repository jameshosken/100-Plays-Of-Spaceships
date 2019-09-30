using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    enum UpdateCycle { Late, Fixed, Normal}
    [SerializeField] UpdateCycle updateCycle = UpdateCycle.Normal;
    [SerializeField] Transform target;
    [SerializeField] float followSpeed = 0.8f;
    [SerializeField] float turnSpeed = 0.9f;

    [SerializeField] Vector3 axisMultipliers;
    // Start is called before the first frame update

    

    // Update is called once per frame
    void LateUpdate()
    {
        if(updateCycle != UpdateCycle.Late)
        {
            return;
        }
        //transform.position = target.position;
        transform.position = Vector3.Lerp(transform.position, target.position, followSpeed);

        Vector3 targetRot = new Vector3(
            target.rotation.eulerAngles.x * axisMultipliers.x,
            target.rotation.eulerAngles.y * axisMultipliers.y,
            target.rotation.eulerAngles.z * axisMultipliers.z);



        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(targetRot), turnSpeed);
    }

    void Update()
    {
        if (updateCycle != UpdateCycle.Normal)
        {
            return;
        }
        //transform.position = target.position;
        transform.position = Vector3.Lerp(transform.position, target.position, followSpeed);

        Vector3 targetRot = new Vector3(
            target.rotation.eulerAngles.x * axisMultipliers.x,
            target.rotation.eulerAngles.y * axisMultipliers.y,
            target.rotation.eulerAngles.z * axisMultipliers.z);



        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(targetRot), turnSpeed);
    }

    void FixedUpdate()
    {
        if (updateCycle != UpdateCycle.Fixed)
        {
            return;
        }
        //transform.position = target.position;
        transform.position = Vector3.Lerp(transform.position, target.position, followSpeed);

        Vector3 targetRot = new Vector3(
            target.rotation.eulerAngles.x * axisMultipliers.x,
            target.rotation.eulerAngles.y * axisMultipliers.y,
            target.rotation.eulerAngles.z * axisMultipliers.z);



        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(targetRot), turnSpeed);
    }
}

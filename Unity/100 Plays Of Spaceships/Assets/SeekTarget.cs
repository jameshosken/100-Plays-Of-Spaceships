using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekTarget : MonoBehaviour
{


    [SerializeField] float maxForce;
    [SerializeField] float maxVelocity;
    //[SerializeField] float turnRate;
    [SerializeField] Transform target;
    Rigidbody body;

    Transform estimatedTarget;
    Transform estimatedTargetLocal;

    Vector3 turngoal;

    void Start()
    {
        body = GetComponent<Rigidbody>();
        GameObject estTarget = new GameObject();
        estTarget.name = "Missile Guidance";
        estimatedTarget = estTarget.transform;

        GameObject estTargetLocal = new GameObject();
        estTargetLocal.name = "Missile Guidance Local";
        estimatedTargetLocal = estTargetLocal.transform;
        estimatedTargetLocal.SetParent(estimatedTarget.transform);
        
    }


    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void FixedUpdate()
    {


        Rigidbody targetBody = target.GetComponent<Rigidbody>();
        ////Vector3 goal = (target.position - transform.position) + 
        ////    (targetBody.velocity - body.velocity);
        ////goal = goal.normalized;




        //Vector3 playerFromTarget = transform.position - target.position;
        //Vector3 estFromTarget = targetBody.velocity - body.velocity;

        ////Vector3 upFromTargetToPlayer = Vector3.Cross(playerFromTarget, estFromTarget);

        ////Vector3 perpFromTargetToPlayer = Vector3.Cross(upFromTargetToPlayer, playerFromTarget); // Vector along which to project line.


        //estimatedTarget.position = target.position;
        //estimatedTarget.LookAt(transform.position);

        //Vector3 perpFromTargetToPlayer = estimatedTarget.right;

        //float theta = Mathf.Deg2Rad * Vector3.Angle(perpFromTargetToPlayer, estFromTarget);
        //float lengthOfEst = estFromTarget.magnitude * Mathf.Cos(theta);

        //Vector3 perpOffset = Vector3.right * lengthOfEst;



        //estimatedTargetLocal.localPosition = Vector3.Lerp(estimatedTargetLocal.localPosition, perpOffset, turnRate);

        ////estimatedTarget.position = target.position;
        ////estimatedTarget.rotation = Quaternion.LookRotation(perpFromTargetToPlayer);

        ////WORKSish
        //Vector3 goal = estimatedTargetLocal.position;


        //transform.LookAt(goal);

        /////

        //Vector3 direction = goal - transform.position;
        //Quaternion toRotation = Quaternion.LookRotation(transform.forward, direction);
        //transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, turnRate * Time.time);

        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(goal), turnRate);
        //transform.rotation = Quaternion.LookRotation(goal, transform.up);


        //Quaternion goalRotation = Quaternion.LookRotation(goal * turnRate * Time.deltaTime);

        ////get the angle between transform.forward and target delta
        //float angleDiff = Vector3.Angle(transform.forward, goal);

        //// get its cross product, which is the axis of rotation to
        //// get from one vector to the other
        //Vector3 cross = Vector3.Cross(transform.forward, goal);

        //body.AddRelativeTorque(Vector3.forward * angleDiff, ForceMode.Force);

        Vector3 desiredVelocity = (target.position - transform.position) + (targetBody.velocity);

        
        desiredVelocity = desiredVelocity.normalized;
        desiredVelocity *= maxVelocity;

        Vector3 steeringForce = desiredVelocity - body.velocity;


        estimatedTarget.position = target.position +  desiredVelocity;

        if (steeringForce.magnitude > maxForce)
        {
            steeringForce = steeringForce.normalized * maxForce;

        }

        body.AddForce(steeringForce);

        transform.forward = Vector3.Lerp(transform.forward, body.velocity, Time.deltaTime);

        //if (body.velocity.magnitude > maxVelocity)
        //{
        //    body.velocity = body.velocity.normalized * maxVelocity;

        //}

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(estimatedTarget.position, 1);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(estimatedTargetLocal.position, 1);
    }

}

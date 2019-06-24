using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpulseDriveController : MonoBehaviour
{
    [SerializeField] Rigidbody controlBody;
    [SerializeField] float turnSpeed = 3;
    [SerializeField] float maxSpeed = 50;
    [SerializeField] float thrust = 1;

    [Tooltip("If ship has movable thrusters. If not, put model section here")]
    [SerializeField] Transform impulseDrive;

    bool isMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void BeginImpulseSequence(Vector3 target)
    {
        if (isMoving)
        {
            return;
        }
        StopAllCoroutines();
        StartCoroutine("BeginImpulseTurn", target);
    }

    IEnumerator BeginImpulseTurn(Vector3 target)
    {
        isMoving = true;
        Vector3 directionToTarget = (target - transform.position).normalized;
        float distanceToTarget = Vector3.Distance(transform.position, target);
        float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);
        float timeToTurn = turnSpeed * angleToTarget/180f;

        Quaternion startRotation = transform.rotation;
        int cycles = 100;
        for (int i = 0; i < cycles; i++)
        {
            float turnAmount = 1 / (float)cycles * (float)i;
            turnAmount = Mathf.SmoothStep(0, 1, turnAmount);        //Ease in/out the value to Slerp by

            Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(startRotation, lookRotation, turnAmount);

            yield return new WaitForSeconds( timeToTurn / (float)cycles);
        }
        StartCoroutine("BeginImpulseMove", target);

        yield return null;
    }


    IEnumerator BeginImpulseMove(Vector3 target)
    {

        float distance = Vector3.Distance(transform.position, target);
        float timeToTarget = distance / maxSpeed;

        float halfTurnTime = turnSpeed;

        Vector3 startPosition = transform.position;
        
        int cycles = 100;
        for (int i = 0; i < cycles; i++)
        {

            float moveAmount = 1 / (float)cycles * (float)i;
            moveAmount = Mathf.SmoothStep(0, 1, moveAmount);        //Ease in/out the value to Slerp by
            transform.position = Vector3.Lerp(startPosition, target, moveAmount);

            

            yield return new WaitForSeconds(timeToTarget / (float)cycles);
        }

        isMoving = false;
        yield return null;
    }


    // RigidBody Move

    //IEnumerator BeginImpulseMove(Vector3 target)
    //{
    //    //Accel to distance/2-(halfTurnTime/2).
    //    //Flip
    //    //Decel to target

    //    float halfTurnTime = turnSpeed; //( * 180/180)

    //    float distance = Vector3.Distance(transform.position, target);
    //    float startDistance = distance;
    //    float accelerationDistance = distance / 2f - halfTurnTime * controlBody.velocity.magnitude / 2f;

    //    float padding = 0f; //Range to stop decelerating

    //    float accelerationTime = 0;
    //    while(distance > startDistance - accelerationDistance)
    //    {
    //        controlBody.AddRelativeForce(Vector3.forward * thrust);

    //        //Calculate when to start turning
    //        distance = Vector3.Distance(transform.position, target);
    //        accelerationDistance = distance / 2f - halfTurnTime * controlBody.velocity.magnitude/2;

    //        accelerationTime += Time.deltaTime;
    //        yield return new WaitForSeconds(Time.deltaTime);
    //    }

    //    Quaternion startRotation = transform.rotation;
    //    Vector3 turnAxis = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
    //    Quaternion endRotation = Quaternion.LookRotation(transform.forward * -1f, turnAxis);
    //    int cycles = 90;
    //    for (int i = 0; i < cycles; i++)
    //    {
    //        float turnAmount = 1 / (float)cycles * (float)i;
    //        turnAmount = Mathf.SmoothStep(0, 1, turnAmount);        //Ease in/out the value to Slerp by


    //        impulseDrive.rotation = Quaternion.Slerp(startRotation, endRotation, turnAmount);

    //        yield return new WaitForSeconds(halfTurnTime / (float)cycles);
    //    }

    //    while (accelerationTime > padding)
    //    {

    //        controlBody.AddRelativeForce(Vector3.forward * thrust * -1f);
    //        distance = Vector3.Distance(transform.position, target);
    //        accelerationTime -= Time.deltaTime;

    //        yield return new WaitForSeconds(Time.deltaTime);
    //    }

    //    //Come to a rest and reset engine orientation

    //    startRotation = impulseDrive.rotation;
    //    endRotation = Quaternion.LookRotation(transform.forward, transform.up);
    //    cycles = 90;
    //    for (int i = 0; i < cycles; i++)
    //    {
    //        float turnAmount = 1 / (float)cycles * (float)i;
    //        turnAmount = Mathf.SmoothStep(0, 1, turnAmount);        //Ease in/out the value to Slerp by
    //        transform.position = Vector3.Lerp(transform.position, target, 0.01f);
    //        impulseDrive.rotation = Quaternion.Slerp(startRotation, endRotation, turnAmount);
    //        controlBody.velocity *= 0.9f;
    //        yield return new WaitForSeconds(halfTurnTime / (float)cycles);
    //    }

    //    isMoving = false;
    //    yield return null;
    //}
}

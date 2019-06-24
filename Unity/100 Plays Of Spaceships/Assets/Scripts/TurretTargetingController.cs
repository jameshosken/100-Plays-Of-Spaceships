using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretTargetingController : MonoBehaviour
{

    [SerializeField] Transform defaultTargetTransform;
    [SerializeField] Transform activeTargetTransform;
    [SerializeField] GameObject targetObject;
    [SerializeField] float turnRate = 1f;



    float laserSpeed = 1;


    public bool isTargeting()
    {
        if (targetObject != defaultTargetTransform.gameObject)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Start()
    {
        SetTurnRates();
        InvokeRepeating("SetNearestTarget", 1, 2);
    }

    // Update is called once per frame
    void Update()
    {
        SetTargetPosition();
    }

    public void SetTarget(GameObject t)
    {
        targetObject = t;
    }

    public void SetLaserSpeed(float s)
    {
        laserSpeed = s;
    }
    void SetNearestTarget()
    {
        targetObject = FindNearestTarget();
    }

    private void SetTargetPosition()
    {

        

        if (!targetObject || targetObject == defaultTargetTransform.gameObject)
        {
            SetNearestTarget();
        }

        if (Vector3.Dot(transform.up, (targetObject.transform.position - transform.position)) < 0)
        {
            SetNearestTarget();
        }


        Vector3 targetPosition = targetObject.transform.position;

        if (targetObject.GetComponent<Rigidbody>())
        {
            Rigidbody tBody = targetObject.GetComponent<Rigidbody>();
            targetPosition = targetObject.transform.position + tBody.velocity * GetFlightTime() + tBody.velocity * (.5f * Mathf.Sin(Time.time*3));
        }

        activeTargetTransform.position = targetPosition;
    }

    private GameObject FindNearestTarget()
    {
        GameObject[] shootables = GameObject.FindGameObjectsWithTag("Shootable");

        if(shootables.Length == 0)
        {
            return defaultTargetTransform.gameObject;
        }

        GameObject closestObj = null;

        float closest = 100000;
        for (int i = 0; i < shootables.Length; i++)
        {
            GameObject shootable = shootables[i];
            //If in sight
            if (Vector3.Dot(transform.up, (shootable.transform.position - transform.position) ) > 0)
            {
                //if nearest
                float dist = Vector3.Distance(shootable.transform.position, transform.position);
                if (dist < closest)
                {
                    closest = dist;
                    closestObj = shootable;
                }
            }
        }

        Debug.Log("Nearest Found: ");
        Debug.Log(closestObj);
        if (closestObj == null)
        {
            return defaultTargetTransform.gameObject;
        }
        return closestObj;

    }

    private float GetFlightTime()
    {
        float dist = Vector3.Distance(transform.position, targetObject.transform.position);

        float time = dist / laserSpeed;

        return time  ;
    }

    private void SetTurnRates()
    {
        RotateTowardTargetConstrained[] rotators = GetComponentsInChildren<RotateTowardTargetConstrained>();
        for (int i = 0; i < rotators.Length; i++)
        {
            RotateTowardTargetConstrained rotator = rotators[i];
            rotator.SetTurnRate(turnRate);
        }
    }
}

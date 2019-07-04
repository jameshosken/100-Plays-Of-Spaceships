using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargettingHandler : MonoBehaviour
{

    [SerializeField] float searchRadius = 100;
    [SerializeField] TargetHUDReference targetHUDRef;

    GameObject currentTarget;
    TargetVelocityIndicator targetVelocityIndicator;

    Camera cam;

    // Start is called before the first frame update
    void Start()
    {
       
        cam = Camera.main;
        targetVelocityIndicator = FindObjectOfType<TargetVelocityIndicator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            FindNewTarget();
        }    
    }

    private void FindNewTarget()
    {
        GameObject[] shootableObjects = GameObject.FindGameObjectsWithTag("Shootable");

        List<GameObject> targets = new List<GameObject>();

        for (int i = 0; i < shootableObjects.Length; i++)
        {
            GameObject obj = shootableObjects[i];

            //If dot product > 0, obj in front
            float dot = Vector3.Dot(transform.forward, obj.transform.position - transform.position);

            if  (dot > 0)
            {
                //On Screen
                Vector3 screenPos = cam.WorldToScreenPoint(obj.transform.position);
                Vector2 xy = new Vector2(screenPos.x, screenPos.y);

                float distFromCenter = Vector2.Distance(xy, new Vector2(Screen.width / 2, Screen.height / 2) );

                if (distFromCenter < searchRadius)
                {
                    targets.Add(obj);
                }
            }
        }

        float currentDist = 100000;
        
        
       
        for (int i = 0; i < targets.Count; i++)
        {
            GameObject obj = targets[i];
            float dist = Vector3.Distance(transform.position, obj.transform.position);
            if (dist < currentDist)
            {
                currentTarget = obj;
                currentDist = dist;
                HandleNewTarget(); ;
               
            }
            
        }

    }

    private void HandleNewTarget()
    {
        targetVelocityIndicator.SetTarget(currentTarget);
        targetHUDRef.SetTarget(currentTarget);
    }

    public Transform GetTarget()
    {
        if (currentTarget)
        {
            return currentTarget.transform;
        }
        else
        {
            return null;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineTrail : MonoBehaviour
{

    [SerializeField] int lineLength;
    [SerializeField] Transform engine;

    [SerializeField] float updateFidelity = 0.1f; // how much do you hvae to move to update?
    [ColorUsage(true, true)]
    [SerializeField] Color startColour;
    [ColorUsage(true, true)]
    [SerializeField] Color endColour;

    LineRenderer line;

    Vector3[] points;

    Vector3 pPosition;

    float pTime = 0;

    Rigidbody ship;

    // Start is called before the first frame update
    void Start()
    {
        ship = engine.GetComponentInParent<Rigidbody>();

        points = new Vector3[lineLength];
        for (int i = 0; i < lineLength; i++)
        {
            points[i] = Vector3.zero;
        }

        line = GetComponent<LineRenderer>();
        line.positionCount = lineLength;
        line.SetPositions(points);
        line.startColor = startColour;
        line.endColor = endColour;
    }

    // Update is called once per frame
    void Update()
    {
        //if(Vector3.Distance(engine.position, pPosition) > updateFidelity)
        //{
        //    UpdatePoints();
        //    pPosition = engine.position;
        //}

        
            UpdatePoints();
        
        


    }

    private void UpdatePoints()
    {
        for (int i = 0; i < lineLength-1; i++)
        {
            points[i] = points[i + 1];
        }
        points[lineLength - 1] = engine.position;
        line.SetPositions(points);


    }
}

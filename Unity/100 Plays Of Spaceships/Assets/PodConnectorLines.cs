using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodConnectorLines : MonoBehaviour
{
    [SerializeField] Transform leftEngine;
    [SerializeField] Transform rightEngine;

    [SerializeField] Transform leftHinge;
    [SerializeField] Transform rightHinge;

    LineRenderer leftLine;

    LineRenderer rightLine;

    LineRenderer lines;



    bool leftActive = true;
    bool rightActive = true;
    // Start is called before the first frame update
    void Start()
    {
        leftLine = leftHinge.GetComponent<LineRenderer>();
        rightLine = rightHinge.GetComponent<LineRenderer>();
        //lines = GetComponent<LineRenderer>();

        leftLine.positionCount = 2;
        rightLine.positionCount = 2;
        //lines.positionCount = 4;

    }

    // Update is called once per frame
    void Update()
    {
        CheckEnginesExist();
        Vector3[] positions = new Vector3[4];

        if (leftActive)
        {
            positions[0] = leftEngine.position;
            positions[1] = leftHinge.position;
        }
        else
        {
            positions[0] = leftHinge.position;
            positions[1] = leftHinge.position;
        }

        if (rightActive)
        {
            positions[2] = rightHinge.position;
            positions[3] = rightEngine.position;
        }
        else
        {
            positions[2] = rightHinge.position;
            positions[3] = rightHinge.position;
        }

        leftLine.SetPosition(0, positions[0]);
        leftLine.SetPosition(1, positions[1]);
        rightLine.SetPosition(0, positions[2]);
        rightLine.SetPosition(1, positions[3]);
        //lines.SetPositions(positions);
        

    }

    private void CheckEnginesExist()
    {
        if(rightEngine == null)
        {
            rightActive = false;
        }
        if (leftEngine == null)
        {
            leftActive = false;
        }
    }

    public void BreakLines(int i)
    {
        if(i == 1)
        {
            leftActive = false;
        }
        if (i == 0)
        {
            rightActive = false;
        }

    }
}

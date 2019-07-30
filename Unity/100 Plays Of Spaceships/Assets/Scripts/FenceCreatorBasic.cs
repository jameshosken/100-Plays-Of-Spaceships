using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FenceCreatorBasic : MonoBehaviour
{

    [SerializeField] GameObject fenceSegment;
    [SerializeField] int numberOfSegments = 10;
    [SerializeField] float segmentLength = 1f;

    List<GameObject> fence = new List<GameObject>();
    // Start is called before the first frame update

    Vector3 pRot = Vector3.zero;

    void Start()
    {
        CreateSegment(transform.position, transform.forward, numberOfSegments);

        pRot = transform.rotation.eulerAngles;

    }

    void Update()
    {

        if (Mathf.Abs((pRot - transform.rotation.eulerAngles).magnitude) > 1f)
        {
            Debug.Log("moved");
            RemoveFence();

            CreateSegment(transform.position, transform.forward, numberOfSegments);
            pRot = transform.rotation.eulerAngles;
        }
    }

    void RemoveFence()
    {
        for (int i = fence.Count-1; i >= 0; i--)
        {
            GameObject.Destroy(fence[i]);
        }

        fence.Clear();
    }

    private void CreateSegment(Vector3 position, Vector3 direction, int segments)
    {
        
        Vector3 startPos = position;


        Vector3[] hitInfo;
        Quaternion rotation;

        Vector3 nextDirection;

        Vector3 point1;
        Vector3 point2;
        Vector3 point3;


        for (int i = 0; i < segments; i++)
        {
            GameObject fencePost = Instantiate(fenceSegment) as GameObject;
            fencePost.transform.position = position;

            
            hitInfo = GetPositionFromRaycast(position, Vector3.down);      //Get hit point below (must rotate from direction to ground
            rotation = Quaternion.FromToRotation(Vector3.up, hitInfo[1]);            //Find offset rotation from current up to normal up
            nextDirection = rotation * direction;                                       //Rotate direction by offset

            point1 = hitInfo[0];
            point2 = fencePost.transform.position;
            point3 = fencePost.transform.position + nextDirection.normalized * segmentLength;

            LineRenderer line = fencePost.GetComponent<LineRenderer>();
            line.positionCount = 3;
            line.SetPositions( new Vector3[] {point1, point2, point3});

            position = point3;

            fence.Add(fencePost);
        }

        //End the post

        GameObject lastPost = Instantiate(fenceSegment) as GameObject;
        lastPost.transform.position = position;

        hitInfo = GetPositionFromRaycast(position, Vector3.down);      //Get hit point below (must rotate from direction to ground
        rotation = Quaternion.FromToRotation(hitInfo[1], Vector3.up);            //Find offset rotation from current up to normal up
        nextDirection = rotation * direction;                                       //Rotate direction by offset

        point1 = hitInfo[0];
        point2 = lastPost.transform.position;
        point3 = lastPost.transform.position + nextDirection.normalized * segmentLength;

        LineRenderer lastLine = lastPost.GetComponent<LineRenderer>();
        lastLine.positionCount = 2;
        lastLine.SetPositions(new Vector3[] { point1, point2 });
        fence.Add(lastPost);

    }

    private Vector3[] GetPositionFromRaycast(Vector3 raycastPosition, Vector3 direction)
    {
        Ray ray = new Ray(raycastPosition, direction);

        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, 200f))
        {
            Vector3[] hitInfo = new Vector3[2];
            hitInfo[0] = hit.point;
            hitInfo[1] = hit.normal;

            return hitInfo;
        }
        else return null;
    }

    

}

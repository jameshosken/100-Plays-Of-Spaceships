using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceCreator : MonoBehaviour
{
    [SerializeField] GameObject fenceSegment;
    [SerializeField] float segmentLength = 1f;
    [SerializeField] float fenceHeight;

    List<GameObject> fence = new List<GameObject>();
    // Start is called before the first frame update

    Camera camera;

    Vector3 mouseStartPosition;
    Vector3 mouseEndPosition;

    bool isPlacingFence = false;

    void Start()
    {
        camera = Camera.main;
    }

    void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            if (isPlacingFence == false)
            {
                isPlacingFence = true;
                mouseStartPosition = GetMouseClickPosition();
            }
            else
            {
                PlaceFence(fence);
                mouseStartPosition = mouseEndPosition;
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            RemoveFence();
            isPlacingFence = false;
        }

        if (isPlacingFence)
        {
            RemoveFence();
            mouseEndPosition = GetMouseClickPosition();
            CreateFenceBetweenTwoPoints(mouseStartPosition, mouseEndPosition, fenceHeight);
        }
       
    }

    
    Vector3 GetMouseClickPosition()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit)){
            return hit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }

    private void PlaceFence(List<GameObject> fence)
    {
        //Remove gameobjects from array, so that RemoveFence() function does not see them.
        fence.Clear();
    }


    void CreateFenceBetweenTwoPoints(Vector3 start, Vector3 end, float height)
    {
        //Createinitial conditions
        float distance = Vector3.Distance(start, end);
        Vector3 fencePostPosition = start + transform.up * height;
        Vector3 fencePostDirection = (end - start).normalized;      //Find direction between start & end
        fencePostDirection.y = 0;                                   //Direction only along xz plane (for straight fence)

        Vector3[] hitInfo;          //Store info about raycast hit
        Quaternion rotation;        //Offset from UP to raycast hit normal (to 'bend' fence around terrain)

        Vector3 nextDirection;      //Store info about direction of each fencepost

        Vector3 point1;
        Vector3 point2;
        Vector3 point3;

        while (distance > segmentLength*1.5){
            //Create segment facing towards end
            GameObject fencePost = Instantiate(fenceSegment) as GameObject;
            fencePost.transform.position = fencePostPosition;

            hitInfo = GetPositionFromRaycast(fencePostPosition, Vector3.down);      //Get hit point below (must rotate from direction to ground
            rotation = Quaternion.FromToRotation(Vector3.up, hitInfo[1]);            //Find offset rotation from current up to normal up
            nextDirection = rotation * fencePostDirection;                                       //Rotate direction by offset

            point1 = hitInfo[0];
            point2 = fencePost.transform.position;
            point3 = fencePost.transform.position + nextDirection.normalized * segmentLength;

            LineRenderer line = fencePost.GetComponent<LineRenderer>();
            line.positionCount = 3;
            line.SetPositions(new Vector3[] { point1, point2, point3 });

            fencePostPosition = point3;

            fence.Add(fencePost);

            distance = Vector3.Distance(fencePostPosition, end + Vector3.up*height);

        }


        //End while means that our next post is less than a segment away


        GameObject lastPost = Instantiate(fenceSegment) as GameObject;
        lastPost.transform.position = fencePostPosition;

        hitInfo = GetPositionFromRaycast(fencePostPosition, Vector3.down);      //Get hit point below (must rotate from direction to ground
        rotation = Quaternion.FromToRotation(hitInfo[1], Vector3.up);            //Find offset rotation from current up to normal up
        nextDirection = rotation * fencePostDirection;                                       //Rotate direction by offset

        point1 = hitInfo[0];
        point2 = lastPost.transform.position;
        point3 = end + (Vector3.up * height);
        Vector3 point4 = end;

        LineRenderer lastLine = lastPost.GetComponent<LineRenderer>();
        lastLine.positionCount = 4;
        lastLine.SetPositions(new Vector3[] { point1, point2, point3, point4  });
        fence.Add(lastPost);
    }
    

    //private void CreateSegment(Vector3 position, Vector3 direction, int segments)
    //{

    //    Vector3 startPos = position;

    //    Vector3[] hitInfo;
    //    Quaternion rotation;

    //    Vector3 nextDirection;

    //    Vector3 point1;
    //    Vector3 point2;
    //    Vector3 point3;


    //    for (int i = 0; i < segments; i++)
    //    {
    //        GameObject fencePost = Instantiate(fenceSegment) as GameObject;
    //        fencePost.transform.position = position;


    //        hitInfo = GetPositionFromRaycast(position, Vector3.down);      //Get hit point below (must rotate from direction to ground
    //        rotation = Quaternion.FromToRotation(Vector3.up, hitInfo[1]);            //Find offset rotation from current up to normal up
    //        nextDirection = rotation * direction;                                       //Rotate direction by offset

    //        point1 = hitInfo[0];
    //        point2 = fencePost.transform.position;
    //        point3 = fencePost.transform.position + nextDirection.normalized * segmentLength;

    //        LineRenderer line = fencePost.GetComponent<LineRenderer>();
    //        line.positionCount = 3;
    //        line.SetPositions(new Vector3[] { point1, point2, point3 });

    //        position = point3;

    //        fence.Add(fencePost);
    //    }

    //    //End the post
    //    GameObject lastPost = Instantiate(fenceSegment) as GameObject;
    //    lastPost.transform.position = position;

    //    hitInfo = GetPositionFromRaycast(position, Vector3.down);      //Get hit point below (must rotate from direction to ground
    //    rotation = Quaternion.FromToRotation(hitInfo[1], Vector3.up);            //Find offset rotation from current up to normal up
    //    nextDirection = rotation * direction;                                       //Rotate direction by offset

    //    point1 = hitInfo[0];
    //    point2 = lastPost.transform.position;
    //    point3 = lastPost.transform.position + nextDirection.normalized * segmentLength;

    //    LineRenderer lastLine = lastPost.GetComponent<LineRenderer>();
    //    lastLine.positionCount = 2;
    //    lastLine.SetPositions(new Vector3[] { point1, point2 });
    //    fence.Add(lastPost);

    //}

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


    void RemoveFence()
    {
        for (int i = fence.Count - 1; i >= 0; i--)
        {
            GameObject.Destroy(fence[i]);
        }

        fence.Clear();
    }


}

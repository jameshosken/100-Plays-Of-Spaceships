using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastFromCameraTargetController : MonoBehaviour
{

    Vector3 lastHitPoint = Vector3.zero;
    GameObject lastHitObject = null;

    Camera cam;
    
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }
       
    public bool IsRayCastHit()
    {
        int layerMask = 1 << 2; //ignoreraycast layer
        layerMask = ~layerMask;

        RaycastHit[] hits;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);


        hits = Physics.RaycastAll(ray, 200, layerMask);
        RaycastHit priorityHit = new RaycastHit();
        int currentLayer = 100;

        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            {
                if(hit.collider.gameObject.layer < currentLayer)
                {
                    priorityHit = hit;
                    currentLayer = priorityHit.collider.gameObject.layer;
                }
            }
        }

        if (currentLayer != 100)
        {
            lastHitObject = priorityHit.collider.gameObject;
            lastHitPoint = priorityHit.point;
            return true;
            // Do something with the object that was hit by the raycast.
        }
        else
        {
            return false;
        }
    }

    public Vector3 GetRaycastHitPoint()
    {
        return lastHitPoint;
    }

    public GameObject GetRaycastHitObject()
    {
        return lastHitObject;
    }

}

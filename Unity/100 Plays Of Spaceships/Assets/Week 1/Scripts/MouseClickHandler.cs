//https://docs.unity3d.com/ScriptReference/Physics.Raycast.html

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClickHandler : MonoBehaviour
{
    [SerializeField] GameObject target;

    GameObject selected;

    public Camera cam;

    private void Start()
    {
        selected = this.gameObject;
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            HandlePrimaryClick();
        }

        if (Input.GetMouseButtonDown(1))
        {
            HandleSecondaryClick();
        }

    }

    private void HandlePrimaryClick()
    {
        // Bit shift the index of the layer (2) to get a bit mask
        int layerMask = 1 << 2; //ignoreraycast layer

        // This would cast rays only against colliders in layer 2.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;


        RaycastHit[] hits;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        hits = Physics.RaycastAll(ray, 200, layerMask);

        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            {
                Transform objectHit = hit.transform;

                HandleSelectableClick(objectHit);

                // Do something with the object that was hit by the raycast.
            }
        }
    }




    private void HandleSecondaryClick()
    {
        // Bit shift the index of the layer (2) to get a bit mask
        int layerMask = 1 << 2; //ignoreraycast layer

        // This would cast rays only against colliders in layer 2.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;


        RaycastHit[] hits;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        hits = Physics.RaycastAll(ray, 200, layerMask);


        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            {
                Transform objectHit = hit.transform;

                if (objectHit.gameObject.tag == "Clickable")
                {
                    //SET TARGET
                    target.transform.position = hit.point;
                    
                    HandleClickableleHit();
                }
                else
                {
                    //Do Nothing?
                }

                // Do something with the object that was hit by the raycast.
            }
        }
    }

    private void HandleSelectableClick(Transform objectHit)
    {
        
        //Debug.Log("Click: " + objectHit.gameObject);

        if (objectHit.gameObject.tag == "Selectable")
        {
            selected = objectHit.gameObject;
            selected.GetComponent<SelectionHandler>().SetSelectionIndicator(true);
        }
        else
        {
            if (selected)
            {
                if (selected.GetComponent<SelectionHandler>())
                    selected.GetComponent<SelectionHandler>().SetSelectionIndicator(false);
                selected = this.gameObject;
            }
        }
    }


    private void HandleClickableleHit()
    {

        if (selected.Equals(this.gameObject))
        {
            return;
        }
        if (selected.GetComponent<MoveToTarget>().Equals(null))     //Use Equals here because the internet said to: https://forum.unity.com/threads/find-out-if-gameobject-has-a-compenent.38524/
        {
            return;
        }

        MoveToTarget mover = selected.GetComponent<MoveToTarget>();
        mover.SetTarget(target.transform);
    }
}

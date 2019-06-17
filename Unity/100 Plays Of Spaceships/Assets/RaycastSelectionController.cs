using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RayCastFromCameraTargetController))]
public class RaycastSelectionController : MonoBehaviour
{
    RayCastFromCameraTargetController caster;

    List<GameObject> selectedObjects = new List<GameObject>();

    [SerializeField] GameObject target;
    [SerializeField] float separationRadius = 10f;
    

    Vector3 targetPosition = Vector3.zero;
    void Start()
    {
        caster = GetComponent<RayCastFromCameraTargetController>();
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
                handleSelectionClick();   
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (caster.IsRayCastHit())
            {
                HandleSecondaryClick();
            }
        }
    }

    private void handleSelectionClick()
    {
        if (caster.IsRayCastHit())
        {
            GameObject selected = caster.GetRaycastHitObject();

            if (selected.tag == "Selectable")
            {

                if (selectedObjects.Contains(selected))
                {
                    SetRTSSelection(selected, false);
                    selectedObjects.Remove(selected);
                }
                else
                {
                    SetRTSSelection(selected, true);
                    selectedObjects.Add(selected);
                }

                
                return;
            }
            else
            {
                ClearSelection();
            }
            
        }
        else
        {
            ClearSelection();
        }
    }

    void ClearSelection()
    {
        foreach (GameObject sel in selectedObjects)
        {
            SetRTSSelection(sel, false);
        }
        selectedObjects.Clear();
    }

    private void HandleSecondaryClick()
    {

        

        targetPosition = caster.GetRaycastHitPoint();

        for (int i = 0; i < selectedObjects.Count; i++)
        {
            GameObject selected = selectedObjects[i];

            if (selected.GetComponentInParent<RTSUnitController>())
            {
                Vector3 offset =  ConstructNSidedPolygon(i);
                selected.GetComponentInParent<RTSUnitController>().SetMovementTarget(targetPosition+offset);
            }
        }
        ClearSelection();
    }

    private Vector3 ConstructNSidedPolygon(int n)
    {
        float r = separationRadius * selectedObjects.Count;
        float x = r * Mathf.Cos(2f * Mathf.PI * (float)n / selectedObjects.Count);
        float y = r * Mathf.Sin(2f * Mathf.PI * (float)n / selectedObjects.Count);


        return new Vector3(x, 0, y);
    }

    private void SetRTSSelection(GameObject selected, bool _b)
    {
        if (selected.GetComponent<RTSUnitController>())
        {
            selected.GetComponent<RTSUnitController>().SetSelection(_b);
        }
    }




}

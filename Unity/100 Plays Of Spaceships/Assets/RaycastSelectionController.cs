using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RayCastFromCameraTargetController))]
public class RaycastSelectionController : MonoBehaviour
{
    RayCastFromCameraTargetController caster;

    GameObject selected;
    [SerializeField] GameObject target;

    [SerializeField] GameObject selectionIcon;

    Vector3 targetPosition = Vector3.zero;
    void Start()
    {
        caster = GetComponent<RayCastFromCameraTargetController>();
        selectionIcon = Instantiate(selectionIcon);
        selectionIcon.SetActive(false);
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (caster.IsRayCastHit())
            {
                handleSelectionClick();
                
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (caster.IsRayCastHit())
            {
                HandleSecondaryClick();
            }
        }
    }

    private void HandleSecondaryClick()
    {
        targetPosition = caster.GetRaycastHitPoint();
        if (selected.GetComponentInParent<TeleportToTarget>())
        {
            selected.transform.LookAt(targetPosition, selected.transform.up);
            selected.GetComponentInParent<TeleportToTarget>().SetNewPosition(targetPosition);
            selected = null;
            selectionIcon.SetActive(false);
        }
    }

    private void handleSelectionClick()
    {

        selected = caster.GetRaycastHitObject();
        if (selected.tag != "Selectable")
        {
            selected = null;
            return;
        }
        selectionIcon.transform.parent = selected.transform;
        selectionIcon.transform.localPosition = Vector3.zero;
        selectionIcon.SetActive(true);
    }
}

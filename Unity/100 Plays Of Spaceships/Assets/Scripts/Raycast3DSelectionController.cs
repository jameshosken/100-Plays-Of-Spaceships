using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast3DSelectionController : MonoBehaviour
{
    RayCastFromCameraTargetController caster;

    List<GameObject> selectedObjects = new List<GameObject>();

    [SerializeField] GameObject target;
    [SerializeField] float separationRadius = 10f;

    [SerializeField] MovementCircleController movementCircle;

    enum MovementWidgetState{None, XY, Z};

    MovementWidgetState widget = MovementWidgetState.None;


    Vector3 targetPositionXZ = Vector3.zero;
    float targetPositionYOffset = 0;
    float yOffsetMultiplier = 5f;

    Camera cam;

    

    void Start()
    {
        movementCircle = Instantiate(movementCircle);
        movementCircle.gameObject.SetActive(false);
        caster = GetComponent<RayCastFromCameraTargetController>();
        cam = Camera.main;
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
        
        HandleSelectionRadius();
    }

    private void HandleSelectionRadius()
    {
        if (Input.GetMouseButton(2))
        {
            return;
        }

        if (movementCircle.gameObject.activeSelf == false)
        {
            return;
        }

        if(widget == MovementWidgetState.XY)
        {

            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                

                float distance = Vector3.Distance(movementCircle.gameObject.transform.position, hit.point);
                movementCircle.SetRadius(distance);

                targetPositionXZ = hit.point;
            }

            
        }else if(widget == MovementWidgetState.Z)
        {   

            targetPositionYOffset += Input.GetAxis("Mouse Y") * yOffsetMultiplier;

            Vector3 endPoint = new Vector3(targetPositionXZ.x, targetPositionXZ.y + targetPositionYOffset, targetPositionXZ.z);
            movementCircle.SetYLine(targetPositionXZ, endPoint);
            
        }

       
    }
        

    private void handleSelectionClick()
    {

        if(widget == MovementWidgetState.XY)
        {
            ClearSelection();
            return;
        }
       
        if (caster.IsRayCastHit())
        {

            GameObject selected = caster.GetRaycastHitObject();
            

            if (selected.tag == "Selectable")
            {
                

                if (selectedObjects.Contains(selected))
                {
                    //REMOVE SELECTION
                    SetRTSSelection(selected, false);
                    selectedObjects.Remove(selected);
                    //Find last selected object
                    if(selectedObjects.Count > 0)
                    {
                        Vector3 mouseCirclePos = new Vector3(
                           selectedObjects[selectedObjects.Count - 1].transform.position.x,
                           0,
                           selectedObjects[selectedObjects.Count - 1].transform.position.z);
                        movementCircle.gameObject.transform.position = mouseCirclePos;

                        movementCircle.SetCenterLine(mouseCirclePos, selectedObjects[selectedObjects.Count - 1].transform.position);
                    }
                    else
                    {
                        ClearSelection();
                    }
                    
                }
                else
                {
                    //NEW SELECTION
                    SetRTSSelection(selected, true);
                    selectedObjects.Add(selected);
                    Vector3 mouseCirclePos = new Vector3(
                        selected.transform.position.x,
                        0,
                        selected.transform.position.z);
                    movementCircle.gameObject.transform.position = mouseCirclePos;

                    movementCircle.gameObject.SetActive(true);
                    widget = MovementWidgetState.XY;

                    movementCircle.SetCenterLine(mouseCirclePos, selected.transform.position);
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
        movementCircle.SetYLine(Vector3.zero, Vector3.zero);
        movementCircle.gameObject.SetActive(false);
        widget = MovementWidgetState.None;
        LockCursor(false);

        foreach (GameObject sel in selectedObjects)
        {
            SetRTSSelection(sel, false);
        }
        selectedObjects.Clear();
    }

    private void HandleSecondaryClick()
    {
        //targetPosition = caster.GetRaycastHitPoint();

        

        for (int i = 0; i < selectedObjects.Count; i++)
        {
            GameObject selected = selectedObjects[i];

            if (selected.GetComponentInParent<RTSUnitController>())
            {
                if (widget == MovementWidgetState.XY)
                {
                    LockCursor(true);
                    widget = MovementWidgetState.Z;
                    return;
                }
                else if(widget == MovementWidgetState.Z)
                {
                    LockCursor(false);
                    Vector3 targetPosition = new Vector3(targetPositionXZ.x, targetPositionXZ.y + targetPositionYOffset, targetPositionXZ.z);
                    targetPositionXZ = Vector3.zero;
                    targetPositionYOffset = 0;

                    Vector3 offset = ConstructNSidedPolygon(i);
                    selected.GetComponentInParent<RTSUnitController>().SetMovementTarget(targetPosition + offset);
                }
                
            }
        }
        ClearSelection();
    }

    private Vector3 ConstructNSidedPolygon(int n)
    {
        if(n == 1)
        {
            return Vector3.zero;
        }
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

    void LockCursor(bool _s)
    {
        if (_s)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}

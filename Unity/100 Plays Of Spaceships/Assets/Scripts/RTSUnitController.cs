using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RTSMovementTypeHandler))]
public class RTSUnitController : MonoBehaviour
{

    SelectionIndicator selectionIndicator;
    RTSMovementTypeHandler mover;

    bool isSelected = false;

    // Start is called before the first frame update
    void Start()
    {
        mover = GetComponent<RTSMovementTypeHandler>();
        selectionIndicator = GetComponentInChildren<SelectionIndicator>();
        selectionIndicator.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMovementTarget(Vector3 target)
    {
        mover.SetMoveTarget(target);
    }

    public void SetSelection(bool _b)
    {

        isSelected = _b;
        selectionIndicator.gameObject.SetActive(isSelected);

    }
}

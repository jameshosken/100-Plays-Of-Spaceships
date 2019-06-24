using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class EditorSnap : MonoBehaviour
{

    [SerializeField] float snapIncrement = 10f;

    void Update()
    { 
        Vector3 snapPosition;

        snapPosition.x = Mathf.RoundToInt(transform.position.x / snapIncrement) * snapIncrement;
        snapPosition.y = Mathf.RoundToInt(transform.position.y / snapIncrement) * snapIncrement;
        snapPosition.z = Mathf.RoundToInt(transform.position.z / snapIncrement) * snapIncrement;

        transform.position = snapPosition;
    }
}

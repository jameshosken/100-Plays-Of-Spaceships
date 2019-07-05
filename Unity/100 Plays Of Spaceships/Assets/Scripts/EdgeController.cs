using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeController : MonoBehaviour
{
    private GameObject[] points = new GameObject[2];
    LineRenderer line;


    private void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    public void SetEdge(GameObject start, GameObject end)
    {
        points[0] = start;
        points[1] = end;

        BoxCollider collider = gameObject.AddComponent<BoxCollider>();
        Vector3 halfEdge = (start.transform.position + end.transform.position) / 2f;
        transform.position = halfEdge;
        collider.center = Vector3.zero;
        collider.size = new Vector3(.2f, .5f, Vector3.Distance(start.transform.position, end.transform.position));
        collider.transform.LookAt(start.transform.position);

    }

    public GameObject[] GetPoints()
    {
        return points;
    }

    
}

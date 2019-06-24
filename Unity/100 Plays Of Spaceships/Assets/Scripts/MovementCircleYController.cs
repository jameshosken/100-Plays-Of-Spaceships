using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementCircleYController : MonoBehaviour
{

    Vector3 lineStart = Vector3.zero;
    Vector3 lineEnd = Vector3.zero;

    LineRenderer line;
    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        line.SetPosition(0, lineStart);
        line.SetPosition(1, lineEnd);
    }

    public void SetLine(Vector3 start, Vector3 end)
    {
        lineStart = start;
        lineEnd = end;
    }
}

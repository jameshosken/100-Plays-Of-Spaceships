using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportLinesController : MonoBehaviour
{
    [SerializeField] float radius;
    [SerializeField] GameObject singleLine;

    static int numLines = 8;
    Vector3[] linePointsOffset = new Vector3[numLines];
    Vector3[] linePointsStart = new Vector3[numLines];
    Vector3[] linePointsEnd = new Vector3[numLines];

    GameObject[] lines = new GameObject[numLines];
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetLinePoints()
    {

        
        for (int i = 0; i < numLines; i += 1)
        {
            lines[i] = Instantiate(singleLine) as GameObject;
            LineRenderer line = lines[i].GetComponent<LineRenderer>();
            line.SetPosition(0, linePointsStart[i]);
            line.SetPosition(1, linePointsEnd[i]);
        }
    }

    public void SetLineStart(Vector3 pos)
    {
        float r = radius;
        for (int i = 0; i < numLines; i++)
        {
            linePointsOffset[i] = new Vector3(Random.Range(-r, r), Random.Range(-r, r), Random.Range(-r, r));

        }
        for (int i = 0; i < numLines; i++)
        {
            linePointsStart[i] = pos + linePointsOffset[i];
        }
    }

    public void SetLineEnd(Vector3 pos)
    {
        for (int i = 0; i < numLines; i++)
        {
            linePointsEnd[i] = pos + linePointsOffset[i];
        }

        SetLinePoints();
    }

    
}

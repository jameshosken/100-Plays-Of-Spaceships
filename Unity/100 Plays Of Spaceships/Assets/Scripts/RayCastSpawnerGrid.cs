using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastSpawnerGrid : MonoBehaviour
{

    [SerializeField] GameObject caster;

    [SerializeField] int numx;
    [SerializeField] int numy;
    [SerializeField] float separation;

    [SerializeField] Color colour1;
    [SerializeField] Color colour2;

    // Start is called before the first frame update
    void Start()
    {
        for (int x = -numx/2; x < numx/2; x++)
        {

            Color lineCol = Color.Lerp(colour1, colour2, (float)(x + numx/2) / (float)numx);
            for (int y = 0-numy/2; y <= numy/2; y++)
            {
                GameObject cln = Instantiate(caster) as GameObject;

                cln.transform.position = new Vector3(0, x* separation + transform.position.y, y*separation + transform.position.z);
                cln.transform.parent = transform;
                cln.GetComponent<LineRenderer>().startColor = lineCol;
                cln.GetComponent<LineRenderer>().endColor = lineCol;
            }
        }
    }


}

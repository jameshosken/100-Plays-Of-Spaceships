using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHUDReference : MonoBehaviour
{
    MeshFilter meshFilter;
    Transform target;

    // Start is called before the first frame update
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
       
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            transform.rotation = target.rotation;
        }
        else
        {
            if (meshFilter.mesh)
            {
                meshFilter.mesh = null;
            }
        }
    }

    public void SetTarget(GameObject obj)
    {
        Debug.Log(obj);
        target = obj.transform ;
        if (obj.GetComponent<MeshFilter>().mesh)
        {

            Debug.Log("change mesh");
            Debug.Log(obj.GetComponent<MeshFilter>());
            Debug.Log(obj.GetComponent<MeshFilter>().mesh);

            meshFilter.mesh = Instantiate(obj.GetComponent<MeshFilter>().mesh);

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysUp : MonoBehaviour
{

    enum UpdateMode { Normal, Fixed, Late}
    [SerializeField] UpdateMode updateMode;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(updateMode != UpdateMode.Normal)
        {
            transform.up = Vector3.up;
        }
    }

    void FixedUpdate()
    {
        if (updateMode != UpdateMode.Fixed)
        {
            transform.up = Vector3.up;
            transform.LookAt(transform.position + transform.parent.forward);
        }
    }

    void LateUpdate()
    {
        if (updateMode != UpdateMode.Late)
        {
            transform.up = Vector3.up;
        }
    }
}

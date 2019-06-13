using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSMovementTypeHandler : MonoBehaviour
{
    [SerializeField] float turnSpeed = 0.1f;
    [SerializeField] float jumpAccuracy = 0.95f;
    [SerializeField] GameObject teleportLines;

    Rigidbody rb = null;
    
   
    
    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<Rigidbody>())
        {
            rb = GetComponent<Rigidbody>();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMoveTarget(Vector3 _t)
    {
        StopCoroutine("TeleportTo");
        StartCoroutine("TeleportTo", _t);


    }

    IEnumerator TeleportTo(Vector3 _t)
    {
        Vector3 lookVector = _t - transform.position;
        Quaternion rot = Quaternion.LookRotation(lookVector, transform.up);
        //While transform not looking at target:
        while (Vector3.Dot( transform.forward, lookVector.normalized) < jumpAccuracy) {

            if (rb)
            {
                rb.velocity *= 0.95f;
                rb.angularVelocity *= 0.95f;
            }
            //Look towards target
            lookVector = _t - transform.position;
            rot = Quaternion.LookRotation(lookVector, transform.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, turnSpeed * Time.deltaTime) ;

            
            yield return null;
        }

        CreateLines(_t, transform.position);
        transform.position = _t;
        yield return null;
    }

    private void CreateLines(Vector3 _t, Vector3 pos)
    {
        GameObject lineObj = Instantiate(teleportLines) as GameObject;
        TeleportLinesController lineController = lineObj.GetComponent<TeleportLinesController>();
        lineController.SetLineStart(pos);
        lineController.SetLineEnd(_t);
    }
}

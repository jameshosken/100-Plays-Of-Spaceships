using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSMovementTypeHandler : MonoBehaviour
{

    enum EngineState { Off, Impulse, Warp };

    [SerializeField] float turnSpeed = 0.1f;
    [SerializeField] float jumpAccuracy = 0.95f;
    [SerializeField] EngineState engine = EngineState.Impulse;
    //TeleportToTarget teleporter;

    Rigidbody rb = null;

    
    
    // Start is called before the first frame update
    void Start()
    {
        //teleporter = GetComponent<TeleportToTarget>();

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
        if (engine == EngineState.Warp)
        {
            StopCoroutine("TurnAndJump");
            StartCoroutine("TurnAndJump", _t);
        }
        else if(engine == EngineState.Impulse)
        {
            BroadcastMessage("BeginImpulseSequence", _t);
        }

        
    }

    


    IEnumerator TurnAndJump(Vector3 _t)
    {

        BroadcastMessage("BeginTurnSequence", _t);
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
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, turnSpeed * Time.deltaTime) ;
            
            yield return null;
        }
        

        //Once lined up to target, begin jump sequence
        BroadcastMessage("BeginJumpSequence", _t);

        //Point to target padding
        for (int i = 0; i < 30; i++)
        {
            lookVector = _t - transform.position;
            rot = Quaternion.LookRotation(lookVector, transform.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, .1f);
            yield return new WaitForSeconds(0.01f);
        }
        yield return null;
    }

    
}

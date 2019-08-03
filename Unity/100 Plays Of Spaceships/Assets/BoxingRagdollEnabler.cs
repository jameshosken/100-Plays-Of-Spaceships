using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxingRagdollEnabler : MonoBehaviour
{
    //[SerializeField] Rigidbody body;
    Animator animator;

    Rigidbody[] ragdoll;

    Collider[] colliders;
    
    // Start is called before the first frame update
    void Start()
    {
        colliders = GetComponentsInChildren<Collider>();
        

        animator = GetComponent<Animator>();
        ragdoll = GetComponentsInChildren<Rigidbody>();

        for (int i = 0; i < ragdoll.Length; i++)
        {
            ragdoll[i].isKinematic = true; 
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //EnableRagdoll();
        }
    }


    void KillVel()
    {

        for (int i = 0; i < ragdoll.Length; i++)
        {
            ragdoll[i].velocity = Vector3.zero;
            ragdoll[i].angularVelocity = Vector3.zero;

        }
       
    }

    private void EnableRagdoll()
    {
        animator.enabled = false;
        for (int i = 0; i < ragdoll.Length; i++)
        {
            ragdoll[i].isKinematic = false;

        }
        Invoke("KillVel", 0.03f);
    }

    public void OnHit()
    {
        
        print("HIT");
        EnableRagdoll();
        
    }
}

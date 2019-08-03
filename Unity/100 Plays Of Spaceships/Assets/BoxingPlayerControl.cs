using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxingPlayerControl : MonoBehaviour
{

    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Jab");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        print("Punch");
        if (other.gameObject.GetComponentInParent<BoxingRagdollEnabler>())
        {
            other.gameObject.GetComponentInParent<BoxingRagdollEnabler>().OnHit();
        }
    }
    
}

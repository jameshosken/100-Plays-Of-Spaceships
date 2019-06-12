using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnorePhysics : MonoBehaviour
{

    Collider[] colliders;
    // Start is called before the first frame update
    void Start()
    {
        colliders = GetComponentsInChildren<Collider>();
        
        for (int i = 0; i < colliders.Length; i++)
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), colliders[i]);

        }
    }

}

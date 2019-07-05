using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalRepulsionForce : MonoBehaviour
{

    public float force = 10f;

    public bool lockY = true;

    public Vector3 GetRepulsionForce(Vector3 target)
    {

        if (lockY)
        {
            target.y = transform.position.y;
            
        }
        float dist = Vector3.Distance(target, transform.position);
        Vector3 lookDirection = target - transform.position;

        lookDirection.Normalize();

        //return lookDirection * force / Mathf.Pow(dist, 2);        //Sqared distance

        return lookDirection * force;
    }
}

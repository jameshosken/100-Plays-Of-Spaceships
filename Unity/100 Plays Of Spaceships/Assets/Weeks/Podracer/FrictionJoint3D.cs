// https://gist.github.com/ditzel/ae6ebc115d767da9a5a1e1f70dee27e5

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrictionJoint3D : MonoBehaviour
{

    [Range(0, 1)]
    public float Friction;

    protected Rigidbody Rigidbody;

    void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Rigidbody.velocity = Rigidbody.velocity * (1 - Friction);
        Rigidbody.angularVelocity = Rigidbody.angularVelocity * (1 - Friction);
    }



}
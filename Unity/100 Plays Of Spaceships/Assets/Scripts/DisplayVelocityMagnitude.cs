using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DisplayVelocityMagnitude : MonoBehaviour
{


    [SerializeField] Rigidbody body;

    // Update is called once per frame
    void Update()
    {
        GetComponent<TextMeshPro>().text = System.Math.Round(body.velocity.magnitude, 2).ToString() ;
    }
}

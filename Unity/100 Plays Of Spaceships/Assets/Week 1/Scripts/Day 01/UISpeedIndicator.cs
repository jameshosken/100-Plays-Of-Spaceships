using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISpeedIndicator : MonoBehaviour
{

    [SerializeField] Transform[] targets;
    [SerializeField] Rigidbody body;
    [SerializeField] float distanceMultiplier;

    void Update()
    {
        float spd = body.velocity.magnitude;

        targets[0].localPosition = new Vector3(0, 0, spd * distanceMultiplier);
        targets[2].localPosition = new Vector3(0, 0, -spd * distanceMultiplier);
    }
}

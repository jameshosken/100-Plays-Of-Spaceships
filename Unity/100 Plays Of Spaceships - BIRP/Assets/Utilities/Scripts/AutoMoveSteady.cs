using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMoveSteady : MonoBehaviour
{
    [SerializeField] private Vector3 axisAmount;
    [SerializeField] private Vector3 axisSpeeds;
    private Vector3 noiseOffsets = Vector3.zero;
    private Vector3 origin;

    // Start is called before the first frame update
    private void Start()
    {
        origin = transform.position;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Vector3 offset = new Vector3(
            Mathf.Sin(noiseOffsets.x) * axisAmount.x,
            Mathf.Sin(noiseOffsets.y) * axisAmount.y,
            Mathf.Sin(noiseOffsets.z) * axisAmount.z);

        transform.position = origin + offset;

        noiseOffsets += axisSpeeds;

    }
}

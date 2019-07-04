using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleThrustShake : MonoBehaviour
{
    [SerializeField] float increment = 0f;
    [SerializeField] float maxIncrement = .1f;
    [SerializeField] float multiplier = .01f;

    float offset;


    Vector3 originalPosition;
    // Start is called before the first frame update
    void Start()
    {
        offset = Random.Range(0, 100);
        originalPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
     

        ApplyShake();

        increment = Mathf.Lerp(increment, 0, 0.01f);
    }

    void ApplyShake()
    {

        offset += increment;
        float xOff = (Mathf.PerlinNoise(offset, 0) - .5f) * multiplier  ;
        float yOff = (Mathf.PerlinNoise(offset, 10) - .5f) * multiplier ;
        float zOff = (Mathf.PerlinNoise(offset, 20) - .5f) * multiplier ;

        transform.localPosition = new Vector3(
            originalPosition.x + xOff,
            originalPosition.y + yOff,
            originalPosition.z + zOff);
    }

    void ApplyThrust(float thrust) {

        increment = Mathf.Min(Mathf.Abs(thrust), maxIncrement);

        //if (thrust > 0.9) { 
        //    increment = Mathf.Lerp(increment, maxIncrement, 0.02f);
        //}
    }
}

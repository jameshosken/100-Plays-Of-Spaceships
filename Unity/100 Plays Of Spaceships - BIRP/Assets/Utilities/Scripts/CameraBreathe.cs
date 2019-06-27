using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBreathe : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float[] multipliers;

    //Vector3 originalAngles;
    // Start is called before the first frame update
    void Start()
    {
        //originalAngles = transform.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {


        float xOff = (Mathf.PerlinNoise(Time.time * speed, 0) - .5f) * multipliers[0];
        float yOff = (Mathf.PerlinNoise(Time.time * speed, 10) - .5f) * multipliers[1];
        float zOff = (Mathf.PerlinNoise(Time.time * speed, 20) - .5f) * multipliers[2];

        transform.localEulerAngles = new Vector3(xOff, yOff, zOff);


        //float xOff = (Mathf.PerlinNoise(Time.time * speed, 0) - .5f) * multipliers[0];
        //float yOff = (Mathf.PerlinNoise(Time.time * speed, 10) - .5f) * multipliers[1];
        //float zOff = (Mathf.PerlinNoise(Time.time * speed, 20) - .5f) * multipliers[2];

        //transform.localEulerAngles = new Vector3(
        //    originalAngles.x + xOff,
        //    originalAngles.y + yOff,
        //    originalAngles.z + zOff);

    }
}

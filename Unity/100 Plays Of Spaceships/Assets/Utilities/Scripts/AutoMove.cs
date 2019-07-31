using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMove : MonoBehaviour
{
    [SerializeField] Vector3 speed;
    [SerializeField] bool randomise;
    [SerializeField] float bounds;
    [SerializeField] Space space;
    // Start is called before the first frame update

    [SerializeField] float[] axesMultiplier;
    void Start()
    {
        if (randomise)
        {
            speed = new Vector3(
                UnityEngine.Random.Range(-bounds, bounds) * axesMultiplier[0],
                UnityEngine.Random.Range(-bounds, bounds) * axesMultiplier[1],
                UnityEngine.Random.Range(-bounds, bounds) * axesMultiplier[2]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Translate(speed * Time.deltaTime, space);
    }
}

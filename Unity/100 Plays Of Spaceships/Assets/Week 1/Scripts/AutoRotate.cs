using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    [SerializeField] Vector3 speed;
    [SerializeField] bool randomise;
    [SerializeField] float bounds;
    // Start is called before the first frame update
    void Start()
    {
        if (randomise)
        {
            speed = new Vector3(
                UnityEngine.Random.Range(-bounds, bounds),
                UnityEngine.Random.Range(-bounds, bounds),
                UnityEngine.Random.Range(-bounds, bounds));
        }
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(speed * Time.deltaTime, Space.Self);
    }
}

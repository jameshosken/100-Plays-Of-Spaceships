using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [SerializeField] int numberOfTargets = 10;
    [SerializeField] float waveRate = 30;
    [SerializeField] float bounds;
    [SerializeField] GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numberOfTargets; i++)
        {
            GameObject clone = Instantiate(target) as GameObject;

            float x = Random.Range(-bounds, bounds);
            float y = Random.Range(-bounds, bounds);
            float z = Random.Range(-bounds, bounds);
            clone.transform.position = new Vector3(x, y, z) + transform.position;

            clone.transform.rotation = Quaternion.Euler(x, y, z);

        }

        InvokeRepeating("Spawn", waveRate, waveRate);
    }

    void Spawn()
    {
        for (int i = 0; i < numberOfTargets; i++)
        {
            GameObject clone = Instantiate(target) as GameObject;

            float x = Random.Range(-bounds, bounds);
            float y = Random.Range(-bounds, bounds);
            float z = Random.Range(-bounds, bounds);
            clone.transform.position = new Vector3(x, y, z) + transform.position;

            clone.transform.rotation = Quaternion.Euler(x, y, z);


        }
    }
}

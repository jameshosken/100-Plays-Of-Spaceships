using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningSpawner : MonoBehaviour
{
    [SerializeField] GameObject light;
    [SerializeField] float timedelay = 2f;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("CreateLight", timedelay);
    }

    void CreateLight()
    {

        GameObject l = Instantiate(light) as GameObject;

        l.transform.parent = transform;

        float delay = Random.Range(timedelay / 2, timedelay * 2);
        Invoke("CreateLight", delay);
    }
}

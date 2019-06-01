using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoloCycler : MonoBehaviour
{

    [SerializeField] List<GameObject> hololist;

    GameObject currentHolo;

    int index = 0;

    double timer = 0;
    double interval = 5;     //min time (secs) between cycles

    // Start is called before the first frame update
    void Start()
    {
        AssignHolo();
        
    }

    private void AssignHolo()
    {
        currentHolo = Instantiate(hololist[index]) as GameObject;
        currentHolo.transform.position = this.transform.position;
        currentHolo.transform.SetParent(this.transform);
        transform.Rotate(new Vector3(0, UnityEngine.Random.Range(0, 360), 0));
    }

    // Update is called once per frame
    void Update()
    {   
        if(timer < interval)
        {
            timer += Time.deltaTime;
            return;
        }

        if (UnityEngine.Random.Range(0, 100) < 0.01)
        {
            timer = 0;
            CycleHolo();
        }
    }

    private void CycleHolo()
    {
        index = (index + 1) % hololist.Count;

        GameObject.Destroy(currentHolo);
        AssignHolo();
    }
}

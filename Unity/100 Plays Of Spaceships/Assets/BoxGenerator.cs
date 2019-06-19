using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxGenerator : MonoBehaviour
{
    [SerializeField] GameObject box;
    [SerializeField] int num = 200;
    [SerializeField] float maxSize = 5;
    [SerializeField] float buffer = 10;
    [SerializeField] Vector3 bounds;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < num; i++){

            Vector3 v = new Vector3(Random.Range(-bounds.x, bounds.x), Random.Range(-bounds.y, bounds.y), Random.Range(-bounds.z, bounds.z));
            while (Vector3.Distance(v, Vector3.zero) < buffer)
            {
                v = new Vector3(Random.Range(-bounds.x, bounds.x), Random.Range(-bounds.y, bounds.y), Random.Range(-bounds.z, bounds.z));
            }
            
            GameObject cln = Instantiate(box, transform);
            cln.transform.position = v;
            cln.transform.localScale = Vector3.one * Random.Range(0, maxSize);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDeactivator : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("EXITING");
        gameObject.SetActive(false);
    }
}

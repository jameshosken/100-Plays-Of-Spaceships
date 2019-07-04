using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootAtFixed : MonoBehaviour
{
    [SerializeField] Transform reticule;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.LookAt(reticule.position);
        
    }

    public void SetTargetDistance(float d)
    {
        reticule.position = Camera.main.transform.position;
        reticule.localPosition = Vector3.forward * d;
    }

    public void SetTarget(Transform t)
    {

        reticule.localPosition = Vector3.zero;
        reticule.position = t.position;
    }
}

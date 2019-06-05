using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootAtFixed : MonoBehaviour
{
    [SerializeField] Transform target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.LookAt(target.position);
    }
}

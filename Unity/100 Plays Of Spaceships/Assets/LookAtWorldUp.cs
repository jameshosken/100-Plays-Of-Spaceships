﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtWorldUp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position + Vector3.up, Vector3.up);
    }
}

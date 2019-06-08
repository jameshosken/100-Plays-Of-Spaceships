using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLauncher : MonoBehaviour
{

    [SerializeField] GameObject missile;
    [SerializeField] float maxRate;
    [SerializeField] float initialVelocity;

    [SerializeField] Transform missileLauncherLocation;

    TargettingHandler targetting;

    // Start is called before the first frame update
    void Start()
    {
        targetting = GetComponent<TargettingHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            FireMissile();
        }
    }

    private void FireMissile()
    {
        Transform target = targetting.GetTarget();

        GameObject clone = Instantiate(missile);
        clone.transform.position = missileLauncherLocation.position;
        clone.transform.rotation = missileLauncherLocation.rotation;
        clone.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;
        clone.GetComponent<Rigidbody>().AddForce(transform.forward * initialVelocity, ForceMode.Impulse);

        if (target)
        {
            clone.GetComponent<SeekTarget>().SetTarget(target);
        }
    }
}

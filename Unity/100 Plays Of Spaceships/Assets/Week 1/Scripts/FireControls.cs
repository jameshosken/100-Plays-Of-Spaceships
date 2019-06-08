using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireControls : MonoBehaviour
{
    [SerializeField] float firingRate;
    [SerializeField] float laserVelocity;
    [SerializeField] float targettingDistance = 100f;
    [SerializeField] HandleDualLaserCannon laser;

    [SerializeField] Transform inertia;
    [SerializeField] Transform reticule;

    [SerializeField] LootAtFixed[] cannonsLookAt;

    // Start is called before the first frame update
    void Start()
    {
        laser.SetFiringRate(firingRate);
        laser.SetLaserVelocity(laserVelocity);
        reticule.position = transform.position + Vector3.forward * -targettingDistance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            laser.SetFiring(true);
        }
        if (Input.GetButtonUp("Fire1"))
        {
            laser.SetFiring(false);
        }

        if (Input.GetKeyDown(KeyCode.T)){
            laser.ToggleTriFire();
        }

    }

   

    public void SetTarget(float d)
    {
        AimCannons(d);
        targettingDistance = d;
    }

    public void SetActualTarget(Transform t)
    {
    
        StickyAimCannons(t);
    }



    private void FixedUpdate()
    {
        //Work out travel time of particles T = D/V
        float travelTime = targettingDistance / laserVelocity;
        inertia.position = inertia.parent.position + GetComponent<Rigidbody>().velocity * travelTime;
    }

    public float GetLaserVelocity()
    {
        return laserVelocity;
    }

    void AimCannons(float d)
    {
        for (int i = 0; i < cannonsLookAt.Length; i++)
        {
            cannonsLookAt[i].SetTargetDistance(d);
        }

    }

    private void StickyAimCannons(Transform t)
    {

        for (int i = 0; i < cannonsLookAt.Length; i++)
        {

            cannonsLookAt[i].SetTarget(t);
        }
    }

}

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

    [SerializeField] LootAtFixed[] cannons;

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
        for (int i = 0; i < cannons.Length; i++)
        {
            cannons[i].SetTargetDistance(d);
        }

    }

}

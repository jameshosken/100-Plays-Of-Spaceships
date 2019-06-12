using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TurretTargetingController))]
public class TurretFiringController : MonoBehaviour
{

    [SerializeField] float laserSpeed = 50;
    [SerializeField] float firingRate = 4;

    TurretTargetingController targetingController;
    ParticleSystem lasers;
    ParticleSystem.MainModule main;
    ParticleSystem.EmissionModule emission;

    bool isFiring = false;
    bool isBurst = false;

    //min/max times
    [SerializeField] float[] burstOn = { 0.5f, 2f };
    [SerializeField] float[] burstOff = { 1f, 5f };
    

    // Start is called before the first frame update
    void Start()
    {
        lasers = GetComponentInChildren<ParticleSystem>();
        main = lasers.main;
        main.startSpeed = laserSpeed;
        emission = lasers.emission;
        emission.rateOverTime = 0;

        targetingController = GetComponent<TurretTargetingController>();
        targetingController.SetLaserSpeed(laserSpeed);

        Invoke("SetBurstOn", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(isFiring == false && targetingController.isTargeting())
        {
            SetFiring(true);
        }
        if(isFiring && targetingController.isTargeting() == false)
        {
            SetFiring(false);
        }
    }

    public void SetFiring(bool status)
    {
        isFiring = status;
        emission.rateOverTime = firingRate * (status?1:0);

        
        
    }

    void SetBurstOn()
    {
        emission.rateOverTime = firingRate * (isFiring?1:0) ;
        Invoke("SetBurstOff", Random.Range(burstOn[0], burstOn[1]));
    }

    void SetBurstOff()
    {
        emission.rateOverTime = 0;
        Invoke("SetBurstOn", Random.Range(burstOff[0], burstOff[1]));
    }
}

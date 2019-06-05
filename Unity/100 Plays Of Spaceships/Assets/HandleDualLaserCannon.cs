using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HandleDualLaserCannon : MonoBehaviour
{

    [SerializeField] Transform[] cannons;
    [SerializeField] ParticleSystem particles;
    [Tooltip("Particles per Second")]
    [SerializeField] float firingRate = 10; // Particles Per Second

    [SerializeField] GameObject HitFX;
    [SerializeField] LerpToClear reticule;

    int currentCannon = 0;

    bool isFiring = false;
    float firingDelay;
    float prevFireCheckpoint = 0;

    public List<ParticleCollisionEvent> collisionEvents;

    // Start is called before the first frame update
    void Start()
    {
        firingDelay = 1 / firingRate;
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    

    // Update is called once per frame
    void Update()
    {
        if (isFiring)
        {
            if(Time.time - firingDelay >= prevFireCheckpoint)
            {
                Fire();
                prevFireCheckpoint = Time.time;
            }
        }
    }

    private void Fire()
    {
        Transform cannon = cannons[currentCannon];
        particles.gameObject.transform.SetPositionAndRotation(cannon.position, cannon.rotation) ;

        particles.Emit(1);

        currentCannon = (currentCannon + 1) % cannons.Length;

    }

    public void SetFiringRate(float rate)
    {
        firingRate = rate;
        firingDelay = 1 / firingRate;
    }

    public void SetLaserVelocity(float vel)
    {
        ParticleSystem.MainModule main = particles.main;
        main.startSpeed = vel;
    }

    public void SetFiring(bool status)
    {
        isFiring = status;
    }

    private void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = particles.GetCollisionEvents(other, collisionEvents);

        Rigidbody rb = other.GetComponent<Rigidbody>();
        BoxTarget target = other.GetComponent<BoxTarget>(); 
        int i = 0;

        while (i < numCollisionEvents)
        {
            reticule.OnHit();
            Instantiate(HitFX, collisionEvents[i].intersection, Quaternion.identity);

            if (rb)
            {
                Vector3 pos = collisionEvents[i].intersection;
                Vector3 force = collisionEvents[i].velocity * 10;
                rb.AddForce(force);
            }
            if (target)
            {
                target.LoseHealth();
            }
            i++;
        }


    }
}

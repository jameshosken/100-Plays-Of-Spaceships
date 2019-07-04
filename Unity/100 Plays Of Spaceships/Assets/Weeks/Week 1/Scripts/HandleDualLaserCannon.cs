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
    [SerializeField] float damage = 50;

    [SerializeField] GameObject HitFX;
    HitIndicator hitIndicator;

    int currentCannon = 0;

    bool isFiring = false;
    float firingDelay;
    float prevFireCheckpoint = 0;

    public List<ParticleCollisionEvent> collisionEvents;

    bool triFire = false;

    // Start is called before the first frame update
    void Start()
    {
        hitIndicator = FindObjectOfType<HitIndicator>();
        firingDelay = 1 / firingRate;
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    public void ToggleTriFire()
    {
        triFire = !triFire;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFiring)
        {
            if (triFire) { 
                if (Time.time - firingDelay*3 >= prevFireCheckpoint)
                {
                    Fire();
                    prevFireCheckpoint = Time.time;
                }

            }
            else
            {
                if (Time.time - firingDelay >= prevFireCheckpoint)
                {
                    Fire();
                    prevFireCheckpoint = Time.time;
                }
            }
        }
    }

    private void Fire()
    {

        if (triFire)
        {
            for (int i = 0; i < cannons.Length; i++)
            {
                Transform cannon = cannons[i];
                particles.gameObject.transform.SetPositionAndRotation(cannon.position, cannon.rotation);

                particles.Emit(1);
            }
        }
        else
        {
            currentCannon = (currentCannon + 1) % cannons.Length;

            Transform cannon = cannons[currentCannon];
            particles.gameObject.transform.SetPositionAndRotation(cannon.position, cannon.rotation);

            particles.Emit(1);
        }

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
        Health target = other.GetComponent<Health>(); 
        int i = 0;

        while (i < numCollisionEvents)
        {
            hitIndicator.GetComponent<LerpToClear>().OnHit();
            Instantiate(HitFX, collisionEvents[i].intersection, Quaternion.identity);

            if (rb)
            {
                Vector3 pos = collisionEvents[i].intersection;
                Vector3 force = collisionEvents[i].velocity;
                rb.AddForce(force);
            }
            if (target)
            {
                target.LoseHealth(damage);
            }
            i++;
        }


    }
}

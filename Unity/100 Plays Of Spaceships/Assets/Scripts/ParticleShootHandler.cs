using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleShootHandler : MonoBehaviour
{

    [SerializeField] ParticleSystem particles;
    [SerializeField] float firingRate = 10; // Particles Per Second

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

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isFiring = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isFiring = false;
        }

        if (isFiring)
        {

            if (Time.time - firingDelay >= prevFireCheckpoint)
            {
                Fire();
                prevFireCheckpoint = Time.time;
            }
            
        }
    }

    private void Fire()
    {

        particles.Emit(1);
        

    }




    private void OnParticleCollision(GameObject other)
    {

        int numCollisionEvents = particles.GetCollisionEvents(other, collisionEvents);
;
        ShieldController shield = other.GetComponent<ShieldController>();
        int i = 0;

        while (i < numCollisionEvents)
        {
            if (shield)
            {
                Debug.Log("Particle Hit!");
                shield.AddHitPoint(collisionEvents[i].intersection, 10);
            }
            i++;
        }
    }
}

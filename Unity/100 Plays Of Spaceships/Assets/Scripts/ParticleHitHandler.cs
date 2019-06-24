using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHitHandler : MonoBehaviour
{

    [SerializeField] GameObject hitFX;
    [SerializeField] ParticleSystem particles;
    [SerializeField] float damage = 10;
    public List<ParticleCollisionEvent> collisionEvents;

    // Start is called before the first frame update
    void Start()
    {
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("Hit!");
        Debug.Log(other);
        int numCollisionEvents = particles.GetCollisionEvents(other, collisionEvents);


        Rigidbody rb = null;
        Health health = null;
        ShieldController shield = null;

        if (other.GetComponent<Rigidbody>())
        {
            rb = other.GetComponent<Rigidbody>();
        }

        if (other.GetComponent<Health>())
        {
            health = other.GetComponent<Health>();
        }

        if (other.GetComponentInChildren<ShieldController>())
        {
            shield = other.GetComponentInChildren<ShieldController>();
        }

        int i = 0;
        Debug.Log("Collision");

        while (i < numCollisionEvents)
        {

            Vector3 pos = collisionEvents[i].intersection;

            if (hitFX != null)
            {
                Instantiate(hitFX, pos, Quaternion.identity);
            }
            if (rb != null)
            {
                Vector3 force = collisionEvents[i].velocity;
                rb.AddForce(force);
                Debug.Log("force");
            }
            if (shield != null)
            {
                shield.AddHitPoint(pos, damage);

                Debug.Log("shield");
            }
            else if (health != null)
            {
                health.LoseHealth(damage);

                Debug.Log("health");
            }
            


            i++;
        }

    }

}




using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineImpactHandler : MonoBehaviour
{

    enum DeathBehaviour { Explode, Fly};

    [SerializeField] GameObject deathFX;
    [SerializeField] float explosionChance = 0f;
    [SerializeField] float maxCollisionVelocity = 10f;
    [SerializeField] float maxExplosionForce = 1000f;
    [SerializeField] float flyForce = 100f;

    EngineParticleHandler particles;
    DeathBehaviour behaviour;

    bool isExploded = false;
    bool isFlying = false;

    Rigidbody body;
    // Start is called before the first frame update
    void Start()
    {
        particles = FindObjectOfType<EngineParticleHandler>();
        behaviour = DeathBehaviour.Explode;
        if (UnityEngine.Random.Range(0f, 1f) > explosionChance)
        {
            behaviour = DeathBehaviour.Fly;
        }

        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isFlying)
        {
            if (particles)
            {
                particles.SetParticleEmission(gameObject, Random.Range(0, 200));
            }
            body.AddRelativeForce(Vector3.forward * flyForce);
        }
    }



    bool OnJointImpact()
    {
        if (GetComponent<Joint>())
        {
            print("REMOVING JOINT");
            Joint.Destroy(GetComponent<Joint>());
            return true;
        }
        return false;
    }

    void HandeExplode(Collision collision)
    {
        if (!isExploded)
        {
            if (particles)
            {
                particles.SetParticleEmission(gameObject, 0);
            }
            print("EXPLODING");
            body.AddExplosionForce(maxExplosionForce, collision.contacts[0].point, 10f);
            isExploded = true;

            Invoke("HandleDeath", UnityEngine.Random.Range(1, 5));
        }
    }

    void HandleFly()
    {
        if (!isFlying)
        {
            if (particles)
            {
                particles.SetParticleEmission(gameObject, 200);
            }
            print("FLYING");
            isFlying = true;
            Invoke("HandleDeath", UnityEngine.Random.Range(3, 5));
        }
    }

    void HandleDeath()
    {
        if (particles)
        {
            particles.SetParticleEmission(gameObject, 0);
        }
        GameObject cln = Instantiate(deathFX, transform.position, transform.rotation);
        GameObject.Destroy(gameObject);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.relativeVelocity.magnitude > maxCollisionVelocity)
        {

            switch (behaviour)
            {
                case DeathBehaviour.Explode:
                    HandeExplode(collision);
                    break;
                case DeathBehaviour.Fly:
                    HandleFly();
                    break;
                default:
                    break;
            }



            if (OnJointImpact())
            {
                FindObjectOfType<PodracerControl>().loseControl();
            }
        }
    }
}

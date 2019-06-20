using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LanderControl2D : MonoBehaviour
{
    public enum State { Alive, Dead }

    [SerializeField] private float thrust = 10f;
    [SerializeField] private float turnSpeed = 10f;
    [SerializeField] float deathExplosionForce = 100f;
    [SerializeField] GameObject deathFX;
    private State state;
    private Rigidbody body;

    ParticleSystem particles;
    ParticleSystem.EmissionModule emission;
    Light light;


    // Start is called before the first frame update
    private void Start()
    {
        light = GetComponentInChildren<Light>();
        particles = GetComponentInChildren<ParticleSystem>();
        emission = particles.emission;
        state = State.Alive;

        body = GetComponent<Rigidbody>();

        SetConstraints(true);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (state == State.Alive)
        {
            HandleInput();
        }
    }

    private void HandleInput()
    {
        float turn = Input.GetAxis("Horizontal") * -1f; // Invert controls
        Vector3 turnVector = new Vector3(0, 0, turn) * turnSpeed;

        body.AddRelativeTorque(turnVector);

        if (Input.GetKey(KeyCode.Space))
        {
            if(emission.enabled  != true)
            {
                light.enabled = true;
                emission.enabled = true;
            }
            body.AddRelativeForce(Vector3.up * thrust);
        }
        else
        {
            if (emission.enabled)
            {
                light.enabled = false;
                emission.enabled = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(state == State.Dead)
        {
            return;
        }


        Collider collider = collision.collider;

        if (collider.gameObject.layer == LayerMask.NameToLayer("Shootable"))
        {

            OnDeath(collision.contacts[0].point);
        }
    }

    private void OnDeath(Vector3 point)
    {

        light.enabled = false;
        emission.enabled = false;

        SetConstraints(false);
        body.AddExplosionForce(deathExplosionForce, point, 10);
        state = State.Dead;

        Instantiate(deathFX, point, Quaternion.identity);
        Invoke("ReloadLevel", 2f);
        
    }

    void ReloadLevel()
    {
        SceneManager.LoadSceneAsync(1);
    }

    private void SetConstraints(bool status)
    {
        if (status)
        {
            body.constraints = RigidbodyConstraints.FreezePositionZ;
            body.constraints = RigidbodyConstraints.FreezeRotationX;
            body.constraints = RigidbodyConstraints.FreezeRotationY;
        }
        else
        {
            body.constraints = RigidbodyConstraints.None;
        }

    }
}

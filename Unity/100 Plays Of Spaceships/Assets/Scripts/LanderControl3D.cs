using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LanderControl3D : MonoBehaviour
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
        float turnLR = Input.GetAxis("Horizontal") * -1f; // Invert controls
        float turnFB = Input.GetAxis("Vertical") * 1f; // Invert controls
        float roll = 0;

        if (Input.GetKey(KeyCode.Q))
        {
            roll = 1f;
        }
        if (Input.GetKey(KeyCode.E))
        {
            roll = -1f;
        }

        Vector3 turnLRVector = new Vector3(0, 0, turnLR) * turnSpeed;
        Vector3 turnFBVector = new Vector3(turnFB, 0, 0) * turnSpeed;
        Vector3 rollVector = new Vector3(0, roll, 0) * turnSpeed;

        body.AddRelativeTorque(turnLRVector);
        body.AddRelativeTorque(turnFBVector);
        body.AddRelativeTorque(rollVector);

        if (Input.GetKey(KeyCode.Space))
        {
            if (emission.enabled != true)
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
        if (state == State.Dead)
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
        SceneManager.LoadSceneAsync(0);
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

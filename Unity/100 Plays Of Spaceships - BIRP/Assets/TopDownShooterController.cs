using UnityEngine;

public class TopDownShooterController : MonoBehaviour
{


    [SerializeField] private ForceMode mode;
    [SerializeField] private float acceleration = 1f;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float rotation = 10f;

    [SerializeField] private float sizeX = 25f;

    [SerializeField] private float fireRate = 6f;

    [SerializeField] private Transform[] cannons;
    private Rigidbody body;
    private ParticleSystem lasers;
    private ParticleSystem.EmissionModule emission;
    AccuracyTracker accuracy;


    private float lastFire = 0;
    private int cannon = 0;


    long bulletsFired = 0;
    long targetsHit = 0;

    // Start is called before the first frame update
    private void Start()
    {
        body = GetComponent<Rigidbody>();
        lasers = GetComponentInChildren<ParticleSystem>();
        accuracy = FindObjectOfType<AccuracyTracker>();

        emission = lasers.emission;
        emission.rateOverTime = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        HandleMovement();
        LimitVelocity();
        LimitPosition();

        HandleLasers();
    }

    private void HandleLasers()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    //emission.rateOverTime = fireRate;
        //}

        if (Input.GetKeyUp(KeyCode.Space))
        {
            emission.rateOverTime = 0;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            Fire();
        }

    }

    private void Fire()
    {
        if (Time.time - lastFire > fireRate)
        {
            lastFire = Time.time;

            lasers.transform.position = cannons[cannon].position;
            lasers.Emit(1);

            accuracy.AddBulletsFired(1);

            cannon = (cannon + 1) % cannons.Length;
        }
    }

    private void LimitPosition()
    {
        if (Mathf.Abs(transform.position.x) > sizeX)
        {
            body.velocity *= 0.9f;


            Vector3 force = new Vector3(-transform.position.x, 0, 0).normalized * acceleration * Time.deltaTime * 1.1f;
            body.AddForce(force, mode);
        }
    }

    private void LimitVelocity()
    {
        if (body.velocity.magnitude > maxSpeed)
        {
            Vector3 vel = body.velocity.normalized * maxSpeed;
            body.velocity = vel;
        }
    }

    private void HandleMovement()
    {
        float xAxis = Input.GetAxis("Horizontal");

        float xMovement = xAxis * acceleration * Time.deltaTime;
        Vector3 force = new Vector3(xMovement, 0, 0);
        body.AddForce(force, mode);
        float zRotation = xAxis * rotation;

        Quaternion goalRotation = Quaternion.Euler(0, 0, zRotation);
        transform.rotation = Quaternion.Lerp(transform.rotation, goalRotation, 0.5f);
    }

    private void OnCollisionEnter(Collision collision)
    {

        FindObjectOfType<TopDownShooterGameEngine>().OnLose();
        gameObject.SetActive(false);
    }
}

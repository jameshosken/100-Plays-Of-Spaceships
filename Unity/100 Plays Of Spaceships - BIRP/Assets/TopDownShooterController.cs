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

    [SerializeField] int health = 100;

    [SerializeField] GameObject hitFX;
    [SerializeField] Transform raycaster;

    [SerializeField] GameObject dualWeapon;
    [SerializeField] GameObject singleWeapon;


    enum Weapon {Dual, Single}
    [SerializeField] Weapon weapon = Weapon.Single;

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

        FindObjectOfType<PlayerHealthText>().UpdateHealthText(health);

        LineRenderer line = raycaster.GetComponent<LineRenderer>();
        ParticleSystem particles = raycaster.gameObject.GetComponentInChildren<ParticleSystem>();
        ParticleSystem.EmissionModule em = particles.emission;

        line.enabled = false;
        em.enabled = false;
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

        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("FFF");
            
            if(weapon == Weapon.Dual)
            {
                weapon = Weapon.Single;
                singleWeapon.SetActive(true);
                dualWeapon.SetActive(false);
            }
            else if (weapon == Weapon.Single)
            {
                weapon = Weapon.Dual;
                singleWeapon.SetActive(false);
                dualWeapon.SetActive(true);
            }
            
        }
        

        if(weapon == Weapon.Dual)
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                emission.rateOverTime = 0;
            }

            if (Input.GetKey(KeyCode.Space))
            {
                Fire();
            }
        }else if(weapon == Weapon.Single)
        {
            HandleSingleLaser();
        }

        

    }

    void HandleSingleLaser()
    {
        LineRenderer line = raycaster.GetComponent<LineRenderer>();

        ParticleSystem particles = raycaster.gameObject.GetComponentInChildren<ParticleSystem>();
        ParticleSystem.EmissionModule em = particles.emission;


        if (Input.GetKeyUp(KeyCode.Space))
        {
            line.enabled = false;
            em.enabled = false;
            emission.rateOverTime = 0;
        }

        else if (Input.GetKeyDown(KeyCode.Space))
        {
            em.enabled = true;
            line.enabled = true;
        }


        else if (Input.GetKey(KeyCode.Space))
        {
            accuracy.AddBulletsFired(1);

            line.widthMultiplier = Random.Range(.5f, 1.2f);
            line.SetPosition(0, raycaster.position);

            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(raycaster.position, raycaster.forward, out hit, Mathf.Infinity))
            {
                line.SetPosition(1, hit.point);

                particles.gameObject.transform.position = hit.point;

                if (hit.collider.gameObject.GetComponent<Health>())
                {
                    hit.collider.gameObject.GetComponent<Health>().OnHit(5);
                    accuracy.AddTargetsHit();
                }
            }
            else
            {
                Vector3 offScreen = raycaster.position + Vector3.forward * 100;
                particles.gameObject.transform.position = offScreen;

                line.SetPosition(1, offScreen);
            }
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


        GameObject cln = Instantiate(hitFX) as GameObject;
        cln.transform.position = collision.gameObject.transform.position;

        GameObject.Destroy(collision.gameObject);
        
        health -= 34;
        FindObjectOfType<ScoreTracker>().HalveScore();
        FindObjectOfType<PlayerHealthText>().UpdateHealthText(health);


        if(health <= 0)
        {
            OnLose();
        }
        
    }

    void OnLose()
    {
        FindObjectOfType<TopDownShooterGameEngine>().OnLose();
        gameObject.SetActive(false);
    }
}

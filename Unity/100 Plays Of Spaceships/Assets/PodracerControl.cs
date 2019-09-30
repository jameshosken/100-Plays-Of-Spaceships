using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PodracerControl : MonoBehaviour
{


    [SerializeField] Transform engineControlBody;
    [SerializeField] float acceleration = 1;
    [SerializeField] float turnRate = 1;
    [SerializeField] float strafeMult = .3f;
    [SerializeField] float verticalMult = 0.5f;
    [SerializeField] float bankingMult = 50f;

    [SerializeField] float maxSpeed = 1f;

    [SerializeField] float friction = 0.98f;

    [Space]
    [Header("Falling Vars")]
    [SerializeField] float fallingFriction = 0.9f;
    [SerializeField] float gravity = 0.01f;
    [SerializeField] float maxFallingVel = 2f; // hacked to avoid breaking falls
    [SerializeField] float groundDistance = 2f;
    [SerializeField] float groundDistanceAdustSpeed = 0.1f;
    List<float> groundDistances = new List<float>();

    ChromaticAberration chroma = null;
    Bloom bloom = null;
    float bloomVal = 0;

    PostProcessVolume volume;

    EngineParticleHandler particles;
    PodracerAnimationHandler podAnimation;


    Vector3 velocity = Vector3.zero; // Track control velocity
    Vector3 trueVel = Vector3.zero;
    Vector3 pPosition; // Track true velocity
    float turn = 0;

    //To allow for turn banking
    GameObject heading;

    bool isInControl = true;
    bool isFalling = false;

    // Start is called before the first frame update
    void Start()
    {
        podAnimation = FindObjectOfType<PodracerAnimationHandler>();
        particles = FindObjectOfType<EngineParticleHandler>();
        heading = new GameObject();
        heading.transform.rotation = engineControlBody.transform.rotation;

        pPosition = engineControlBody.position;

        volume = Camera.main.GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out chroma);
        volume.profile.TryGetSettings(out bloom);
        bloomVal = bloom.intensity.value;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isInControl)
        {
            HandleControls();
        }
        else
        {
            if (podAnimation)
            {
                podAnimation.Stop();
            }
            chroma.intensity.value = 0;
            bloom.intensity.value = Mathf.Lerp(bloom.intensity.value, bloomVal, 0.1f);
        }

        print(velocity);
        engineControlBody.Translate(velocity, Space.World);

        HandleGroundDistance();

        heading.transform.Rotate(Vector3.up * turn);


        engineControlBody.rotation = heading.transform.rotation;
        engineControlBody.Rotate(Vector3.forward * turn * bankingMult);

        trueVel = Vector3.Lerp(trueVel, pPosition - engineControlBody.position, 0.01f);

        engineControlBody.Rotate(Vector3.left, trueVel.y * -90f); // Align to up/down velocity

        pPosition = engineControlBody.position;

    }

    private void HandleGroundDistance()
    {
        Ray ray = new Ray(engineControlBody.position, -engineControlBody.up);
        RaycastHit hit;

        Vector3 groundPoint = engineControlBody.position - (engineControlBody.up * (groundDistance));

        float vYSquared = (trueVel.y * trueVel.y);

        Debug.DrawRay(engineControlBody.position, Vector3.down * 10f);

        if (groundDistances.Count > 30)
        {
            groundDistances.Remove(0);
        }

        float avgDistance = 0;
        if (groundDistances.Count > 0)
        {
            for (int i = 0; i < groundDistances.Count; i++)
            {
                avgDistance += groundDistances[i];
            }
            avgDistance /= (float)groundDistances.Count;
        }



        if (Physics.Raycast(ray, out hit, groundDistance))
        {

            groundPoint = hit.point;

            //Gross gross gross I'm sorry:
            //float dist = Vector3.Distance(engineControlBody.position, groundPoint);

            //float upVel = 1 - (dist / groundDistance);
            //if(upVel > 0)
            //{
            //    velocity.y += upVel / groundDistanceAdustSpeed;
            //}

            isFalling = false;

            Vector3 desiredPosition = groundPoint + (engineControlBody.up * groundDistance);
            engineControlBody.position = Vector3.Lerp(engineControlBody.position, desiredPosition, groundDistanceAdustSpeed);
            //velocity.y += ((groundDistance - avgDistance) / groundDistance) / 100f;
            //Vector3 desiredPosition = groundPoint + (engineControlBody.up * groundDistance * (vYSquared + 1));


        }
        else
        {
            velocity += Vector3.down * gravity;
            isFalling = true;
        }

    }

    private void HandleControls()
    {

        float fwd = Input.GetAxis("Vertical");
        turn = Input.GetAxis("Horizontal");
        float strafe = 0;

        if (Input.GetKey(KeyCode.Q))
        {
            strafe -= acceleration;
        }

        if (Input.GetKey(KeyCode.E))
        {
            strafe += acceleration;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            velocity.y += acceleration * verticalMult;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            fwd *= 2f;
            chroma.intensity.value += 0.01f;
            chroma.intensity.value = Mathf.Clamp(chroma.intensity.value, 0, 1);
            bloom.intensity.value += 0.1f;
        }
        else
        {
            chroma.intensity.value -= 0.01f;
            chroma.intensity.value = Mathf.Clamp(chroma.intensity.value, 0, 1);
            bloom.intensity.value = Mathf.Lerp(bloom.intensity.value, bloomVal, 0.1f);
        }

        if (podAnimation != null)
        {
            if (fwd > 1)
            {
                podAnimation.Boost();
            }
            else if (fwd > 0)
            {
                podAnimation.Thrust();
            }
            else
            {
                podAnimation.Stop();
            }
        }

        if (particles != null)
        {
            particles.Boost(fwd);
        }
        fwd *= acceleration;

        fwd = Mathf.Clamp(fwd, 0, float.MaxValue);
        turn *= turnRate;

        velocity += engineControlBody.forward * fwd;
        velocity += engineControlBody.right * strafe * strafeMult;

        //velocity.z = Mathf.Clamp(velocity.z, 0, float.MaxValue);

        if (velocity.magnitude > maxSpeed)
        {
            velocity = velocity.normalized * maxSpeed;
        }


        //velocity *= friction;


        //If falling do not slow lateral movement
        //if (!isFalling)
        //{
            
        //}
        //else
        //{
        //    velocity *= 0.997f;
            
        //    if (velocity.y > maxFallingVel)
        //    {
        //        velocity.y = Mathf.Lerp(velocity.y, maxFallingVel, 0.1f);
        //    }
        //}
        velocity.x *= friction;
        velocity.z *= friction;
        velocity.y *= fallingFriction;
        strafe *= friction;


    }

    public void loseControl()
    {
        isInControl = false;
    }
}

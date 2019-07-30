using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day35 : MonoBehaviour
{

    [SerializeField] Transform rEngine;
    [SerializeField] Transform lEngine;

    [SerializeField] ParticleSystem rParticle;
    [SerializeField] ParticleSystem lParticle;

    [SerializeField] float force;

    ParticleSystem.EmissionModule rEm;

    ParticleSystem.EmissionModule lEm;

    Rigidbody body;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();

        rEm = rParticle.emission;

        lEm = lParticle.emission;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A))
        {
            body.AddForceAtPosition(transform.up * force * Time.deltaTime, lEngine.position);

        }

        if (Input.GetKey(KeyCode.D))
        {
            body.AddForceAtPosition(transform.up * force * Time.deltaTime, rEngine.position);
        }


        if (Input.GetKeyDown(KeyCode.A)){
            lEm.rateOverTime = 10;
        }

        if (Input.GetKeyDown(KeyCode.D)){
            rEm.rateOverTime = 10;
        }

        if (Input.GetKeyUp(KeyCode.A)){
            lEm.rateOverTime = 0;
        }

        if (Input.GetKeyUp(KeyCode.D)){
            rEm.rateOverTime = 0;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateParticlesOnKeypress : MonoBehaviour
{
    ParticleSystem particles;
    ParticleSystem.EmissionModule emission;

    [SerializeField] KeyCode key;
    ParticleSystem.MinMaxCurve rate;
    // Start is called before the first frame update
    void Start()
    {
        particles = GetComponent<ParticleSystem>();
        emission = particles.emission;

        rate = emission.rateOverTime;
        emission.rateOverTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            emission.rateOverTime = rate;
        }
        if (Input.GetKeyUp(key))
        {
            emission.rateOverTime = 0;
        }
    }
}

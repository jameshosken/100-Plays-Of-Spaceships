using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineParticleHandler : MonoBehaviour
{
    [SerializeField] ParticleSystem leftEngine;
    [SerializeField] ParticleSystem rightEngine;

    ParticleSystem.EmissionModule leftEmission;
    ParticleSystem.EmissionModule rightEmission;

    [SerializeField] float maxRate;
    // Start is called before the first frame update
    void Start()
    {
        leftEmission = leftEngine.emission;
        rightEmission = rightEngine.emission;

        leftEmission.rateOverTime = 0;
        rightEmission.rateOverTime = 0;
    }

    // Update is called once per frame
   public void Boost(float multiplier)
    {

        leftEmission.rateOverTime = maxRate * multiplier;
        rightEmission.rateOverTime = maxRate * multiplier;
    }

    public void FullStop()
    {
        leftEmission.rateOverTime = 0;
        rightEmission.rateOverTime = 0;
    }

    public void SetParticleEmission(GameObject engine, float rate)
    {
        if (leftEngine != null && engine != null)
        {
            if (engine == leftEngine || engine == leftEngine.transform.parent.gameObject)
            {
                leftEmission.rateOverTime = rate;
            }
        }
        if (rightEngine != null && engine != null)
        {
            if (engine == rightEngine || engine == rightEngine.transform.parent.gameObject)
            {
                rightEmission.rateOverTime = rate;
            }
        }
    }

    public void SetRLeftParticleEmission(float rate)
    {
        leftEmission.rateOverTime = rate;
    }
    public void SetRightParticleEmission(float rate)
    {
        rightEmission.rateOverTime = rate;
    }
}

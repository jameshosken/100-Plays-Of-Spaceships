using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpJumpParticleController : MonoBehaviour
{

    [SerializeField] ParticleSystem particles;
    [SerializeField] ParticleSystemForceField force;

    ParticleSystem.MainModule main;

    // Start is called before the first frame update
    void Start()
    {
        var wpc_emission = particles.emission;
        main = particles.main;
    }

    public void SetEmissionSphere(float f)
    {
        var shape = particles.shape;
        shape.radius = f;
    }

    public void Emit(int amount)
    {
        particles.Emit(amount);
    }

    public void SetEmission(float amount)
    {
        var wpc_emission = particles.emission;
        wpc_emission.rateOverTime = amount;
    }

    public void SetForceGravity(float f)
    {
        force.gravity = f;
    }

    public void SetForceDrag(float f)
    {
        force.drag = f;
    }

    public void SetForceRotation(float f, float i)
    {
        force.rotationSpeed = f;
        force.rotationAttraction = i;
    }


    // GET

    public float GetForceGravity()
    {
        return force.gravity.constant;
    }

    public float GetForceDrag()
    {
        return force.drag.constant;
    }

    public float GetForceRotation()
    {
        return force.rotationSpeed.constant;
    }
}

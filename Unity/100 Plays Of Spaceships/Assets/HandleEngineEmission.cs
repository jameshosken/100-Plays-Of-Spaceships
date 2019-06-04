using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleEngineEmission : MonoBehaviour
{
    [SerializeField] int materialIndex;
    [SerializeField] Color thrustColour;
    [SerializeField] float thrustColourMultiplier = 2f;

    Renderer renderer;
    Material mat;



    private void Start()
    {
        renderer = GetComponent<Renderer>();
        mat = renderer.materials[materialIndex];
        
    }


    void ApplyThrust(float thrust)
    {
        //Debug.Log("Thrusting: " + thrust);
        mat.SetColor("_EmissionColor", thrustColour * Mathf.Max(thrust,0) * thrustColourMultiplier);
        
    }
}

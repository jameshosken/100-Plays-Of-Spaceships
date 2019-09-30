using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GenerateGasGiant : MonoBehaviour
{

    [SerializeField] GameObject GasTemplate;
    [Tooltip("For LOD")]
    [SerializeField] GameObject SimpleSphere;

    [Header("World Params")]
    [SerializeField] Vector2 size;

    [Header("Gas Giant Day Parameter Ranges")]
    [Tooltip("Range: .5-2")]
    [SerializeField] Vector2 scale;
    [Tooltip("Range: 1f - 3f")]
    [SerializeField] Vector2 stormEvolution;
    [Tooltip("Range: 0f - 3f")]
    [SerializeField] Vector2 stormMultiplier;
    [Tooltip("Range: -1f - 1f")]
    [SerializeField] Vector2 midPoint;



    [Header("Gas Giant Colour Parameter Ranges")]
    [SerializeField] Gradient lowColour;
    [SerializeField] Gradient midColour;
    [SerializeField] Gradient highColour;


    GameObject planet;
    GameObject sphere;

    Material mat;

    // Start is called before the first frame update
    void Start()
    {
        //GeneratePlanet();
    }



    public void GeneratePlanet()
    {
        planet = Instantiate(GasTemplate, transform.position, transform.rotation);

        planet.transform.localScale = Vector3.one * UnityEngine.Random.Range(size.x, size.y);
        planet.transform.parent = transform;

        sphere = Instantiate(SimpleSphere, transform.position, transform.rotation);
        sphere.transform.localScale = planet.transform.localScale;
        sphere.transform.parent = transform;
        sphere.SetActive(false);

        mat = planet.GetComponent<Renderer>().material;

        SetShaderParams();
    }

    private void SetShaderParams()
    {
        mat.SetFloat("_gas_Scale", UnityEngine.Random.Range(scale.x, scale.y));

        mat.SetFloat("_gad_StormEvolution", UnityEngine.Random.Range(stormEvolution.x, stormEvolution.y));

        mat.SetFloat("_gad_Multiplier", UnityEngine.Random.Range(stormMultiplier.x, stormMultiplier.y));

        mat.SetFloat("_gas_Mid", UnityEngine.Random.Range(midPoint.x, midPoint.y));


        mat.SetColor("_gas_LowColour", lowColour.Evaluate(UnityEngine.Random.Range(0f, 1f)));

        mat.SetColor("_gas_MidColour", midColour.Evaluate(UnityEngine.Random.Range(0f, 1f)));

        mat.SetColor("_gas_HighColour", highColour.Evaluate(UnityEngine.Random.Range(0f, 1f)));



        
        sphere.GetComponent<Renderer>().materials[0].SetColor("_BaseColor", 
            Color.Lerp( 
                mat.GetColor("_gas_LowColour"), 
                mat.GetColor("_gas_HighColour"), 
                .3f
                ));


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject.Destroy(planet);
            GeneratePlanet();
        }
    }

    public void ActivateShader()
    {

            planet.SetActive(true);
            sphere.SetActive(false);
        
    }

    public void ActivateSphere()
    {

            planet.SetActive(false);
            sphere.SetActive(true);
        
    }
    public void DeactivateAll()
    {

            planet.SetActive(false);
            sphere.SetActive(false);
        
    }
}


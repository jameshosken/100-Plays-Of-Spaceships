using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GenerateEarthlikePlanet : MonoBehaviour
{

    [SerializeField] GameObject earthlikeTemplate;
    [Tooltip("For LOD")]
    [SerializeField] GameObject simpleSphere;
    
    [Header("World Params")]
    [SerializeField] Vector2 size;

    [Header("Earthlike Day Parameter Ranges")]
    [Tooltip("Range: 2 - 10")]
    [SerializeField] Vector2 octaves;
    [Tooltip("Range: 1f - 3f")]
    [SerializeField] Vector2 lacunarity;
    [Tooltip("Range: 0f - 1f")]
    [SerializeField] Vector2 persistence;
    [Tooltip("Range: 1f - 3f")]
    [SerializeField] Vector2 contrast;
    [Tooltip("Range: 1f - 5f")]
    [SerializeField] Vector2 scale;
    [Tooltip("Range: 0f - 1f")]
    [SerializeField] Vector2 seaLevel;

    [Header("Earthlike Colour Parameter Ranges")]
    [Tooltip("2 Colours per range: deepsea, shallowsea, land, mountain")]
    [SerializeField] Gradient deepSea;
    [SerializeField] Gradient shallowSea;
    [SerializeField] Gradient land;
    [SerializeField] Gradient mountain;


    [Header("Earthlike Night Parameter Ranges")]
    [Tooltip("Range: 150 - 200")]
    [SerializeField] Vector2 darksideScale;
    [Tooltip("Range: 10 - 15")]
    [SerializeField] Vector2 darksidePower;
    [Tooltip("2 Colours, min and max")]
    
    [GradientUsage(true)]
    [SerializeField] Gradient darksideColour;

    [Header("Earthlike Icecap Parameter Ranges")]
    [Tooltip("Range: 2 - 4 ")]
    [SerializeField] Vector2 icecapProgression;
    

    GameObject planet;
    GameObject sphere;
    public int maxOctaves;

    Material mat;

    // Start is called before the first frame update
    void Start()
    {
        //GeneratePlanet();
    }

    

    public void GeneratePlanet()
    {
        maxOctaves = (int)UnityEngine.Random.Range(octaves.x, octaves.y);
        planet = Instantiate(earthlikeTemplate, transform.position, transform.rotation);

        planet.transform.localScale = Vector3.one * UnityEngine.Random.Range(size.x, size.y);
        planet.transform.parent = transform;
        GameObject atmo = planet.GetComponentInChildren<Atmosphere>().gameObject;

        sphere = Instantiate(simpleSphere, transform.position, transform.rotation);
        sphere.transform.localScale = planet.transform.localScale;
        sphere.transform.parent = transform;
        sphere.SetActive(false);

        mat = planet.GetComponent<Renderer>().material;

        SetShaderParams();
    }

    private void SetShaderParams()
    {

        print("Generating Planet");
        mat = planet.GetComponent<Renderer>().material;

        mat.SetFloat("_planet_Scale", UnityEngine.Random.Range(scale.x, scale.y) );

        mat.SetFloat("_planet_Lacunarity", UnityEngine.Random.Range(lacunarity.x, lacunarity.y));

        mat.SetFloat("_planet_Persistence", UnityEngine.Random.Range(persistence.x, persistence.y));

        mat.SetFloat("_planet_Contrast", UnityEngine.Random.Range(contrast.x, contrast.y));

        mat.SetFloat("_planet_Octaves", (float)maxOctaves);

        mat.SetFloat("_planet_Persistence", UnityEngine.Random.Range(persistence.x, persistence.y));

        mat.SetFloat("_planet_SeaLevel", UnityEngine.Random.Range(seaLevel.x, seaLevel.y));

        mat.SetColor("_planet_SeaColour", deepSea.Evaluate(UnityEngine.Random.Range(0f, 1f) ) );

        mat.SetColor("_planet_ShallowColour", shallowSea.Evaluate(UnityEngine.Random.Range(0f, 1f)));

        mat.SetColor("_planet_LandColour", land.Evaluate(UnityEngine.Random.Range(0f, 1f)));

        mat.SetColor("_planet_MountainColour", mountain.Evaluate(UnityEngine.Random.Range(0f, 1f)));

        mat.SetFloat("_planet_DarksideScale", UnityEngine.Random.Range(darksideScale.x, darksideScale.y));

        mat.SetFloat("_planet_DarksidePower", UnityEngine.Random.Range(darksidePower.x, darksidePower.y));

        mat.SetColor("_planet_DarksideColour", darksideColour.Evaluate(UnityEngine.Random.Range(0f, 1f)));

        
        //Handle colour of distant sphere in LOD
        if (mat.GetFloat("_planet_SeaLevel") < 0.5f)
        {
            print("Land majprity");
            sphere.GetComponent<Renderer>().materials[0].SetColor("_BaseColor", mat.GetColor("_planet_LandColour"));
        }
        else
        {
            print("Sea majority");
            sphere.GetComponent<Renderer>().materials[0].SetColor("_BaseColor", mat.GetColor("_planet_ShallowColour"));
        }

        mat.SetFloat("_planet_IcecapMultiplier", UnityEngine.Random.Range(icecapProgression.x, icecapProgression.y));


        //mat.SetFloatArray("_planet_LightPosition", new float[] { lightpos.x, lightpos.y, lightpos.z });
    }

    public void SetLOD(int lod)
    {
        if(planet)
        {
            print("Setting Octaves for Shader: " + lod.ToString());
            mat.SetFloat("_planet_Octaves", (float)lod);
        }
        else
        {
            GeneratePlanet();
        }
        
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
        if (planet)
        {
            planet.SetActive(true);
            sphere.SetActive(false);
        }
    }

    public void ActivateSphere()
    {
        if (planet)
        {
            planet.SetActive(false);
            sphere.SetActive(true);
        }
    }
    public void DeactivateAll()
    {
        if (planet)
        {
            planet.SetActive(false);
            sphere.SetActive(false);
        }
    }
}


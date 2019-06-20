using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class WarpJumpControllerCockpit : MonoBehaviour
{

    [SerializeField] ParticleSystem warpParticles;
    [SerializeField] Renderer innerWarp;
    [SerializeField] Renderer outerWarp;

    [SerializeField] SkyboxHandler skyboxHandler;

    //For disabling controls
    HandleMouseInput inputScript;

    //Particle Info
    ParticleSystem.EmissionModule emission;

    


    // somewhere during initializing
    
    Bloom bloom = null;
    ChromaticAberration chroma = null;
    LensDistortion lens = null;

    int skyMax;
    int sky = 0;

    // Start is called before the first frame update
    void Start()
    {
        inputScript = FindObjectOfType<HandleMouseInput>();

        skyMax = skyboxHandler.GetNumberOfSkyboxes();

        emission = GetComponentInChildren<ParticleSystem>().emission;
        emission.enabled = false;


        SetChildrenActive(false);

        PostProcessVolume volume;
        volume = FindObjectOfType<PostProcessVolume>();
        volume.profile.TryGetSettings(out bloom);
        volume.profile.TryGetSettings(out chroma);
        volume.profile.TryGetSettings(out lens);

    }

    // Update is called once per frame
    void Update()
    {
            
    }

    void SetChildrenActive(bool s)
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(s);
        }
    }

    public void InitiateWarp()
    {
        
        StartCoroutine(WarpSequence());
    }

    IEnumerator WarpSequence()
    {
        inputScript.SetControlLock(true);

        Debug.Log("Charging");
        //Charge Up
        float timeToCharge = 2;
        int cycles = 60;

        for(int i = 0; i < cycles; i++)
        {
            float increment = 1f / cycles * (float)i;

            float intensity = Mathf.SmoothStep(1, 20, increment);
            skyboxHandler.SetSkyboxIntensity(intensity);

            inputScript.GetComponent<Rigidbody>().angularVelocity *= .9f;
            inputScript.GetComponent<Rigidbody>().velocity *= .8f;

            float fov = Mathf.SmoothStep(60, 55, increment);
            Camera.main.fieldOfView = fov;

            float lensIntensity = Mathf.SmoothStep(0, 20, increment);
            lens.intensity.value = lensIntensity;

            float bloomIntensity = Mathf.SmoothStep(3, 5, increment);
            bloom.intensity.value = bloomIntensity;

            yield return new WaitForSeconds(timeToCharge / (float)cycles);


        }

        Debug.Log("Starting Warp");

        //Warp
        transform.position = inputScript.transform.position;
        transform.rotation = inputScript.transform.rotation;

        SetChildrenActive(true);
        

        emission.enabled = true;

        timeToCharge = 1;
        cycles = 50;
        for (int i = 0; i < cycles; i++)
        {
            float increment = 1f / cycles * (float)i;

            float innerIntensity = Mathf.SmoothStep(0, -2, increment);
            float outerIntensity = Mathf.SmoothStep(0, 1, increment);

            innerWarp.material.SetFloat("_Lift", innerIntensity);
            outerWarp.material.SetFloat("_Lift", outerIntensity);

            float fov = Mathf.SmoothStep(55, 70, increment);
            Camera.main.fieldOfView = fov;

            float chromaIntensity = Mathf.SmoothStep(.2f, 1, increment);
            chroma.intensity.value = chromaIntensity;

            float lensIntensity = Mathf.SmoothStep(20, -30, increment);
            lens.intensity.value = lensIntensity;

            yield return new WaitForSeconds(timeToCharge / (float)cycles);
        }

        

        timeToCharge = 2f;
        cycles = 60;
        for (int i = 0; i < cycles; i++)
        {
            float increment = 1f / cycles * (float)i;

            float intensity = Mathf.SmoothStep(20, 0, increment);
            skyboxHandler.SetSkyboxIntensity(intensity);

            float innerIntensity = Mathf.SmoothStep(-2, .6f, increment);
            float outerIntensity = Mathf.SmoothStep(1, -1f, increment);

            innerWarp.material.SetFloat("_Lift", innerIntensity);
            outerWarp.material.SetFloat("_Lift", outerIntensity);



            yield return new WaitForSeconds(timeToCharge / (float)cycles);
        }

        skyboxHandler.SetNoSkybox();

        Debug.Log("Warping");
        yield return new WaitForSeconds(4);

        Debug.Log("Changing BG");

        //Change BG
        sky = (sky + 1) % skyMax;
        skyboxHandler.SetSkyboxByIndex(sky);
        skyboxHandler.SetSkyboxIntensity(0);

        emission.enabled = false;

        timeToCharge = 1f;
        cycles = 30;
        
        for (int i = 0; i < cycles; i++)
        {
            float increment = 1f / cycles * (float)i;

            float intensity = Mathf.SmoothStep(0, 1, increment);
            skyboxHandler.SetSkyboxIntensity(intensity);

            float innerIntensity = Mathf.SmoothStep(.6f, 1, increment);     //1 is off
            float outerIntensity = Mathf.SmoothStep(-1f, 1, increment);     //1 is off

            innerWarp.material.SetFloat("_Lift", innerIntensity);
            outerWarp.material.SetFloat("_Lift", outerIntensity);

            float fov = Mathf.SmoothStep(70, 60, increment);
            Camera.main.fieldOfView = fov;

            float chromaIntensity = Mathf.SmoothStep(1, .2f, increment);
            chroma.intensity.value = chromaIntensity;

            float lensIntensity = Mathf.SmoothStep(-30, 0, increment);
            lens.intensity.value = lensIntensity;

            float bloomIntensity = Mathf.SmoothStep(5, 3, increment);
            bloom.intensity.value = bloomIntensity;

            yield return new WaitForSeconds(timeToCharge / (float)cycles);
        }
        Debug.Log("Cool Down");

        //Cool Down
        

        yield return new WaitForSeconds(1);

        inputScript.SetControlLock(false);
        SetChildrenActive(false);

        yield return null;
    }
}

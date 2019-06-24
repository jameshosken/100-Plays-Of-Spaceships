using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxHandler : MonoBehaviour
{

    [SerializeField] Material[] materials;

    int c = 0;
    // Start is called before the first frame update
    void Start()
    {
        RenderSettings.skybox = materials[0];
        SetSkyboxIntensity(1);
    }

    private void Update()
    {

    }

    public int GetNumberOfSkyboxes()
    {
        return materials.Length;
    }

    public void SetSkyboxByIndex(int i)
    {
        if (i >= 0 && i < materials.Length)
        {
            RenderSettings.skybox = materials[i];
        }
    }

    public void SetSkyboxIntensity(float i)
    {
        RenderSettings.skybox.SetFloat("_Exposure", i);

    }

    public void SetNoSkybox()
    {

        RenderSettings.skybox = null;

    }
}

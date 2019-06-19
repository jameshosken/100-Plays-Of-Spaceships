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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetSkyboxByIndex(c);
            c = (c + 1) % materials.Length;
        }
    }

    void SetSkyboxByIndex(int i)
    {
        if (i >= 0 && i < materials.Length)
        {
            RenderSettings.skybox = materials[i];
        }
    }
}

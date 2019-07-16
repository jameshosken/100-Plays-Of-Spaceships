using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntermittentGlow : MonoBehaviour
{
    
    [SerializeField] float maxGlowTime = 2f;
    [SerializeField] float glowChance = .5f;

    Renderer renderer;
    Material mat;
    
    Color maxColor;

    bool locker = false;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponentInChildren<Renderer>();

        mat = renderer.materials[0];

        maxColor = mat.GetColor("_EmissionColor");
        mat.SetColor("_EmissionColor", Color.black);
    }

    // Update is called once per frame
    void Update()
    {
        if (Random.Range(0, 100) < glowChance && !locker)
        {
            StopAllCoroutines();
            StartCoroutine(Glow());
        }
    }

    IEnumerator Glow()
    {
        locker = true;
        float glowTime = Random.Range(.1f, maxGlowTime);

        int loopTime = Random.Range(20,50);
        for(int i = 0; i < loopTime; i++)
        {
            float val = (float)i / (float)loopTime;

            Color col = Color.Lerp(Color.black, maxColor, val);
            mat.SetColor("_EmissionColor", col);
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(glowTime);

        for (int i = 0; i < loopTime; i++)
        {
            float val = (float)i / (float)loopTime;

            Color col = Color.Lerp( maxColor, Color.black, val);
            mat.SetColor("_EmissionColor", col);
            yield return new WaitForSeconds(0.01f);
        }

        locker = false;
        yield return null;

    }
}

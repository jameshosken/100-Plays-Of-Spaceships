using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthController : MonoBehaviour
{
    [SerializeField] Renderer hull;
    [SerializeField] Renderer window;
    Material stealthMat;
    Material windowMat;

    bool stealthIsActive = false;
    bool isTransitioning = false;

    // Start is called before the first frame update
    void Start()
    {
        stealthMat = hull.materials[0];
        windowMat = window.materials[0];

        //stealthMat.SetFloat("_Transition", 1);
        //windowMat.SetFloat("_Transition", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!isTransitioning) { 
            
                StopCoroutine("ToggleStealth");
                StartCoroutine("ToggleStealth");
            }
        }
    }

    //public void ToggleStealth()
    //{

    //    if (stealthIsActive)
    //    {

    //    }
    //    else
    //    {

    //    }

    //}

    IEnumerator ToggleStealth()
    {
        isTransitioning = true;
        float current = 1f;
        float target = 0;
        float scale = -5;
        if (stealthIsActive)
        {
            target = 1f;
            current = 0;
            scale = 5;
        }
        stealthIsActive = !stealthIsActive;
        stealthMat.SetFloat("_Transition", current);
        stealthMat.SetFloat("_Scale", scale);

        float transitionTime = 3f;
        int cycles = 100;
        for (int i = 0; i < cycles; i++)
        {

            float interpolation = 1 / (float)cycles * (float)i;
            //interpolation = Mathf.SmoothStep(0, 1, interpolation);        //Ease in/out the value to Slerp by
            float t = Mathf.Lerp(current, target, interpolation);


            windowMat.SetFloat("_Transition", 1f - t);
            t = map(t, 0, 1, -1, 3.5f);
            stealthMat.SetFloat("_Transition", t);




            yield return new WaitForSeconds(transitionTime / (float)cycles);
        }

        isTransitioning = false;
        yield return null;
    }

    float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }


}

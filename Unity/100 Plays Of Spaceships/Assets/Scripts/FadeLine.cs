using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeLine : MonoBehaviour
{

    DestroyAfterTime killer;
    LineRenderer line;
    float fader;
    // Start is called before the first frame update
    void Start()
    {
        killer = GetComponent<DestroyAfterTime>();
        line = GetComponent<LineRenderer>();

        fader = Random.Range(1f, 2f);

    }

    // Update is called once per frame
    void Update()
    {
        Color lineStartColour = line.startColor;

        Color lineEndColour = line.endColor;
        if(lineStartColour.grayscale < .2f)
        {
            lineStartColour = Color.clear;
            lineEndColour = Color.clear;
        }

        Color startLerper = Color.Lerp(lineStartColour, Color.clear, fader * Time.deltaTime);
        Color endLerper = Color.Lerp(lineEndColour, Color.clear, fader * Time.deltaTime);

        line.startColor = startLerper;
        line.endColor = endLerper;

        //float width = line.widthMultiplier;
        //line.widthMultiplier = Mathf.Lerp(width, 0, fader);


    }
}

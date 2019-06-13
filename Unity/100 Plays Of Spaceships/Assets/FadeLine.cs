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

        fader = Random.Range(0.01f, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        Color lineStartColour = line.startColor;
        Color startLerper = Color.Lerp(lineStartColour, Color.clear, fader);

        Color lineEndColour = line.endColor;
        Color endLerper = Color.Lerp(lineEndColour, Color.clear, fader*0.5f);
        line.startColor = startLerper;
        line.endColor = endLerper;
    }
}

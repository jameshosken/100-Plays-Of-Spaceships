using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOverLife : MonoBehaviour
{
    [SerializeField] Color colour;
    DestroyAfterTime destroyScript;
    Material mat;
    float lifetime;
    float counter;
    float startTime;
    
    // Start is called before the first frame update
    void Start()
    {
        destroyScript = GetComponent<DestroyAfterTime>();
        lifetime = destroyScript.selfDestructTime;
        counter = 0;
        mat = GetComponent<Renderer>().materials[0] ;
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        counter = Time.time - startTime;

        Color col = Color.Lerp(colour, Color.black, ( (counter )/ lifetime));

        print((counter) / lifetime);

        mat.SetColor("_EmissionColor", col);

    }
}

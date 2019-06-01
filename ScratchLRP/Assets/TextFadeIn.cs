using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextFadeIn : MonoBehaviour
{

    public float dilation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        
        dilation = Mathf.Lerp(dilation, 0, 0.1f * Time.deltaTime);
    }
}

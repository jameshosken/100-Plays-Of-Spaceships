using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeTransition : MonoBehaviour
{

    Material mat;
    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<Renderer>().sharedMaterial;

    }

    public void SetTransitionValue(float val)
    {
        mat.SetFloat("_Transition", val);
    }

    
}

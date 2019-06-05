using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LerpToClear : MonoBehaviour
{
    Image img;
    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();    
    }

    // Update is called once per frame
    void Update()
    {
        img.color = Color.Lerp(img.color, Color.clear, 0.05f);
    }

    public void OnHit()
    {
        img.color = Color.red;
    }
}

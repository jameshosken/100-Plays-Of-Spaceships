using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayAltitude : MonoBehaviour
{

    [SerializeField] TextMeshPro altitudeDisplay;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        altitudeDisplay.text = System.Math.Round(transform.position.y).ToString();
    }
}

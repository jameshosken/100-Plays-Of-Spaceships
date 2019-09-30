using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MapRotationFromSlider : MonoBehaviour
{
    [SerializeField] float min;
    [SerializeField] float max;
    [SerializeField] Slider slider;

    Vector3 startRot;
    float pValue = 0;
    // Start is called before the first frame update
    void Start()
    {
        startRot = transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        float val = slider.value;
        if(val != pValue)
        {
            transform.rotation = Quaternion.Euler(new Vector3(startRot.x, Mathf.Lerp(min, max, val), startRot.z));
        }

    }

}

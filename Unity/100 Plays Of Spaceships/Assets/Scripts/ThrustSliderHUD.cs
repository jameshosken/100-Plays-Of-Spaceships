using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrustSliderHUD : MonoBehaviour
{
    
    [SerializeField] Slider accel;
    [SerializeField] Slider deccel;

    [SerializeField] Slider lateralPort;
    [SerializeField] Slider lateralStarboard;


    void ApplyThrust(float thrust)
    {
        if(thrust >= 0)
        {
            accel.value = Mathf.Lerp(accel.value, thrust, 0.1f);
            deccel.value = Mathf.Lerp(deccel.value, 0, 0.1f); 
        }
        else 
        {
            deccel.value = Mathf.Lerp(deccel.value, Mathf.Abs(thrust), 0.1f);
            accel.value = Mathf.Lerp(accel.value, 0, 0.1f);
        }
    }

    void ApplyLateral(float thrust)
    {
        if (thrust >= 0)
        {
            lateralStarboard.value = Mathf.Lerp(lateralStarboard.value, thrust, 0.1f);
            lateralPort.value = Mathf.Lerp(lateralPort.value, 0, 0.1f);
        }
        else
        {
            lateralPort.value = Mathf.Lerp(lateralPort.value, Mathf.Abs(thrust), 0.1f);
            lateralStarboard.value = Mathf.Lerp(lateralStarboard.value, 0, 0.1f);
        }
    }

}

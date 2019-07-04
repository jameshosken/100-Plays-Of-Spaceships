using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlFeedbackDisplay : MonoBehaviour
{

    [SerializeField] GameObject[] thrust;
    [SerializeField] GameObject[] yaw;
    [SerializeField] GameObject[] pitch;
    [SerializeField] GameObject[] roll;
    [SerializeField] GameObject[] x;
    [SerializeField] GameObject[] y;


    public void SetThrust(int val)
    {
        switch (val)
        {
            case 0:
                thrust[0].SetActive(false);
                thrust[1].SetActive(false);
                break;
            case 1:
                thrust[0].SetActive(true);
                thrust[1].SetActive(false);
                break;
            case -1:
                thrust[0].SetActive(false);
                thrust[1].SetActive(true);
                break;
            default:
                break;
        }
    }

    public void SetPitch(int val)
    {
        switch (val)
        {
            case 0:
                pitch[0].SetActive(false);
                pitch[1].SetActive(false);
                break;
            case 1:
                pitch[0].SetActive(true);
                pitch[1].SetActive(false);
                break;
            case -1:
                pitch[0].SetActive(false);
                pitch[1].SetActive(true);
                break;
            default:
                break;
        }
    }

    public void SetYaw(int val)
    {
        switch (val)
        {
            case 0:
                yaw[0].SetActive(false);
                yaw[1].SetActive(false);
                break;
            case 1:
                yaw[0].SetActive(true);
                yaw[1].SetActive(false);
                break;
            case -1:
                yaw[0].SetActive(false);
                yaw[1].SetActive(true);
                break;
            default:
                break;
        }
    }

    public void SetRoll(int val)
    {
        switch (val)
        {
            case 0:
                roll[0].SetActive(false);
                roll[1].SetActive(false);
                break;
            case 1:
                roll[0].SetActive(true);
                roll[1].SetActive(false);
                break;
            case -1:
                roll[0].SetActive(false);
                roll[1].SetActive(true);
                break;
            default:
                break;
        }
    }

    public void SetX(int val)
    {
        switch (val)
        {
            case 0:
                x[0].SetActive(false);
                x[1].SetActive(false);
                break;
            case 1:
                x[0].SetActive(true);
                x[1].SetActive(false);
                break;
            case -1:
                x[0].SetActive(false);
                x[1].SetActive(true);
                break;
            default:
                break;
        }
    }
    public void SetY(int val)
    {
        switch (val)
        {
            case 0:
                y[0].SetActive(false);
                y[1].SetActive(false);
                break;
            case 1:
                y[0].SetActive(true);
                y[1].SetActive(false);
                break;
            case -1:
                y[0].SetActive(false);
                y[1].SetActive(true);
                break;
            default:
                break;
        }
    }

}

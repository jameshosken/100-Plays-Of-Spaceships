using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodracerJointBreakHandler : MonoBehaviour
{
    [SerializeField] Joint engineRightJoint;
    [SerializeField] Joint engineLeftJoint;
    [SerializeField] GameObject pod;
    [SerializeField] PodracerEngineGlowLines engineConnectionLines;
    [SerializeField] PodConnectorLines podLines;
    PodracerControl podControls;

    Joint[] podJoints;

    bool leftIntact = true;
    bool rightIntact = true;
    bool[] podIntact = { true, true };

    void Start()
    {
        podControls = GetComponent<PodracerControl>();
        podJoints = pod.GetComponents< Joint > (); 
    }

    // Update is called once per frame
    void Update()
    {
        if (leftIntact)
        {
            if (engineLeftJoint == null)
            {
                print("Left Broken!");
                BreakEngineConnection(engineRightJoint);
                leftIntact = false;
            }
        }


        if (rightIntact)
        {
            if (engineRightJoint == null)
            {
                print("Right Broken!");

                BreakEngineConnection(engineLeftJoint);

                rightIntact = false;
            }
        }

        if (podIntact[0] )
        {
                if (podJoints[0] == null)
                {
                    print("Pod 0 Broken!");
                    BreakPodConnection(0);
                    podIntact[0] = false;
                    Invoke("BreakPod", 1f);
                }
        }

        if (podIntact[1])
        {
            if (podJoints[1] == null)
            {
                print("Pod 1 Broken!");
                BreakPodConnection(1);
                podIntact[1] = false;
                Invoke("BreakPod", 1f);
            }
        }

    }

    void BreakPod()
    {
        if (podIntact[0])
        {
                podJoints[0].breakForce = 0;
                BreakPodConnection(0);
                podIntact[0] = false;
        }

        if (podIntact[1])
        {
                podJoints[1].breakForce = 0;
                BreakPodConnection(1);
                podIntact[1] = false;
        }

        Invoke("BreakLeftEngine", UnityEngine.Random.Range(1, 2));
        Invoke("BreakRightEngine", UnityEngine.Random.Range(1, 2));
        //Invoke("Restart", 7f);
    }

    private void BreakPodConnection(int i)
    {
        podControls.loseControl();
        podLines.BreakLines(i);
        Invoke("Restart", 7f);
    }

    void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    void BreakLeftEngine()
    {
        if (leftIntact)
        {
            BreakEngineConnection(engineLeftJoint);
        }
    }

    void BreakRightEngine()
    {
        if (rightIntact)
        {
            BreakEngineConnection(engineRightJoint);
        }
    }

    private void BreakEngineConnection(Joint engineJoint)
    {
        engineConnectionLines.Break();

        if(engineJoint == null)
        {
            return;
        }
        //Break other joint
        engineJoint.breakForce = 0;
        engineJoint.breakTorque = 0;

    }
}

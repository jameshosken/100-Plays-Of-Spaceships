using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodracerAnimationHandler : MonoBehaviour
{

    [SerializeField] Animator leftEngine;
    [SerializeField] Animator rightEngine;

    int engineStates = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Stop()
    {
        if (engineStates == 0)
        {
            return;
        }
        print("Stopping");
        engineStates = 0;
        leftEngine.SetInteger("Engine State", 0);
        rightEngine.SetInteger("Engine State", 0);
    }

    // Update is called once per frame
    public void Thrust()
    {
        if(engineStates == 1)
        {
            return;
        }
        print("Boosting");
        engineStates = 1;
        leftEngine.SetInteger("Engine State", 1);
        rightEngine.SetInteger("Engine State", 1);

    }

    public void Boost()
    {
        if (engineStates == 2)
        {
            return;
        }
        print("Thrusting");
        engineStates = 2;
        leftEngine.SetInteger("Engine State", 2);
        rightEngine.SetInteger("Engine State", 2);
    }
}

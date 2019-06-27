using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class AccuracyTracker : MonoBehaviour
{

    [SerializeField] Text accuracyText;
    long bulletsFired = 0;
    long targetsHit = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(bulletsFired == 0)
        {
            return;
        }

        float accuracy = (float) targetsHit / (float)bulletsFired;
        accuracyText.text = string.Format("Accuracy: {0:#.00}", accuracy);
    }

    

    public void AddBulletsFired(int i)
    {
        bulletsFired += i;
    }
    public void AddTargetsHit()
    {
        targetsHit += 1;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollisionHandler : MonoBehaviour
{
    [SerializeField] float damage = 50f;
    AccuracyTracker accuracy;
    // Start is called before the first frame update
    void Start()
    {
        accuracy = FindObjectOfType<AccuracyTracker>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnParticleCollision(GameObject other)
    {

        if (other.GetComponent<Health>())
        {
            other.GetComponent<Health>().OnHit(damage);
            accuracy.AddTargetsHit();
        }
    }


}

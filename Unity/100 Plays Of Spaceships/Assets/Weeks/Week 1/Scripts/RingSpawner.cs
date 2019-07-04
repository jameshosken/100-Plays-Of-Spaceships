using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingSpawner : MonoBehaviour
{
    [SerializeField] int numberOfRings = 50;
    [SerializeField] float bounds;
    [SerializeField] GameObject ring;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < numberOfRings; i++)
        {
            GameObject clone = Instantiate(ring) as GameObject;

            float x = Random.Range(-bounds, bounds);
            float y = Random.Range(-bounds, bounds);
            float z = Random.Range(-bounds, bounds);
            clone.transform.position = new Vector3(x, y, z);

            clone.transform.rotation = Quaternion.Euler(x, y, z);

        }
    }


}

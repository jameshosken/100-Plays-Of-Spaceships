using System.Collections.Generic;
using UnityEngine;

public class GenericSpawner : MonoBehaviour
{

    [SerializeField] private GameObject spawnObject;
    [SerializeField] private Vector3 bounds = Vector3.one * 100;
    [SerializeField] private int amount = 50;
    [SerializeField] private float separationDistance = 5f;
    private List<Vector3> positions = new List<Vector3>();

    // Start is called before the first frame update
    private void Start()
    {
        for (int i = 0; i < amount; i++)
        {
            Vector3 newPos = new Vector3(
                UnityEngine.Random.Range(-bounds.x, bounds.x),
                UnityEngine.Random.Range(-bounds.y, bounds.y),
                UnityEngine.Random.Range(-bounds.z, bounds.z)
                );

            Vector3 newScale = Vector3.one * Random.Range(.1f, .3f);

            GameObject node =  Instantiate(spawnObject, newPos, Quaternion.identity);
            node.transform.localScale = newScale;
        }
    }

    //Try prevent interections (later);

    //float minDistance = 0;

    //        if(i == 0)
    //        {
    //            minDistance = Vector3.Distance(newPos, Vector3.zero);
    //        }

    //        while (minDistance<separationDistance)
    //        {
    //            newPos = new Vector3(
    //                UnityEngine.Random.Range(-bounds.x, bounds.x),
    //                UnityEngine.Random.Range(-bounds.y, bounds.y),
    //                UnityEngine.Random.Range(-bounds.z, bounds.z)
    //            );

    //            for (int j = 0; j<positions.Count; j++)
    //            {
    //                float currentDistance = Vector3.Distance(positions[j], newPos);
    //                if (currentDistance<minDistance)
    //                {
    //                    minDistance = currentDistance;
    //                }
    //            }
    //        }


}

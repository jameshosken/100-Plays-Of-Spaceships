using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This is now a helper class. Attach onto gameobject and call "CreateObjects" from other script.
public class RandomSpawnOnTerrain : MonoBehaviour
{
    //[SerializeField] GameObject objectToSpawn;
    //[SerializeField] int numberToSpawn;

    //[SerializeField] float xBounds;
    //[SerializeField] float zBounds;

    //[SerializeField] bool allowBounceSpace;
    


    public List<GameObject> CreateObjects(GameObject objectToSpawn, int numberToSpawn, float xBounds, float zBounds, bool allowBounceSpace) {

        List<GameObject> objects = new List<GameObject>();
        for (int i = 0; i < numberToSpawn; i++)
        {
            Vector3 raycastPosition = new Vector3(
                UnityEngine.Random.Range(transform.position.x - xBounds, transform.position.x + xBounds),
                0,
                UnityEngine.Random.Range(transform.position.z - zBounds, transform.position.z + zBounds)
                );

            //Returns position and normal of hit point

            Vector3[] spawnPos = GetSpawnPointFromRaycast(raycastPosition);
            if (spawnPos != null)
            {
                GameObject cln = Instantiate(objectToSpawn) as GameObject;
                cln.transform.position = spawnPos[0];
                cln.transform.rotation = Quaternion.Euler(spawnPos[1]);

                cln.transform.Rotate(Vector3.up, UnityEngine.Random.Range(0, 360), Space.Self);

                if (allowBounceSpace)
                {
                    cln.transform.Translate(Vector3.up * 1f, Space.Self);
                }
                objects.Add(cln);
            }
        }
        return objects;
    }

    private Vector3[] GetSpawnPointFromRaycast(Vector3 raycastPosition)
    {
        Ray ray = new Ray(raycastPosition, Vector3.down);

        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, 200f))
        {
            Vector3[] hitInfo = new Vector3[2];
            hitInfo[0] = hit.point;
            hitInfo[1] = hit.normal;

            return hitInfo;
        }
        else return null;
    }
    

}

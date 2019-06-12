using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    [SerializeField] float shieldHealth = 50f;

    int index = 0;
    int maxFXs = 4;

    public Vector4[] hit;
    String[] names = {"_Hit1", "_Hit2" , "_Hit3" , "_Hit4" };

    public Material shieldMaterial;

    [SerializeField] float fadeSpeed = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        UpdateHitPoints();

        if (shieldHealth < 0)
        {
            GameObject.Destroy(gameObject);
        }
    }

    private void UpdateHitPoints()
    {

        
        for (int i = 0; i < maxFXs; i++)
        {
            if (hit[i].w < 5)
            {

                
                hit[i].w = Mathf.Lerp(hit[i].w, 5, fadeSpeed * Time.deltaTime);
                GetComponent<Renderer>().materials[0].SetVector(names[i], hit[i]);
                //shieldMaterial.SetVector(names[i], hit[i]);
            }
            else
            {
                hit[i].w = 5;
            }
        }

    }



    public void AddHitPoint(Vector3 point, float damage)
    {

        shieldHealth -= damage;

        index = (index + 1) % maxFXs;

        Debug.Log("HIT: " + names[index]);
        Debug.Log("Add Hit Point");

        hit[index].x = point.x;
        hit[index].y = point.y;
        hit[index].z = point.z;
        hit[index].w = 0;

    }
}

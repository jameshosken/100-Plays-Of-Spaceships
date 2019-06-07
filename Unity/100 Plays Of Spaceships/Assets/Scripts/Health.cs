using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float health = 100;
    [SerializeField] GameObject deathFX;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (health < 0)
        {
            GameObject obj = Instantiate(deathFX, transform.position, transform.rotation) as GameObject;
            GameObject.Destroy(gameObject);
        }
    }

    public void LoseHealth(float damage)
    {
        health -= damage;
        Debug.Log("HEALTH: " + health);
    }
}

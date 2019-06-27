using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

            FindObjectOfType<ScoreTracker>().AddSCore(1);
            GameObject.Destroy(gameObject);
        }
    }

    public void OnHit(float damage)
    {
        health -= damage;
        Debug.Log("HEALTH: " + health);
    }
}

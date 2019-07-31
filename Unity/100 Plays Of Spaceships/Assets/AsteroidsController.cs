using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidsController : MonoBehaviour
{

    [SerializeField] GameObject asteroidTemplate;

    [SerializeField] Vector2 spawnBounds;

    [Tooltip("Percent")]
    [SerializeField] float spawnRate;

    [SerializeField] float minDistFromPlayer = 5f;


    // Update is called once per frame
    void Update()
    {
        if (UnityEngine.Random.Range(0f, 100f) < spawnRate)
        {
            GenerateAsteroid();
        }
    }

    private void GenerateAsteroid()
    {

        Vector2 randomPosition = Vector2.zero;

        //Avoid spawning on player
        while (Vector3.Distance(
            randomPosition,
            Vector2.zero)
            < minDistFromPlayer)
        {
            randomPosition = new Vector2(
            UnityEngine.Random.Range(-spawnBounds.x, spawnBounds.x),
            UnityEngine.Random.Range(-spawnBounds.y, spawnBounds.y)
            );
        }

        GameObject roid = Instantiate(asteroidTemplate) as GameObject;

        roid.transform.position = new Vector3(
            transform.position.x + randomPosition.x,
            0,
            transform.position.z + randomPosition.y);

    }
}

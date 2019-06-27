using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TopDownShooterGameEngine : MonoBehaviour
{

    [SerializeField] GameObject target;
    [SerializeField] float timeBetweenWaves;
    [SerializeField] Vector3 targetSpeed;
    [SerializeField] int targetsPerSpawn;

    [SerializeField] float xRange = 40;
    [SerializeField] float zRange = 6;

    [SerializeField] GameObject loseMessage;


    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnWave", 2f, timeBetweenWaves);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnWave()
    {
        for (int i = 0; i < targetsPerSpawn; i++)
        {
            GameObject clone = Instantiate(target) as GameObject;

            clone.transform.position = transform.position;  //set to parent
            clone.transform.parent = transform;

            clone.transform.Translate(Random.Range(-xRange, xRange), 0, Random.Range(-zRange, zRange));

            clone.GetComponent<AutoMove>().SetMoveVector(targetSpeed * Random.Range(.9f,1.3f));
        }
        targetsPerSpawn++;
        targetSpeed *= 1.1f;
        
    }

    public void OnLose()
    {
        loseMessage.SetActive(true);

        Invoke("ResetGame", 3);
    }

    void ResetGame()
    {
        SceneManager.LoadScene(0);
    }
}

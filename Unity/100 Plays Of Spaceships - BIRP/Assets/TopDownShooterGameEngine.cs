using UnityEngine;
using UnityEngine.SceneManagement;

public class TopDownShooterGameEngine : MonoBehaviour
{

    [SerializeField] private GameObject target;
    [SerializeField] private float timeBetweenWaves;
    [SerializeField] private Vector3 targetSpeed;
    [SerializeField] Vector3 speedIncrement;
    [SerializeField] private int targetsPerSpawn;

    [SerializeField] private float xRange = 40;
    [SerializeField] private float zRange = 6;

    [SerializeField] private GameObject loseMessage;


    // Start is called before the first frame update
    private void Start()
    {
        InvokeRepeating("SpawnWave", 2f, timeBetweenWaves);
    }

    // Update is called once per frame
    private void Update()
    {

    }

    private void SpawnWave()
    {
        for (int i = 0; i < targetsPerSpawn; i++)
        {
            GameObject clone = Instantiate(target) as GameObject;

            clone.transform.position = transform.position;  //set to parent
            clone.transform.parent = transform;

            clone.transform.Translate(Random.Range(-xRange, xRange), 0, Random.Range(-zRange, zRange));


            
            if(Random.Range(0,100) < 20)
            {
                targetSpeed.x = Random.Range(-10, 10);
            }
            else
            {
                targetSpeed.x = 0;
            }
            clone.GetComponent<AutoMove>().SetMoveVector(targetSpeed * Random.Range(.9f, 1.5f));

            
        }

        targetSpeed +=  speedIncrement;


    }

    public void OnLose()
    {
        loseMessage.SetActive(true);

        Invoke("ResetGame", 3);
    }

    private void ResetGame()
    {
        SceneManager.LoadScene(0);
    }
}

using UnityEngine;
using System.Collections.Generic;
using System;

public class FlappyTileController : MonoBehaviour
{
    FlappyGameEngine gameEngine;

    [SerializeField] GameObject tileTemplate;
    [SerializeField] GameObject obstacleTemplate;
    [SerializeField] private int tileSize = 50;
    [SerializeField] private int obstacleSpacing = 5;
    [SerializeField] private float obstacleProbability = .6f;
    [SerializeField] float yOffsetRange = 10f;
    List<GameObject> obstacles = new List<GameObject>();

    float speed;

    // Start is called before the first frame update
    private void Start()
    {

        tileTemplate = (GameObject)Resources.Load("FlappyTile");
        gameEngine = FindObjectOfType<FlappyGameEngine>();
        speed = gameEngine.gameSpeed;

        float prevTileHeight = 0;
        
        for (int i = 0; i < tileSize; i += obstacleSpacing)
        {
            if (UnityEngine.Random.Range(0f, 1f) < obstacleProbability)
            {

                float offsetY = UnityEngine.Random.Range(-yOffsetRange, yOffsetRange);
                float offsetX = i;

                //Ensure no large jumps:
                while(Mathf.Abs(offsetY - prevTileHeight) > yOffsetRange)
                {
                    offsetY = UnityEngine.Random.Range(-yOffsetRange, yOffsetRange);
                }
                Vector3 positionOffset = new Vector3(offsetX, offsetY, 0);

                GameObject obstacle = Instantiate(obstacleTemplate) as GameObject;
                obstacle.transform.position = transform.position + positionOffset;
                obstacle.transform.localScale =  new Vector3(obstacle.transform.localScale.x * UnityEngine.Random.Range(1, 3), obstacle.transform.localScale.y, obstacle.transform.localScale.z);
                obstacle.transform.parent = this.transform;
                prevTileHeight = offsetY;
            }
            else
            {
                prevTileHeight = 0; //Reset if no obstacle
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {

        transform.Translate(new Vector3(-speed*Time.deltaTime, 0, 0));

        if (transform.position.x <= tileSize * -1)
        {
            HandleEdges();
            for (int i = transform.childCount - 1; i >=0 ; i--)
            {
                Transform child = transform.GetChild(i);
                GameObject.DestroyImmediate(child.gameObject);
            }

            GameObject.Destroy(this.gameObject);

        }
    }

    private void HandleEdges()
    {
        Vector3 newPostion = new Vector3(-1 * transform.position.x , transform.position.y, transform.position.z);
        GameObject newTile = Instantiate(tileTemplate, newPostion, Quaternion.identity) as GameObject;
        newTile.transform.parent = transform.parent;

    }
}

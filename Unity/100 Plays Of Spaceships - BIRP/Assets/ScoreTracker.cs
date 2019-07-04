using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTracker : MonoBehaviour
{

    Text scoreText;

    int score = 0;
    // Start is called before the first frame update
    void Start()
    {
        scoreText = GameObject.Find("Score").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddSCore(int i)
    {
        score += i;

        scoreText.text = "Score: " + score.ToString();
    }

    public void HalveScore()
    {
        score = score / 2;

        scoreText.text = "Score: " + score.ToString();
    }

}

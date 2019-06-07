using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

public class AnalyticsBasic : MonoBehaviour
{
    private Scene thisScene;
    private float secondsElapsed = 0;


    void Awake()
    {
        thisScene = SceneManager.GetActiveScene();
        AnalyticsEvent.LevelStart(thisScene.name,
                                      thisScene.buildIndex);
    }

    void Update()
    {
        secondsElapsed += Time.deltaTime;
    }

    void OnDestroy()
    {
        Dictionary<string, object> customParams = new Dictionary<string, object>();
        customParams.Add("seconds_played", secondsElapsed);

        AnalyticsEvent.LevelQuit(thisScene.name,
            thisScene.buildIndex,
            customParams);
        
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadLevelAsync : MonoBehaviour
{
    [SerializeField] Slider progress;
    [SerializeField] string levelToLoad;

    float counter = 0;
    // Start is called before the first frame update
    public void LoadLevel()
    {
        StartCoroutine(LoadAsyncScene());
    }


    IEnumerator LoadAsyncScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        

        // Wait until the asynchronous scene fully loads
        //while (!asyncLoad.isDone)
        //{
        //    progress.value = asyncLoad.progress;
        //    counter += 0.1;
        //    yield return new WaitForSeconds(0.1);
        //}
        for (int i = 0; i < 300; i++)
        {
            progress.value = counter;
            counter += 1f/300f;
            yield return new WaitForSeconds(0.01f);
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelToLoad);
    }
}

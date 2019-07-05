using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class ReloadScene : MonoBehaviour
{
    [SerializeField] int sceneToLoad = 1;

    public void Reload()
    {
        SceneManager.LoadScene(1);
    }
}

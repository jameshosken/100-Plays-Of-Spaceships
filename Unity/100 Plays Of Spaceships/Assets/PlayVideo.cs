using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayVideo : MonoBehaviour
{

    UnityEngine.Video.VideoPlayer player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<UnityEngine.Video.VideoPlayer>();


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        print("Playing");
        player.Play();
    }

    public void Pause()
    {
        player.Pause();
    }

    public void Stop()
    {
        player.Stop();
    }
}

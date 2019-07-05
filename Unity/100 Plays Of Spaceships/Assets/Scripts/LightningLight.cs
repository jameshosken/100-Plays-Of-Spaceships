using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningLight : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float bounds = 50;
    [SerializeField] float fadeTime = 0.05f;


    float maxTime = .5f;
    Light light;


    // Start is called before the first frame update
    void Start()
    {

        Vector3 randomVec = new Vector3(
            Random.Range(-bounds, bounds),
            Random.Range(-bounds, bounds),
            Random.Range(-bounds, bounds)
            );


        player = GameObject.Find("Player").transform;


        transform.position = player.position + randomVec + player.forward * 25;


        light = GetComponent<Light>();

        Strike();


        float secondStrikeTime = Random.Range(.1f, maxTime);
        Invoke("Strike", secondStrikeTime);


    }

    // Update is called once per frame
    void Update()
    {
        light.intensity = Mathf.Lerp(light.intensity, 0, fadeTime);
    }

    void Strike()
    {
        light.intensity = 200;
    }
}

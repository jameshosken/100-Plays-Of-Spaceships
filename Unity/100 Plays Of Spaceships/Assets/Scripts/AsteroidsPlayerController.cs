using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidsPlayerController : MonoBehaviour
{

    [SerializeField] float turnRate = 0.1f;
    [SerializeField] float thrustRate = 1f;

    [SerializeField] Renderer shipRenderer;
    [SerializeField] ParticleSystem gun;

    [SerializeField] GameObject deathFX;
    

    Color engineCol;

    Material engineMat;

    Rigidbody body;

    
    
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        engineMat = shipRenderer.materials[1];

        engineCol = engineMat.GetColor("_EmissionColor");
    }

    // Update is called once per frame
    void Update()
    {

        HandleShooting();

        float turn = Input.GetAxis("Horizontal") * turnRate * Time.deltaTime;

        float thrust = Input.GetAxis("Vertical") ;
        engineMat.SetColor("_EmissionColor", Color.Lerp(Color.clear, engineCol, thrust));

        float thrustActual = thrust * thrustRate * Time.deltaTime;

        Vector3 rotationVec = transform.up * turn;

        Vector3 thrustVec = transform.forward * thrustActual;


        body.AddTorque(rotationVec);
        body.AddForce(thrustVec);

    }

    private void HandleShooting()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gun.Emit(1);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject death = Instantiate(deathFX) as GameObject;

        death.transform.position = transform.position;


        Invoke("Reset", 4f);
        gameObject.SetActive(false);

    }

    private void Reset()
    {

        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}

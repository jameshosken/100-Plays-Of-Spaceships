using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FlappyController : MonoBehaviour
{
    [SerializeField] Transform mainPivot;
    [SerializeField] float mainPivotRotationAmount = 1;
    [SerializeField] Transform enginePivot;
    [SerializeField] Transform engineLookAt;
    [SerializeField] float upForce = 1000f;
    [SerializeField] int upForceDuration = 10;
    [SerializeField] Vector3 gravity;

    [SerializeField] Renderer engineRenderer;
    [ColorUsageAttribute(true, true)]
    [SerializeField] Color engineOnColour;

    [SerializeField] Text scoreText;

    Material engineMat;
    ParticleSystem particles;

    Vector3 originalRotation;
    Rigidbody body;

    int score = 0;

    public bool isDead = false;


    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();

        Physics.gravity = gravity;

        originalRotation = mainPivot.transform.rotation.eulerAngles;

        engineMat = engineRenderer.materials[2];    //Based on engine materials index

        particles = GetComponentInChildren<ParticleSystem>();

    }

    // Update is called once per frame
    void Update()
    {

        if (isDead)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            HandleSpacebar();
        }

        HandleEngineMaterial();
    }

    private void HandleEngineMaterial()
    {
        engineMat.SetColor("_EmissionColor", Color.Lerp(engineMat.GetColor("_EmissionColor"), Color.black, 0.2f) );
    }

    private void HandleSpacebar()
    {
        particles.Emit(30);
        engineMat.SetColor("_EmissionColor", engineOnColour);
        StartCoroutine("ApplyForce");
    }

    IEnumerator ApplyForce()
    {
        for (int i = 0; i < upForceDuration; i++)
        {
            body.AddForce(Vector3.up * upForce);
            yield return null;
        }
        
    }

    private void FixedUpdate()
    {
        if (isDead)
        {
            return;
        }

        EnginePivotPointDown();
        HandleMainLookDirection();
    }

    void HandleMainLookDirection()
    {
        float verticalSpeed = body.velocity.y;
        Vector3 rotation = new Vector3(verticalSpeed, 0, 0) * mainPivotRotationAmount;

        mainPivot.rotation = Quaternion.Euler(originalRotation +  rotation);
    }


    void EnginePivotPointDown()
    {
        enginePivot.LookAt(engineLookAt, Vector3.left);
    }

    private void OnCollisionEnter(Collision collision)

    {
        if (isDead)
        {
            return;
        }
        body.constraints = RigidbodyConstraints.None;

        body.AddExplosionForce(500f, collision.contacts[0].point, 50);
        body.AddRelativeTorque(Vector3.forward * 1000);
        Invoke("Reload", 1f);
        isDead = true;
    }

    void Reload()
    {
        SceneManager.LoadScene("Day21");
    }

    private void OnTriggerEnter(Collider other)
    {
        score++;
        scoreText.text = "SCORE: " + score.ToString();
    }
}

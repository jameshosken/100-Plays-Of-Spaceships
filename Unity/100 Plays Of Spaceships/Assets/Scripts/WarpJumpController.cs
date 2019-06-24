using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpJumpController : MonoBehaviour
{
    [SerializeField] GameObject jumpLines;
    [Tooltip("Model to apply warp FX to")]
    [SerializeField] Transform scalableObject;
    [SerializeField] AutoRotate rotationObject;
    [SerializeField] GameObject warpParticleController;
    [SerializeField] GameObject warpShockwave;
    WarpJumpParticleController controller;
    float armSpinSpeed = 150;
    Vector3 originalScale;

    // Start is called before the first frame update
    void Start()
    {
        originalScale = scalableObject.localScale;

    }

    private void Update()
    {
        if (controller)
        {
            controller.transform.rotation = transform.rotation;
        }
    }

    public void BeginTurnSequence(Vector3 _t)
    {

        StopCoroutine("InitiateTurn");
        StartCoroutine("InitiateTurn");
    }

    public void BeginJumpSequence(Vector3 _t)
    {

        StopCoroutine("InitiateQuantumJump");
        StartCoroutine("InitiateQuantumJump", _t);
    }

    IEnumerator InitiateQuantumJump(Vector3 target)
    {

        controller.SetEmission(20);

        float maxRot = 30;
        int cycles = 100;
        float timeToSpinUp = 3f;
        for (int i = 0; i < cycles; i++)
        {
            float c = (1 / (float)cycles) * (float)i;
            float rotSpeed = Mathf.SmoothStep(10f, maxRot, c);

            controller.SetForceRotation(rotSpeed, 1);

            yield return new WaitForSeconds(timeToSpinUp / (float)cycles);
        }

        controller.SetEmission(0);
        controller.SetForceDrag(100);



        CreateLines(target, transform.position);
        transform.position = target;

        GameObject ws = Instantiate(warpShockwave, transform.position, transform.rotation) as GameObject;


        StopCoroutine("StopTurn");
        StopCoroutine("InitiateQuantumJump");
        StartCoroutine("StopTurn");

        controller = null;
        yield return null;
    }

    IEnumerator InitiateTurn()
    {
        GameObject WPC = Instantiate(warpParticleController, transform.position, Quaternion.identity) as GameObject;
        WPC.name = "Departure";
        controller = WPC.GetComponent<WarpJumpParticleController>();

        controller.SetEmission(2);
        controller.SetForceRotation(10f,1);

        int cycles = 100;
        float timeToSpinUp = 5f;
        for (int i = 0; i < cycles; i++)
        {
            float c = (1 / (float)cycles) * (float)i;
            float yRot = Mathf.SmoothStep(0f, armSpinSpeed, c);
        
            rotationObject.SetRotation(new Vector3(0, yRot, 0));

            yield return new WaitForSeconds(timeToSpinUp / (float)cycles);
        }

        yield return null;
    }

    IEnumerator StopTurn()
    {

        int cycles = 100;
        float timeToSpinUp = 5f;
        for (int i = 0; i < cycles; i++)
        {
            float c = (1 / (float)cycles) * (float)i;
            float yRot = Mathf.SmoothStep(armSpinSpeed, 0f, c);
            rotationObject.SetRotation(new Vector3(0, yRot, 0));
            yield return new WaitForSeconds(timeToSpinUp / (float)cycles);
        }

        yield return null;
    }

    private void CreateLines(Vector3 _t, Vector3 pos)
    {
        GameObject lineObj = Instantiate(jumpLines) as GameObject;
        TeleportLinesController lineController = lineObj.GetComponent<TeleportLinesController>();
        lineController.SetLineStart(pos);
        lineController.SetLineEnd(_t);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportToTarget : MonoBehaviour
{

    [SerializeField] GameObject teleportLines;

    [SerializeField] GameObject teleportParticles;
    [SerializeField] int numberOfParticles = 50;
    [Tooltip("Model to apply warp FX to")]
    [SerializeField] Transform model;

    [SerializeField] ParticleSystem ChargingParticles;
    [SerializeField] float emissionRate = 5;

    Vector3 originalScale;

    // Start is called before the first frame update
    void Start()
    {
        originalScale = model.localScale;

        ParticleSystem.EmissionModule em = ChargingParticles.emission;
        em.rateOverTime = 0;
    }

    public void BeginJumpSequence(Vector3 _t) { 
    
        StopCoroutine("JumpWithScale");
        StartCoroutine("JumpWithScale", _t);
    }

    IEnumerator JumpWithScale(Vector3 _t)
    {
        
        Vector3 anticipationPosition = Vector3.back * 4;
        Vector3 targetScale = new Vector3(.4f, .4f, 2f);


        ParticleSystem.EmissionModule em = ChargingParticles.emission;
        em.rateOverTime = emissionRate;

        //Pre Jump
        yield return new WaitForSeconds(3);
        
        float c = 0;
        while (model.localPosition != anticipationPosition)
        {
            model.localPosition = Vector3.Lerp(Vector3.zero, anticipationPosition, c);
            model.localScale = Vector3.Lerp(originalScale/2, targetScale, c);
            c += 5f * Time.deltaTime;

            yield return null;

        }



        //Jump
        model.localPosition = Vector3.zero;
        model.localScale = originalScale;

        teleportParticles.GetComponent<ParticleSystem>().Emit(numberOfParticles);
        

        em.rateOverTime = 0;


        targetScale = new Vector3(.5f, .5f, 5);
        Vector3 posVel = Vector3.zero;
        Vector3 scaleVel = Vector3.zero;

        float t = 0.05f;

        while ((transform.position - _t).magnitude > 0.1f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, _t, ref posVel, t);
            model.localScale = Vector3.SmoothDamp(model.localScale, targetScale, ref scaleVel, t);
            yield return null;
        }

        Debug.Log(model.localScale);
        scaleVel = Vector3.zero;

        while ((model.localScale - Vector3.one).magnitude > 0.1f)
        {
            model.localScale = Vector3.SmoothDamp(model.localScale, originalScale*.6f, ref scaleVel, t);
            yield return null;
        }
        scaleVel = Vector3.zero;

        while ((model.localScale - Vector3.one).magnitude > 0.01f)
        {
            model.localScale = Vector3.SmoothDamp(model.localScale, originalScale, ref scaleVel, t);
            yield return null;
        }

        model.localScale = originalScale;
        yield return null;

    }

    private void CreateLines(Vector3 _t, Vector3 pos)
    {
        GameObject lineObj = Instantiate(teleportLines) as GameObject;
        TeleportLinesController lineController = lineObj.GetComponent<TeleportLinesController>();
        lineController.SetLineStart(pos);
        lineController.SetLineEnd(_t);
    }
}

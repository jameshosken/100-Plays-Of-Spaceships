using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuantumJump : MonoBehaviour
{

    [Tooltip("Model to apply warp FX to")]
    [SerializeField] Transform scalableObject;
    [SerializeField] GameObject quantumParticleControllerObject;

    

    Vector3 originalScale;

    // Start is called before the first frame update
    void Start()
    {

        originalScale = scalableObject.localScale;
    }

    public void BeginJumpSequence(Vector3 _t)
    {

        StopCoroutine("InitiateQuantumJump");
        StartCoroutine("InitiateQuantumJump", _t);
    }

    public void BeginTurnSequence(Vector3 _t)
    {
        //Do nothing
    }

    IEnumerator InitiateQuantumJump(Vector3 target)
    {
        GameObject QPC = Instantiate(quantumParticleControllerObject, transform.position, Quaternion.identity) as GameObject;
        QPC.name = "Departure";
        QuantumParticleController controller = QPC.GetComponent<QuantumParticleController>();

        //Start: Rotation, no grav
        controller.Emit(10);
        controller.SetEmission(5);
        controller.SetForceRotation(15, 1);
        controller.SetForceGravity(0);
        controller.SetForceDrag(0);

        yield return new WaitForSeconds(2);

        //Grow:
        controller.SetForceGravity(-.1f);
        controller.SetForceDrag(1);
        controller.SetForceRotation(0, 1);

        int timeInFrames = 25;
        float growTime = 0.25f;
        for (int i = 0; i < timeInFrames; i++)
        {
            float c = (1 / (float)timeInFrames) * (float)i;

            float factor = 1.2f;
            float x = Mathf.SmoothStep(originalScale.x, originalScale.x * factor, c);
            float y = Mathf.SmoothStep(originalScale.y, originalScale.y * factor, c);
            float z = Mathf.SmoothStep(originalScale.z, originalScale.z * factor, c);

            scalableObject.localScale = new Vector3(x, y, z);
            yield return new WaitForSeconds( growTime / (float)timeInFrames);        //Wait for a little bit, making this a function of time not frames.
        }

        //Shrink:
        controller.SetEmission(0);
        controller.SetForceGravity(10);

        timeInFrames = 25;
        float shrinkTime = 0.3f;
        for (int i = 0; i < timeInFrames; i++)
        {
            float c = (1 / (float)timeInFrames) * (float)i;

            float x = Mathf.SmoothStep(originalScale.x, 0, c);
            float y = Mathf.SmoothStep(originalScale.y, 0, c);
            float z = Mathf.SmoothStep(originalScale.z, 0, c);

            scalableObject.localScale = new Vector3(x, y, z);
            yield return new WaitForSeconds(shrinkTime / (float)timeInFrames);
        }

        yield return new WaitForSeconds(1);

        transform.position = target;

        //Create landing QPC
        QPC = Instantiate(quantumParticleControllerObject, transform.position, Quaternion.identity) as GameObject;
        QPC.name = "Arrival";
        controller = null;
        controller = QPC.GetComponent<QuantumParticleController>();

        //controller.SetEmissionSphere(1f);
        controller.Emit(10);
        controller.SetEmission(5);
        controller.SetForceGravity(10);
        controller.SetForceRotation(0, 1);
        controller.SetForceDrag(1);


        yield return new WaitForSeconds(1f);

        controller.SetForceGravity(-1);
        controller.Emit(10);



        yield return new WaitForSeconds(.2f);


        //Arrive Grow
        timeInFrames = 10;
        float arriveTime = 0.1f;
        for (int i = 0; i < timeInFrames; i++)
        {
            float c = (1 / (float)timeInFrames) * (float)i;
            float factor = 1.1f;
            float x = Mathf.SmoothStep(0, originalScale.x * factor, c);
            float y = Mathf.SmoothStep(0, originalScale.y * factor, c);
            float z = Mathf.SmoothStep(0, originalScale.z * factor, c);

            scalableObject.localScale = new Vector3(x, y, z);
            yield return new WaitForSeconds(arriveTime / (float)timeInFrames);
        }

        controller.SetForceGravity(0);
        controller.SetForceRotation(15, .1f);
        controller.SetForceDrag(0);

        //Arrive Settle
        timeInFrames = 10;
        float settleTime = 0.1f;
        for (int i = 0; i < timeInFrames; i++)
        {
            float c = (1 / (float)timeInFrames) * (float)i;
            float factor = 1.1f;
            float x = Mathf.SmoothStep(originalScale.x * factor, originalScale.x, c);
            float y = Mathf.SmoothStep(originalScale.y * factor, originalScale.y, c);
            float z = Mathf.SmoothStep(originalScale.z * factor, originalScale.z, c);
            scalableObject.localScale = new Vector3(x, y, z);
            yield return new WaitForSeconds(settleTime / (float)timeInFrames);
        }


        scalableObject.localScale = originalScale;
        controller.SetEmission(0);
        yield return null;

    }

}

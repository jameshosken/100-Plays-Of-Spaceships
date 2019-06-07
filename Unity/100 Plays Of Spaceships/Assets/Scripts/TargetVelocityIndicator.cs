using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TargetVelocityIndicator : MonoBehaviour
{
    
    [SerializeField] Rigidbody player;
    [SerializeField] Rigidbody target;
    [SerializeField] Color targetColour;

    [SerializeField] Text text;
    [SerializeField] float defaultTargetDistance = 100;

    Transform estimatedTarget;

    FireControls fireControls;
    Camera cam;

    Image targettingImage;

    float distToTarget = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        //Fire controls contain targetting distance
        fireControls = player.gameObject.GetComponent<FireControls>();
        cam = Camera.main;

        GameObject trgtTransform = new GameObject();
        trgtTransform.name = "Targeting Assist";
        estimatedTarget = trgtTransform.transform;

        targettingImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

        if(target == null)
        {
            Color col = Color.Lerp(targettingImage.color, Color.clear, 0.1f);
            targettingImage.color = Color.Lerp(targettingImage.color, Color.clear, 0.1f);
            text.color = col;

            fireControls.SetTarget(defaultTargetDistance);    
            
        }
        else
        {
            float dot = Vector3.Dot(player.transform.forward, target.transform.position - player.transform.position);

            if (dot > 0)
            {
                Color col = Color.Lerp(targettingImage.color, targetColour, 0.1f);
                targettingImage.color = col;
                text.color = col;

                //Advanced estimaton algorithm:

                //Get flight time (from distance)
                //get projected position.
                //Get difference in flight time.
                //Ust that as multiplier for target position based on target velocity.

                float timeToTarget = GetLaserFlightTime(target.transform.position);
                Vector3 projection = target.transform.position + (target.velocity - player.velocity) * timeToTarget;

                float timeToProjection = GetLaserFlightTime(projection);
                //projection / original
                float projectionMultiplier = timeToProjection / timeToTarget;
                
                estimatedTarget.position = target.transform.position + (target.velocity - player.velocity) * projectionMultiplier * GetLaserFlightTime(target.transform.position);
                SetDistToTarget(estimatedTarget.position);
                SetText(distToTarget);

                if (Time.frameCount % 20 == 0)
                {
                    print(projectionMultiplier);
                }

                //fireControls.SetActualTarget(estimatedTarget);
                //Old Algorithm
                estimatedTarget.position = target.transform.position + (target.velocity - player.velocity) * GetLaserFlightTime(target.transform.position);

                Vector3 screenPos = cam.WorldToScreenPoint(estimatedTarget.position);
                Vector2 screenPosV2 = new Vector2(screenPos.x, screenPos.y);

                targettingImage.rectTransform.position = screenPosV2;

            }
            else
            {
                Color col = Color.Lerp(targettingImage.color, Color.clear, 0.1f);
                targettingImage.color = Color.Lerp(targettingImage.color, Color.clear, 0.1f);
                text.color = col;
                fireControls.SetTarget(defaultTargetDistance);
            }

            fireControls.SetTarget(distToTarget);
        }

        
    }

    void SetText(float dist)
    {
        text.text = Mathf.Round(dist).ToString();
    }

    void SetDistToTarget(Vector3 trgt)
    {
        distToTarget = Vector3.Distance(player.transform.position, trgt);

    }

    float GetLaserFlightTime(Vector3 position)
    {
    //Distance to trgt
    float dist = Vector3.Distance(player.transform.position, position);

    //Laser travels distance in:
    float timeToTarget = dist / fireControls.GetLaserVelocity();

        return timeToTarget;
    }

    //Call when new target
    public void SetTarget(GameObject obj)
    {
        if (obj.GetComponent<Rigidbody>())

            target = obj.GetComponent<Rigidbody>() ;
    }
}

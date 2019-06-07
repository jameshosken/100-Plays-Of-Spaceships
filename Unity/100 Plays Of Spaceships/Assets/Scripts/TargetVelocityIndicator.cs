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

                //Use relative vel
                estimatedTarget.position = target.transform.position + (target.velocity - player.velocity) * GetLaserFlightTime();

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

    float GetLaserFlightTime()
    {
        //Distance to trgt
        distToTarget = Vector3.Distance(player.transform.position, target.transform.position);

        text.text = Mathf.Round(distToTarget).ToString() ;

        //Laser travels distance in:
        float timeToTarget = distToTarget / fireControls.GetLaserVelocity();

        return timeToTarget;
    }

    //Call when new target
    public void SetTarget(GameObject obj)
    {
        if (obj.GetComponent<Rigidbody>())

            target = obj.GetComponent<Rigidbody>() ;
    }
}

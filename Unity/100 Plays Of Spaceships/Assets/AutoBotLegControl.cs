using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoBotLegControl : MonoBehaviour
{

    //Rest points should be as close to the floor as possible
    [SerializeField] Transform[] legRestPoints;
    [SerializeField] Transform[] targets;

    [SerializeField] float footMoveRadius = 1f;
    [SerializeField] float distanceAboveGround = 1f;
    [SerializeField] float stability = 0.5f;

    Vector3[] footNormals;
    // Start is called before the first frame update
    void Start()
    {
        footNormals = new Vector3[targets.Length];

        for (int i = 0; i < targets.Length; i++)
        {
            footNormals[i] = Vector3.up;

            targets[i].position = legRestPoints[i].position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Evaluate distance between foot and restpoint
        for (int i = 0; i < targets.Length; i++)
        {
            float dist = Vector3.Distance(targets[i].position, legRestPoints[i].position);

            if(dist > footMoveRadius)
            {
                StartCoroutine(MoveFootToRestPosition(targets[i], legRestPoints[i], i));
            }
        }

        //Orient robot based on footNormals;

        Vector3 averageUp = Vector3.zero;

        for (int i = 0; i < footNormals.Length; i++)
        {
            averageUp += footNormals[i];
        }
        averageUp = averageUp.normalized;

        Ray ray = new Ray(transform.position, transform.up * -1f);
        RaycastHit hit;
        Vector3 bodyUp = Vector3.up;
        Vector3 hitPosition = transform.position;
        if (Physics.Raycast(ray, out hit))
        {
            bodyUp = hit.normal;
            hitPosition = hit.point;
        }


        transform.position = Vector3.Lerp(transform.position, hitPosition + transform.up * distanceAboveGround, 0.1f);


        Vector3 desiredVec = Vector3.Cross(bodyUp.normalized, transform.up);

        transform.Rotate(desiredVec * -1 * stability * Time.deltaTime);

        //transform.up = Vector3.RotateTowards(transform.up, bodyUp, .1f, .1f);

        //averageUp = averageUp.normalized;
        ////Determine angle we're rotated by from world basis
        //float angle = Vector3.Angle(transform.up, bodyUp);

        ////Determine rotation from world UP to normal UP
        //Quaternion upRotation = Quaternion.FromToRotation(Vector3.up, bodyUp);

        ////Fix rotation to normal UP
        //transform.rotation = upRotation;

        ////Rotate by original angle from basis
        //transform.Rotate(Vector3.up, angle, Space.Self);
    }

    IEnumerator MoveFootToRestPosition(Transform target, Transform restPoint, int i)
    {

        Vector3 newRestPoint = GetFootPositionOnGround(restPoint, i);

        //For now snap to point
        target.position = newRestPoint;

        yield return null;
    }


    Vector3 GetFootPositionOnGround(Transform restPoint, int i)
    {
        Ray ray = new Ray(restPoint.position + restPoint.up*10f, restPoint.up * -1f); // move up then cast down

        RaycastHit hit;

        Vector3 newRestPoint = restPoint.position;
        if (Physics.Raycast(ray, out hit, 100f))
        {
            newRestPoint = hit.point;
            footNormals[i] += hit.normal;
        }
        return newRestPoint;
    }
}

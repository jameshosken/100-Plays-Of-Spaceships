using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RaycastWithLightBounce : MonoBehaviour
{

    [SerializeField] int numberOfBounces = 10;

    LineRenderer lineRenderer;

    List<Vector3> points = new List<Vector3>();

    float pRefraction = 1;
    float nRefraction = 1;

    // Start is called before the first frame update
    void Start()
    {
        //Make sure we can hit both sides of colliders
        //Must (also?) change in project settings
        Physics.queriesHitBackfaces = true;

        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        points.Clear();

        points.Add(transform.position);

        pRefraction = 1;
        nRefraction = 1;

        RaycastFromPoint(transform.position, transform.right, numberOfBounces, null);

        lineRenderer.positionCount = points.Count;

        for (int i = 0; i < points.Count; i++)
        {
            lineRenderer.SetPosition(i, points[i]);
        }


    }


    void RaycastFromPoint(Vector3 position, Vector3 direction, int n, GameObject prevHitObject)
    {
        //Debug.Log(n);
        
        if(n <= 0)
        {
            return;
        }

        else
        {
            RaycastHit hit;
            Ray ray = new Ray(position, direction);

            if (Physics.Raycast(ray, out hit))
            {
                // Find the line from the origin to the hit.
                Vector3 incomingVec = hit.point - position;

                Vector3 rotationAxis = Vector3.Cross(direction, hit.normal);

                float theta = Vector3.Angle(-direction, hit.normal); // AKA angle of incidence

                //Fully reflective:     N.rotate(theta)
                //Fully Transparent:    N.rotate(180 - theta)
                // Refractive:          N.rotate(r + 180 - theta)
                //float refraction = 1;
                

                bool exit = false;

                //Default case for air;
                nRefraction = 1;

                if (hit.collider.GetComponent<RaycastWithLightBounceMaterial>())
                {
                    //returns number from -1 to 1
                    
                    //if (prevHitObject)
                    //{
                    //    pRefraction = prevHitObject.GetComponent<RaycastWithLightBounceMaterial>().GetRefraction();
                    //}

                    if (prevHitObject == hit.collider.gameObject)
                    {
                        exit = true;
                    }
                    else
                    {
                        nRefraction = hit.collider.GetComponent<RaycastWithLightBounceMaterial>().GetRefraction();
                    }

                }

                //Quaternion rotation = Quaternion.AngleAxis(refraction, rotationAxis);

                //Vector3 newDirection = rotation * hit.normal;

                //Draw new ray from hit point
                //Rays had a tendency to re-intersect the same collider so
                //I've offset the new position slightly

                Vector3 surfNorm = hit.normal;
                if (exit)
                {
                    surfNorm = -hit.normal;
                }

                Vector3 newDirection = Refract(pRefraction, nRefraction, surfNorm, direction);
                pRefraction = nRefraction;

                Debug.DrawRay(hit.point, surfNorm, Color.yellow);

                points.Add(hit.point);
                RaycastFromPoint(hit.point+newDirection*0.1f, newDirection, n - 1, hit.collider.gameObject);

                
                // Draw lines to show the incoming "beam" and the reflection.
                
            }
            else
            {
                // If no collider, add a point arbitrarily far away and end loop
                points.Add(position + direction * 500);
                return;
            }
        }
    }

    //After a number of failed attempts, this reddit user saved my life: https://pastebin.com/10pVRz5R

    /**
  * returns:
  *  normalized Vector3 refracted by passing from one medium to another in a realistic manner according to Snell's Law
  *
  * parameters:
  *  RI1 - the refractive index of the first medium
  *  RI2 - the refractive index of the second medium
  *  surfNorm - the normal of the interface between the two mediums (for example the normal returned by a raycast)
  *  incident - the incoming Vector3 to be refracted
  *
  * usage example (laser pointed from a medium with RI roughly equal to air through a medium with RI roughly equal to water):
  *  Vector3 laserRefracted = Refract(1.0f, 1.33f, waterPointNorm, laserForward);
*/
    public static Vector3 Refract(float RI1, float RI2, Vector3 surfNorm, Vector3 incident)
    {
        surfNorm.Normalize(); //should already be normalized, but normalize just to be sure
        incident.Normalize();

        return (RI1 / RI2 * Vector3.Cross(surfNorm, Vector3.Cross(-surfNorm, incident)) - surfNorm * Mathf.Sqrt(1 - Vector3.Dot(Vector3.Cross(surfNorm, incident) * (RI1 / RI2 * RI1 / RI2), Vector3.Cross(surfNorm, incident)))).normalized;
    }
}

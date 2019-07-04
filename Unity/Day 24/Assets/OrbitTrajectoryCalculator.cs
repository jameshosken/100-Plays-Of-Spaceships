using System;
using UnityEngine;

public class OrbitTrajectoryCalculator : MonoBehaviour
{
    /// <summary>
    /// 
    ///  r = (l^2) / (m^2 * u) * 1 / ( 1 + e * cos (theta) );
    ///  
    ///  r = distance, 
    ///  theta = angle from periapsis
    ///  l = angular momentum = m * r^2 * theta(dot) - theta(dot) is angular velocity 
    ///  u is parameter for which u/r^2 = accel. u = -GM
    ///  
    ///  e is eccentricity of orbit: 
    ///  e = sqrt(1 + (2 E l^2)/(m^3 * u^2)
    /// 
    /// </summary>

    //Alternative brute force method

    /// <summary>
    /// 
    /// 
    /// next position = current position + current velocity + force
    /// </summary>
    /// 

    [SerializeField] int predictions = 10;
    [SerializeField] float predictionStep = 1f;

    [SerializeField] Transform apoapsis;
    [SerializeField] Transform periapsis;

    [SerializeField] bool showApsis = false;


    LineRenderer lineRenderer;
    Rigidbody body;
    GravitationalAttraction grav;


    // Start is called before the first frame update
    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        body = GetComponent<Rigidbody>();
        grav = GetComponent<GravitationalAttraction>();

        lineRenderer.positionCount = predictions;
    }

    private void FixedUpdate()
    {
        if (Time.frameCount % 3 == 0)
        {
            PredictTrajectory();
        }
    }

    private void PredictTrajectory()
    {

        Vector3 predictedPosition = transform.position;
        Vector3 predictedVelocity = body.velocity;

        Vector3 maxPos = Vector3.zero;
        Vector3 minPos = Vector3.zero;
        Vector3 attractor = grav.GetClosestAttractor(transform.position);

        double maxDistance = double.MinValue;
        double minDistance = double.MaxValue;

        for (int i = predictions * 1 / 4 ; i < predictions; i++)
        {
            Vector3 predictedforce = grav.GetForceAtPosition(predictedPosition);

            predictedVelocity = predictedVelocity + predictedforce * predictionStep;
            predictedPosition = predictedPosition + predictedVelocity  * predictionStep;


            double dist = Vector3.Distance(predictedPosition, attractor);

            if (dist > maxDistance) {
                maxDistance = dist;
                maxPos = predictedPosition;
                
            }else if(dist < minDistance)
            {
                minDistance = dist;
                minPos = predictedPosition;
            }

            lineRenderer.SetPosition( i, predictedPosition);
        }

        predictedPosition = transform.position;
        predictedVelocity = -body.velocity;

        for (int i = (predictions*1/4)-1; i >= 0; i--)
        {
            Vector3 predictedforce = grav.GetForceAtPosition(predictedPosition);

            predictedVelocity = predictedVelocity + predictedforce * predictionStep;
            predictedPosition = predictedPosition + predictedVelocity * predictionStep;

            double dist = Vector3.Distance(predictedPosition, attractor);

            if (dist > maxDistance)
            {
                maxDistance = dist;
                maxPos = predictedPosition;

            }
            else if (dist < minDistance)
            {
                minDistance = dist;
                minPos = predictedPosition;
            }

            lineRenderer.SetPosition(i, predictedPosition);
        }


        if (showApsis)
        {
            apoapsis.position = Vector3.Lerp(apoapsis.position, maxPos, 0.5f);
            periapsis.position = Vector3.Lerp(periapsis.position, minPos, 0.5f);
        }
        
    }
}

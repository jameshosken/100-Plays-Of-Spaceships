using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Based on https://www.alanzucconi.com/2017/04/17/procedural-animations/
/// </summary>
public class IKSolver : MonoBehaviour
{
    [SerializeField] IKRobotJoint[] joints;
    [SerializeField] float samplingDistance = 0.1f;
    [SerializeField] float learningRate = 1f;
    [SerializeField] float distanceThreshold = 0.1f;

    [SerializeField] Transform target;


    int maxIKIterations = 500;

    private void Update()
    {
        float[] angles = GetAngles();

        InverseKinematicsInstant(target.transform.position, angles);

        //InverseKinematics(target.transform.position, angles);

        DrawDebug();
    }

    private void InverseKinematicsInstant(Vector3 target, float[] angles)
    {
        //bool targetReached = false;
        int count = 0;

        //while (DistanceFromTarget(target, angles) > distanceThreshold && count < maxIKIterations )
        //{
        for (int c = 0; c < maxIKIterations; c++)
        {
            //count++;
            if (DistanceFromTarget(target, angles) < distanceThreshold)
                break;

            for (int i = joints.Length - 1; i >= 0; i--)
            {

                float gradient = PartialGradient(target, angles, i);
                angles[i] -= learningRate * gradient;

                angles[i] = Mathf.Clamp(angles[i], joints[i].MinAngle, joints[i].MaxAngle);

                if (DistanceFromTarget(target, angles) < distanceThreshold)
                    break;
            }
        }


        if (DistanceFromTarget(target, angles) < distanceThreshold)
        {
            for (int i = joints.Length - 1; i >= 0; i--)
            {
                joints[i].RotateJoint(angles[i]);
            }
        }
    }

    void DrawDebug()
    {
        for (int i = 1; i < joints.Length; i++)
        {
            Color col = new Color(1, 1 - (float)i / (float)joints.Length, (float)i / (float)joints.Length);
            Debug.DrawLine(joints[i - 1].transform.position, joints[i].transform.position, col);
        }

        Debug.DrawLine(joints[joints.Length - 1].transform.position, target.position, Color.green);
    }


    private float[] GetAngles()
    {
        float[] angles = new float[joints.Length];
        for (int i = 0; i < joints.Length; i++)
        {
            angles[i] = joints[i].transform.localRotation.eulerAngles.magnitude;
        }
        return angles;
    }

    public void InverseKinematics(Vector3 target, float[] angles)
    {
        if (DistanceFromTarget(target, angles) < distanceThreshold)
            return;

        for (int i = joints.Length - 1; i >= 0; i--)
        {
            // Gradient descent
            // Update : Solution -= LearningRate * Gradient
            float gradient = PartialGradient(target, angles, i);
            angles[i] -= learningRate * gradient;


            joints[i].RotateJoint(angles[i]);

            if (DistanceFromTarget(target, angles) < distanceThreshold)
                return;
        }
    }


    public float PartialGradient(Vector3 target, float[] angles, int i)
    {
        // Saves the angle,
        // it will be restored later
        float angle = angles[i];

        // Gradient : [F(x+SamplingDistance) - F(x)] / h
        float f_x = DistanceFromTarget(target, angles);

        angles[i] += samplingDistance;
        float f_x_plus_d = DistanceFromTarget(target, angles);

        float gradient = (f_x_plus_d - f_x) / samplingDistance;

        // Restores
        angles[i] = angle;

        return gradient;
    }

    public Vector3 ForwardKinematics(float[] angles)
    {
        Vector3 prevPoint = joints[0].transform.position;
        Quaternion rotation = Quaternion.identity;
        for (int i = 1; i < joints.Length; i++)
        {
            // Rotates around a new axis
            rotation *= Quaternion.AngleAxis(angles[i - 1], joints[i - 1].axis);
            Vector3 nextPoint = prevPoint + rotation * joints[i].startOffset;

            prevPoint = nextPoint;
        }
        return prevPoint;
    }
    public float DistanceFromTarget(Vector3 target, float[] angles)
    {
        Vector3 point = ForwardKinematics(angles);
        return Vector3.Distance(point, target);
    }
}


using UnityEngine;

public class IKRobotJoint : MonoBehaviour
{
    public Vector3 axis;
    public Vector3 startOffset;

    public float MinAngle;
    public float MaxAngle;

    void Awake()
    {
        startOffset = transform.localPosition;
    }

    public void RotateJoint(float angle)
    {
        transform.localEulerAngles = axis * angle;
    }
}


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverbikeController : MonoBehaviour
{

    [SerializeField] Transform frontEngine;
    [SerializeField] Transform rearEngine;
    [SerializeField] Transform leftEngine;
    [SerializeField] Transform rightEngine;

    [SerializeField] float hoverDistance = 2f;
    [SerializeField] float hoverPower = 1f;
    [SerializeField] AnimationCurve hoverCurve;

    [SerializeField] float thrust = 1f;
    [SerializeField] float turn = 1f;
    [SerializeField] float leanAmount = 0.5f;
    [SerializeField] float turnThrust = .2f;
    [SerializeField] float stability = .5f;
    [SerializeField] int stabilitySmooth = 100;
    Rigidbody body;

    List<Vector3> normals = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();   
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        Vector3[] front = HandleEngine(frontEngine);
        Vector3[] rear = HandleEngine(rearEngine);

        //Vector3 left = HandleEngine(leftEngine, stability);
        //Vector3 right= HandleEngine(rightEngine, stability);

        HandleStabilise(front, rear);

        HandleInput();

    }

    private void HandleStabilise(Vector3[] front, Vector3[] rear)
    {

        if(front[1] == Vector3.zero || rear[1] == Vector3.zero)
        {
            return;
        }



        Vector3 normal = front[1] + rear[1];


        normals.Add(normal);
        if (normals.Count > stabilitySmooth)
        {
            normals.RemoveAt(0);
        }


        Vector3 aggregate = Vector3.zero;
        for (int i = 0; i < normals.Count; i++)
        {
            aggregate += normals[i];
        }

        Debug.DrawRay(transform.position, aggregate, Color.green);

        //transform.position = Vector3.Lerp(transform.position, floatPoint, stability);
        Quaternion desiredRotation = Quaternion.FromToRotation(transform.up, aggregate.normalized);

        //Quaternion upRotation = Quaternion.FromToRotation(transform.up, Vector3.up);

        //transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, stability);


        Vector3 desiredVec = Vector3.Cross(aggregate.normalized, transform.up);



        //desiredVec += upVec;

        body.AddTorque((desiredVec * -1 * stability) - body.angularVelocity);

        Vector3 upVec = Vector3.Cross(Vector3.up, transform.up);

        body.AddTorque((upVec * -1 * stability) - body.angularVelocity);

        //desiredRotation = desiredRotation * transform.rotation;

        //transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, stability);

        //upRotation = upRotation * transform.rotation;

        //transform.rotation = Quaternion.Lerp(transform.rotation, upRotation, stability);

    }

    private void HandleInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            body.AddForce(transform.forward * thrust);
        }

        if (Input.GetKey(KeyCode.S))
        {
            body.AddForce(transform.forward * -1 * thrust);
        }

        if (Input.GetKey(KeyCode.A))
        {
            body.AddTorque(transform.up * -1 * turn);
            body.AddTorque(transform.forward  * leanAmount);
            body.AddForce(transform.right * -1 * turnThrust);
            //transform.Rotate(transform.up * turn);
        }

        if (Input.GetKey(KeyCode.D))
        {
            body.AddTorque(transform.up * turn);
            body.AddTorque(transform.forward * -1 * leanAmount);
            body.AddForce(transform.right * turnThrust);
            //transform.Rotate(-transform.up * turn);
        }

        if (Input.GetKey(KeyCode.X))
        {
            body.velocity *= 0.95f;
            body.angularVelocity *= 0.95f;
        }
    }

    private Vector3[] HandleEngine(Transform engine)
    {
        Ray ray = new Ray(engine.position, engine.up * -1);

        RaycastHit hit;

        Vector3 hitpoint = engine.position + engine.up * -hoverDistance;
        Vector3 normal = engine.up;

        if (Physics.Raycast(ray, out hit, 100f))
        {

            hitpoint = hit.point;

            normal = hit.normal;

            float dist = Vector3.Distance(engine.position, hitpoint);

            if (dist < hoverDistance)
            {
                Vector3 force = normal.normalized * hoverPower * hoverCurve.Evaluate(Mathf.Clamp01(dist / hoverDistance));
                //Vector3 force = normal.normalized * hoverPower * multiplier;


                body.AddForceAtPosition(force, engine.position, ForceMode.Force);
                Debug.DrawRay(engine.position, force, Color.red, 1f);
            }
            else
            {
                Vector3 force = normal.normalized * hoverPower * (1 - (dist /  hoverDistance)) * .1f;

                body.AddForceAtPosition(force, engine.position, ForceMode.Force);
                Debug.DrawRay(engine.position, force, Color.red, 1f);
            }
        }

        return new Vector3[] { hitpoint, normal };
    }

}

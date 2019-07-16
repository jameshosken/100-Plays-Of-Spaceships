using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBoid : MonoBehaviour
{

    [SerializeField] Transform detectionSphere;
    [SerializeField] float rotationSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float maxSpeed;
    [SerializeField] float detectionRadius;

    Rigidbody body;

    [SerializeField] Transform target;

    Renderer[] renderers;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        target = GameObject.Find("EndGoal").transform;

    }

    public void ApplyDNASequence(SimpleDNASequence genes)
    {
        
        this.detectionRadius = genes.detectionRadius;
        this.rotationSpeed = genes.rotationSpeed;
        this.acceleration = genes.acceleration;
        this.maxSpeed = genes.maxSpeed;

        detectionSphere.localScale = Vector3.one * detectionRadius;
        //this.transform.localScale = Vector3.one * maxSpeed / 100f;

        renderers = GetComponentsInChildren<Renderer>();


        //for (int i = 0; i < renderers.Length; i++)
        //{
        //    Material mat = renderers[i].materials[i];

        //}
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        SeekTarget(target.position);
        LimitVelocity();

        Physics.IgnoreLayerCollision(gameObject.layer, gameObject.layer);
    }

    private void SeekTarget(Vector3 targetPosition)
    {
        //Seek target, velocity dependent
        Vector3 lookVector = (targetPosition - body.velocity) - transform.position;

        lookVector = lookVector.normalized;

        body.AddForce(lookVector * acceleration * Time.deltaTime);

        return;

    }

    private void LimitVelocity()
    {
        if (body.velocity.magnitude > maxSpeed)
        {
            body.velocity = body.velocity.normalized * maxSpeed;
        }
    }

    void AvoidTarget(Vector3 targetPosition)
    {

        targetPosition.y = transform.position.y;

        float dist = Vector3.Distance(targetPosition, transform.position);

        float multiplier = detectionRadius / (dist + 0.00001f);


        //Avoid specific location, velocity agnostic
        Vector3 lookVector = transform.position - targetPosition;

        lookVector = lookVector.normalized;

        body.AddForce(lookVector * acceleration * multiplier * Time.deltaTime);

    }

    private void OnTriggerStay(Collider other)
    {
        Vector3 point = other.ClosestPoint(transform.position);
        AvoidTarget(point);
    }
}

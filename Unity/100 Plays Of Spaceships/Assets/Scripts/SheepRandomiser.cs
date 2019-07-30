using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepRandomiser : MonoBehaviour
{
    [SerializeField] float bodyMinOffset = 0.8f;
    [SerializeField] float bodyMaxOffset = 1.2f;
    [SerializeField] float rotationOffsetmax;

    [SerializeField] Transform body;
    [SerializeField] Transform head;
    [SerializeField] Transform tail;
    [SerializeField] Transform leftEar;
    [SerializeField] Transform rightEar;


    // Start is called before the first frame update
    void Start()
    {
        Vector3 bodyOffset = new Vector3(
            Random.Range(bodyMinOffset, bodyMaxOffset) * body.localScale.x,
            Random.Range(bodyMinOffset, bodyMaxOffset) * body.localScale.y,
            Random.Range(bodyMinOffset, bodyMaxOffset) * body.localScale.z);

        Vector3 headOffset = new Vector3(
            Random.Range(bodyMinOffset, bodyMaxOffset) * head.localScale.x,
            Random.Range(bodyMinOffset, bodyMaxOffset) * head.localScale.y,
            Random.Range(bodyMinOffset, bodyMaxOffset) * head.localScale.z);

        Vector3 tailOffset = new Vector3(
            Random.Range(bodyMinOffset, bodyMaxOffset) * tail.localScale.x,
            Random.Range(bodyMinOffset, bodyMaxOffset) * tail.localScale.y,
            Random.Range(bodyMinOffset, bodyMaxOffset) * tail.localScale.z);

        float leftEarRot = Random.Range(-rotationOffsetmax, rotationOffsetmax);
        float rightEarRot = Random.Range(-rotationOffsetmax, rotationOffsetmax);


        body.localScale =  bodyOffset;
        head.localScale = headOffset;
        tail.localScale = tailOffset;

        leftEar.Rotate(Vector3.forward, leftEarRot, Space.Self);
        rightEar.Rotate(Vector3.forward, rightEarRot, Space.Self);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpToRotation : MonoBehaviour
{

    [SerializeField] Transform target;
    [SerializeField] float speed;
    // Start is called before the first frame update

    // Update is called once per frame
    void LateUpdate()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, speed * Time.deltaTime);
    }
}

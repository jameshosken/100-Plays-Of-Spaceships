using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LookAtConstrained : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] Vector3 localRotationAxis;
    // Start is called before the first frame update

    Transform parent;
    void Start()
    {
        parent = transform.parent;
    }


    // Update is called once per frame
    void Update()
    {


        
        transform.rotation = Quaternion.Euler(parent.rotation.x * localRotationAxis.x * Mathf.Rad2Deg,
            parent.rotation.y * localRotationAxis.y * Mathf.Rad2Deg,
            parent.rotation.z * localRotationAxis.z * Mathf.Rad2Deg);
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTarget : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField] Vector3 maxVel;
    [SerializeField] Vector3 maxSpin;
    [SerializeField] Mesh[] meshes;

    // Start is called before the first frame update
    void Start()
    {

        int rand = Random.Range(0, meshes.Length);
        Mesh mesh = meshes[rand];
        GetComponent<MeshFilter>().mesh = mesh;
        rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(Random.Range(-maxVel.x, maxVel.x), Random.Range(-maxVel.y, maxVel.y), Random.Range(-maxVel.z, maxVel.z));
        rb.AddTorque(new Vector3(Random.Range(-maxSpin.x, maxSpin.x), Random.Range(-maxSpin.y, maxSpin.y), Random.Range(-maxSpin.z, maxSpin.z)) );

        transform.localScale = transform.localScale * Random.Range(0.5f, 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

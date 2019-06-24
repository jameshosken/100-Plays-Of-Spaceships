using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LimitHeight : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float maxHeight = 50;

    [SerializeField] float force = 1f;
    [SerializeField] Text warning;
    [SerializeField] Color warningColour;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (transform.position.y > maxHeight)
        {
            rb.velocity *= .99f;
            rb.AddForce(Vector3.up * -force);

            warning.color = Color.Lerp(warning.color, warningColour, 0.05f);
        }
        else
        {
            warning.color = Color.Lerp(warning.color, Color.clear, 0.05f);
        }

    }
}

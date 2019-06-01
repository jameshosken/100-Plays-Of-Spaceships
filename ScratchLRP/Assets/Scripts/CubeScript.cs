using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeScript : MonoBehaviour
{
    GameObject thisObject;
    // Start is called before the first frame update
    void Start()
    {
        thisObject = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        thisObject.transform.Rotate(new Vector3(0, 1, 1f) * Time.deltaTime);
    }
}

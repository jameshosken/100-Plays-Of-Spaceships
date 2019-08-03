using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleportHandler : MonoBehaviour
{
    [SerializeField] Transform connectedPortal;
    
    // Start is called before the first frame update
    

    Vector3 forwardDirection;

    bool isOpen = true;

    void Start()
    {
        forwardDirection = transform.forward* - 1; // Change Amount to jump forward by
    }

    private void OnTriggerEnter(Collider other)
    {
        print("HIT");
        

        Vector3 portalToObject = other.transform.position - transform.position;

        float angleToObject = Vector3.Dot(forwardDirection, portalToObject);

        print(angleToObject);
        if(angleToObject > 0)
        {
            print("Translating: " + (transform.position - connectedPortal.position).ToString());

            if (other.GetComponent<UnityTemplateProjects.SimpleCameraController>())
            {
                other.GetComponent<UnityTemplateProjects.SimpleCameraController>().enabled = false;
            }

            Vector3 objectToPortalCenter = transform.position - other.transform.position;

            objectToPortalCenter.y = 0; //Only flipped on X and Z

            other.transform.Translate(objectToPortalCenter * 2f + forwardDirection, Space.World);

            other.transform.Translate((transform.position - connectedPortal.position) * -1f, Space.World);

            other.transform.Rotate(Vector3.up * 180f, Space.World);

            if (other.GetComponent<UnityTemplateProjects.SimpleCameraController>())
            {
                other.GetComponent<UnityTemplateProjects.SimpleCameraController>().enabled = true;
            }

        }


    }

}

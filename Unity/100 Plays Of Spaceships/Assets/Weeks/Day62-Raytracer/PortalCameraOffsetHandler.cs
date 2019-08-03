using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCameraOffsetHandler : MonoBehaviour
{

    [SerializeField] Transform myPortal;
    [SerializeField] Transform connectedPortal;

    Camera mainCam;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 mainCamPositionOffset = GetMainCameraPositionRelativeToOther();

        mainCamPositionOffset.x *= -1; // Flip it
        mainCamPositionOffset.z *= -1; // Flip it

        transform.position = mainCamPositionOffset + transform.parent.position;

        Quaternion mainCamRotationOffset = GetMainCameraRotationRelativeToOther();

        transform.localRotation = mainCamRotationOffset;

    }

    Vector3 GetMainCameraPositionRelativeToOther()
    {
        Vector3 offset = mainCam.transform.position - connectedPortal.transform.position;

        return offset;
    }

    Quaternion GetMainCameraRotationRelativeToOther()
    {
        //Quaternion rotation = Quaternion.FromToRotation(connectedPortal.transform.eulerAngles, mainCam.transform.eulerAngles);
        Quaternion rotation = Quaternion.Inverse(connectedPortal.transform.rotation) * mainCam.transform.rotation;
        return rotation;
    }

}

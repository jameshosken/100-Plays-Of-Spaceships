using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireControlHUD : MonoBehaviour
{
    [SerializeField] Transform fixedReticule;
    [SerializeField] Transform inertiaReticule;
    [SerializeField] Camera cam;

    [SerializeField] RectTransform fixedImage;
    [SerializeField] RectTransform inertiaImage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 fixedPos = cam.WorldToScreenPoint(fixedReticule.position);
        fixedImage.transform.position = fixedPos;

        Vector3 aimPos = cam.WorldToScreenPoint(inertiaReticule.position);
        inertiaImage.transform.position = aimPos;

    }
}

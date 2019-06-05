using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MouseAxisUIOffset : MonoBehaviour
{

    [SerializeField] Rigidbody rb;
    [SerializeField] RectTransform xy;
    [SerializeField] RectTransform z;
    [SerializeField] RectTransform aim;
    [SerializeField] RectTransform aim1;
    [SerializeField] RectTransform aim2;



    private void Start()
    {
        
    }
    // Update is called once per frame
    void FixedUpdate()
    {

        //float x = Input.GetAxis("Mouse X");
        //float y = Input.GetAxis("Mouse Y");
       
        Vector3 inertia = rb.velocity;
        Vector3 localangularvelocity = rb.transform.InverseTransformDirection(rb.angularVelocity);
        Vector3 position = new Vector3(localangularvelocity.y * 10, localangularvelocity.x * -10, 0);


        Vector3 aim1position = new Vector3(localangularvelocity.y * -75, localangularvelocity.x * 75, 0);
        Vector3 aimposition = new Vector3(localangularvelocity.y * -100, localangularvelocity.x * 100, 0);
        Vector3 aim2position = new Vector3(localangularvelocity.y * -125, localangularvelocity.x * 125, 0);

        //aim1.parent.GetComponent<RectTransform>().position = new Vector3(inertia.y * -10, inertia.x * 10, 0); ;
        //aim.parent.GetComponent<RectTransform>().position = new Vector3(inertia.y * -10, inertia.x * 10, 0); ;
        //aim2.parent.GetComponent<RectTransform>().position = new Vector3(inertia.y * -10, inertia.x * 10, 0); ;

        aim1.parent.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, localangularvelocity.z * -3);
        aim.parent.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, localangularvelocity.z * -5);
        aim2.parent.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, localangularvelocity.z * -7);

        aim.transform.localPosition = Vector3.Lerp(aim.transform.localPosition, aimposition, 0.5f) ;
        aim1.transform.localPosition = Vector3.Lerp(aim1.transform.localPosition, aim1position, 0.5f);
        aim2.transform.localPosition = Vector3.Lerp(aim.transform.localPosition, aim2position, 0.5f);



        xy.transform.localPosition = position;
        z.transform.rotation = Quaternion.Euler(0, 0, localangularvelocity.z * -5);
    }
}

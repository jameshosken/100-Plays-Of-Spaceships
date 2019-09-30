using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicPaintBrush : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        
        
    }



    private void OnCollisionStay(Collision collision)
    {
        GameObject other = collision.gameObject;

        if (other.GetComponent<DynamicPaintCanvas>())
        {
            DynamicPaintCanvas canvas = other.GetComponent<DynamicPaintCanvas>();

            RaycastHit hit = new RaycastHit();
            Ray ray = new Ray(collision.contacts[0].point - collision.contacts[0].normal, collision.contacts[0].normal);

            Vector2 pixelUV = Vector3.zero;

            if (Physics.Raycast(ray, out hit))
            {
                pixelUV = new Vector2(hit.textureCoord.x, hit.textureCoord.y);
            }

            print(pixelUV);

            canvas.PaintOnUVPosition(pixelUV.x, pixelUV.y);

            //uvWorldPosition.x = pixelUV.x;
            //uvWorldPosition.y = pixelUV.y;
            //uvWorldPosition.z = 0.0f;


        }



    }


}


//Ray ray = new Ray(transform.position, collision.GetContact(0).point - transform.position);
//RaycastHit hit;
//Physics.Raycast(ray, out hit, 10f){
//    }


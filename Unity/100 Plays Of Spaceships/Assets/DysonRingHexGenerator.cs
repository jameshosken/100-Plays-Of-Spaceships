using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DysonRingHexGenerator : MonoBehaviour
{
    [SerializeField] GameObject hexTile;
    
    [SerializeField] float radius = 1;

    [SerializeField] float angleIncrement = 1;

    [SerializeField] int cylinderHeight = 6;

    int steps;

    // Start is called before the first frame update
    void Start()
    {
        steps = (int)(360f / angleIncrement);

        StartCoroutine(GenerateTilesInCircle());
        //GenerateTilesInCircle();
    }

    IEnumerator GenerateTilesInCircle()
    {

        //tileW must == cosine law

        float tileW = Mathf.Sqrt(Sq(radius) + Sq(radius) - 2f * radius * radius * Mathf.Cos(Mathf.Deg2Rad*angleIncrement));

        //float tileW = tileSize * Mathf.Sqrt(3);
        float tileSize = tileW / Mathf.Sqrt(3);
        float tileH = tileSize * 2f;

        

        for (int h = -cylinderHeight / 2; h < cylinderHeight / 2; h++)
        {


            float angleOffset;

            angleOffset = angleIncrement / 2 * (h % 2);

            for (float i = 0; i < 360; i += angleIncrement)
            {

                float angle = i + angleOffset;

                float x = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
                float y = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;

                GameObject tile = Instantiate(hexTile) as GameObject;

                float zpos =  h * tileH * 3 / 4 ;
                tile.transform.position = new Vector3(x, zpos, y);
                tile.transform.localScale = Vector3.one * tileSize;

                tile.transform.Rotate(Vector3.right * -90);
                tile.transform.Rotate(Vector3.forward * 90);

                tile.transform.Rotate(0, 0, -angle);

                tile.GetComponent<HexTileBase>().SetCoords((int)angle, h);

                if(i % 6 == 0)
                {
                    yield return null;
                }
            }
            

        }
        yield return null;
    }

    float Sq(float a)
    {
        return (Mathf.Pow(a, 2));
    }
}

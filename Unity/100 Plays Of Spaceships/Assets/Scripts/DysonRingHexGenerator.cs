using System.Collections;
using UnityEngine;

public class DysonRingHexGenerator : MonoBehaviour
{
    [SerializeField] private GameObject hexTile;

    [SerializeField] private float radius = 1;

    [SerializeField] private float angleIncrement = 1;

    [SerializeField] private int cylinderHeight = 6;

    [SerializeField] private int spawnsPerFrame = 12;
    private int steps;
    private GameObject[] tiles;

    // Start is called before the first frame update
    private void Start()
    {

        steps = (int)(360f / angleIncrement);
        tiles = new GameObject[steps * cylinderHeight];

        StartCoroutine(GenerateTilesInCircle());
        //GenerateTilesInCircle();
    }

    private IEnumerator GenerateTilesInCircle()
    {

        //tileW must == cosine law

        float tileW = Mathf.Sqrt(Sq(radius) + Sq(radius) - 2f * radius * radius * Mathf.Cos(Mathf.Deg2Rad * angleIncrement));

        //float tileW = tileSize * Mathf.Sqrt(3);
        float tileSize = tileW / Mathf.Sqrt(3);
        float tileH = tileSize * 2f;


        int c = 0;
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

                tile.transform.parent = transform;

                float zpos = h * tileH * 3 / 4;
                tile.transform.position = new Vector3(x, zpos, y);
                tile.transform.localScale = Vector3.one * tileSize;

                tile.transform.Rotate(Vector3.right * -90);
                tile.transform.Rotate(Vector3.forward * 90);

                tile.transform.Rotate(0, 0, -angle);

                tile.GetComponent<HexTileBase>().SetCoords((int)angle, h);

                tiles[c] = tile;
                tile.SetActive(false);
                c++;

                if (i % spawnsPerFrame == 0)
                {
                    yield return null;
                }
            }
        }

        // Somewhere else
        // objectList is a List<GameObject>

        // We don't want to mess up the original list, copy it
        GameObject[] copyArray = new GameObject[tiles.Length];
        System.Array.Copy(tiles, copyArray, tiles.Length);

        ShuffleArray(copyArray);  // Call this more to shuffle again

        c = 0;
        foreach (GameObject obj in copyArray)
        {
            // This will be a random order of objectList


            obj.SetActive(true);

            if(c % spawnsPerFrame == 0)
            {
                yield return null;
            }
            c++;
        }

        yield return null;
    }

    private float Sq(float a)
    {
        return (Mathf.Pow(a, 2));
    }

    private void ShuffleArray<T>(T[] array)
    {
        int n = array.Length;
        for (int i = 0; i < n; i++)
        {
            // Pick a new index higher than current for each item in the array
            int r = i + Random.Range(0, n - i);

            // Swap item into new spot
            T t = array[r];
            array[r] = array[i];
            array[i] = t;
        }
    }


}

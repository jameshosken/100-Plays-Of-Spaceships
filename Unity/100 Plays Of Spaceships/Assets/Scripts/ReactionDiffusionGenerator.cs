using System;
using UnityEngine;

public class ReactionDiffusionGenerator : MonoBehaviour
{
    Renderer rendererer;
    Material mat;

    [SerializeField] private int size = 500;

    private float[][][] grid;
    private float[][][] next;
    [Range(0,1)]
    [SerializeField] private float dA = 1;
    [Range(0, 1)]
    [SerializeField] private float dB = 0.5f;
    [Range(0, 1)]
    [SerializeField] private float feed = 0.055f;
    [Range(0, 1)]
    [SerializeField] private float k = 0.062f;

    [SerializeField] float addThreshold = 0.1f;

    [ColorUsage(true, true)]
    [SerializeField] Color colourA;
    [ColorUsage(true, true)]
    [SerializeField] Color colourB;
    [SerializeField] bool emissive;

    
    Texture2D tex;

    Camera cam;


    void Start()
    {
        print("START");
        rendererer = GetComponent<Renderer>();
        mat = rendererer.materials[0];
        tex = new Texture2D(size, size);
        rendererer.transform.localScale = new Vector3(tex.width, 1, tex.height);

        print("Renderer");

        grid = new float[size][][];

        next = new float[size][][];

        print("Init");

        for (int x = 0; x < size; x++)
        {
            //print(x.ToString());

            grid[x] = new float[size][];

            next[x] = new float[size][];

            for (int y = 0; y < size; y++)
            {
                //print(y.ToString());
                grid[x][y] = new float[2];
                next[x][y] = new float[2];

                grid[x][y][0] = 1f;
                grid[x][y][1] = 0f;

                next[x][y][0] = 1f;
                next[x][y][1] = 0f;

                //print(grid[x][y][0].ToString() + "," + grid[x][y][1].ToString());
            };
            print(x);

        }


        AddFeedAtCenter();
    }

    void AddRandomFeed()
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if(UnityEngine.Random.Range(0, 100) < addThreshold)
                {
                    grid[i][j][1] = 1;
                }
            }
        }
    }

    void AddFeedAtCenter()
    {
        for (int i = size / 2-5; i < size / 2 + 5; i++)
        {
            for (int j = size / 2-5; j < size / 2 + 5; j++)
            {
                grid[i][j][1] = 1;
            }
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AddRandomFeed();
        }

        for (int x = 1; x < size - 1; x++)
        {
            for (int y = 1; y < size - 1; y++)
            {

                float a = grid[x][y][0];

                float b = grid[x][y][1];

                next[x][y][0] = a +

                  (dA * LaplaceA(x, y)) -

                  (a * b * b) +

                  (feed * (1 - a));

                next[x][y][1] = b +

                  (dB * LaplaceB(x, y)) +

                  (a * b * b) -

                  ((k + feed) * b);



                next[x][y][0] = Constrain(next[x][y][0], 0, 1);

                next[x][y][1] = Constrain(next[x][y][1], 0, 1);

            }

        }





        float[,] map = new float[size, size];
        for (var x = 0; x < size; x++)
        {

            for (var y = 0; y < size; y++)
            {

                var pix = (x + y * size) * 4;

                var a = next[x][y][0];

                var b = next[x][y][1];

                float c = Mathf.Floor((a - b) * 255f);

                c = Constrain(c, 0, 255);

                map[x, y] = c;

            }

        }

        tex = TextureGenerator.TextureFromHeightMapWithColour(map, colourA, colourB);

        if (emissive)
        {
            mat.SetTexture("_EmissionMap", tex);
        }
        else
        {
            mat.SetTexture("_BaseMap", tex);
        }
        
        Swap();



    }

    

    float Constrain(float value, float min, float max)
    {

        if(value < min)
        {
            value = min;
        }
        else if(value > max)
        {
            value = max;
        }
        return value;
    }

    private float LaplaceA(int x, int y)
    {

        float sumA = 0;

        sumA += grid[x][y][0] * -1f;

        sumA += grid[x - 1][y][0] * 0.2f;

        sumA += grid[x + 1][y][0] * 0.2f;

        sumA += grid[x][y + 1][0] * 0.2f;

        sumA += grid[x][y - 1][0] * 0.2f;

        sumA += grid[x - 1][y - 1][0] * 0.05f;

        sumA += grid[x + 1][y - 1][0]  * 0.05f;

        sumA += grid[x + 1][y + 1][0] * 0.05f;

        sumA += grid[x - 1][y + 1][0] * 0.05f;

        return sumA;

    }

    private float LaplaceB(int x,int y)
    {

        float sumB = 0;

        sumB += grid[x][y][1] * -1;

        sumB += grid[x - 1][y][1] * 0.2f;

        sumB += grid[x + 1][y][1] * 0.2f;

        sumB += grid[x][y + 1][1] * 0.2f;

        sumB += grid[x][y - 1][1] * 0.2f;

        sumB += grid[x - 1][y - 1][1] * 0.05f;

        sumB += grid[x + 1][y - 1][1] * 0.05f;

        sumB += grid[x + 1][y + 1][1] * 0.05f;

        sumB += grid[x - 1][y + 1][1] * 0.05f;

        return sumB;

    }

    private void Swap()
    {

        var temp = grid;

        grid = next;

        next = temp;

    }
}

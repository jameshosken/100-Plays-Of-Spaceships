using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineEquations : MonoBehaviour
{

    enum Function { SIN, COS, TAN, ATAN, NOISE};

    [SerializeField] Function function;
    [SerializeField] float xMin;
    [SerializeField] float xMax;
    [SerializeField] float xIncrement;
    [SerializeField] float xScale = 1f;
    [SerializeField] float yScale = 0.1f;

    [SerializeField] float exponent = 1f;

    LineRenderer lineRenderer;

    public float offset = 0;
    public float offsetSpeed = 0.1f;

    List<Vector3> points = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        points.Clear();
        
        for (float x = xMin; x < xMax; x+=xIncrement)
        {

            float xOff = (x + offset) * xScale;

            float fX;

            switch (function)
            {
                case( Function.SIN):
                    fX = Sine(xOff);
                    break;
                case (Function.COS):
                    fX = Cosine(xOff);
                    break;
                case (Function.TAN):
                    fX = Tan(xOff);
                    break;
                case (Function.ATAN):
                    fX = Noise(xOff);
                    break;
                case (Function.NOISE):
                    fX = Noise(xOff);
                    break;
                default:
                    fX = xOff;
                    break;
            }


            fX *= yScale;

            Vector3 newPoint = new Vector3(x, fX, 0) + transform.position;
            points.Add(newPoint);
        }

        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());


        offset += offsetSpeed * Time.deltaTime;
    }


    float Sine(float x)
    {

        return Mathf.Sin(x);
    }

    float Cosine(float x)
    {

        return Mathf.Cos(x);
    }

    float Tan(float x)
    {

        return Mathf.Tan(x);
    }

    float Noise(float x)
    {

        return Mathf.PerlinNoise(x,0);
    }


    private void OnValidate()
    {
        if(xIncrement < 0.1f)
        {
            xIncrement = 0.1f;
        }


    }
}

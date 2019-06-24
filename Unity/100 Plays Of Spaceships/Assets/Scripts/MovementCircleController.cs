using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MovementCircleController : MonoBehaviour
{
    [SerializeField] float thetaScale = 0.01f;
    [SerializeField] float radius = 3f;
    [SerializeField] MovementCircleYController lineController;

    MovementCircleYController yLineController;
    MovementCircleYController centerLineController;

    private int size;
    private LineRenderer line;
    private LineRenderer centerLine;
    private float theta = 0f;
    

    void Start()
    {
        yLineController = Instantiate(lineController);
        yLineController.transform.parent = transform;
        yLineController.gameObject.name = "Y LINE";

        centerLineController = Instantiate(lineController);
        centerLineController.transform.parent = transform;
        centerLineController.gameObject.name = "CENTER LINE";

        line = GetComponent<LineRenderer>();

       

    }

    void Update()
    {
        theta = 0f;
        size = (int)((1f / thetaScale) + 1f);
        line.positionCount = size;

        for (int i = 0; i < size; i++)
        {
            theta += (2.0f * Mathf.PI * thetaScale);
            float x = radius * Mathf.Cos(theta);
            float y = radius * Mathf.Sin(theta);
            line.SetPosition(i, new Vector3(x, 0, y) + transform.position);
        }
    }

    public void SetRadius(float r)
    {
        radius = r;
    }

    public void SetYLine(Vector3 start, Vector3 end)
    {
        yLineController.SetLine(start, end);
    }


    public void SetCenterLine(Vector3 start, Vector3 end)
    {
        centerLineController.SetLine(start, end);
    }


}

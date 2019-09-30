using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodracerEngineGlowLines : MonoBehaviour
{

    [SerializeField] Transform leftEngine;
    [SerializeField] Transform rightEngine;

    LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (leftEngine != null && rightEngine != null)
        {
            lineRenderer.SetPositions(new Vector3[] { leftEngine.position, rightEngine.position });
        }
    }

    public void Break()
    {
        lineRenderer.enabled = false;
    }
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>
/// BG Plane set to 'Clickable' layer,
/// Nodes set to 'Selectable' layer
/// </summary>
/// 
public class ConnectTheDotsController : MonoBehaviour
{

    [SerializeField] private GameObject edgeTemplate;

    [ColorUsage(true, true)]
    [SerializeField] Color lineColor;

    private List<EdgeController> edges = new List<EdgeController>();

    private enum State { None, Selected }
    private Camera cam;
    private State state = State.None;
    private LineRenderer lineRenderer;
    private GameObject selected = null;

    // Start is called before the first frame update
    private void Start()
    {
        cam = Camera.main;
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            HandleMouseDown();
        }

        if (Input.GetMouseButtonUp(0))
        {
            HandleMouseRelease();
        }

        HandleRaycast();

        DrawLine();
    }

    /// <summary>
    /// LINE FUNCTIONS
    /// </summary>

    private void DrawLine()
    {
        if (state == State.Selected)
        {
            lineRenderer.SetPosition(0, selected.transform.position);

            Vector3 mouse = GetRaycastPoint();
            lineRenderer.SetPosition(1, mouse);

            GameObject hitObj = HandleRaycast();

            if(hitObj.tag == "Selectable")
            {
                lineRenderer.materials[0].SetColor("_Color", lineColor);
            }
            else
            {
                lineRenderer.materials[0].SetColor("_Color", Color.white);
            }
        }
    }

    private void AddNewEdge(GameObject start, GameObject end)
    {

        bool isUnique = true;
        for (int i = 0; i < edges.Count; i++)
        {
            //Check if any prev edge contains these nodes:
            GameObject[] edgePoints = edges[i].GetPoints();
            if (edgePoints.Contains<GameObject>(start) && edgePoints.Contains<GameObject>(end))
            {
                isUnique = false;
            }
        }

        if (!isUnique)
        {
            print("Already exists!");
            return;
        }

        GameObject edge = Instantiate(edgeTemplate);
        edge.transform.parent = transform.parent;

        LineRenderer edgeLine = edge.GetComponent<LineRenderer>();

        edgeLine.SetPosition(0, start.transform.position);
        edgeLine.SetPosition(1, end.transform.position);

        //Handle creating collider

        EdgeController edgeController = edge.GetComponent<EdgeController>();
        edgeController.SetEdge(start, end);

        edges.Add(edgeController);

    }



    /// <summary>
    /// MOUSE FUNCTIONS
    /// </summary>

    private void HandleMouseDown()
    {
        GameObject clicked = HandleRaycast();

        if (clicked)
        {
            //Something was clicked!
            //Figure out what:
            switch (clicked.tag)
            {
                case "Clickable":
                    //Nothing useful;
                    ClearSelection();
                    break;
                case "Selectable":
                    //Clicked a node!
                    state = State.Selected;
                    selected = clicked;
                    lineRenderer.enabled = true;
                    break;
                case "Shootable":
                    //Clicked an Edge! Remove edge
                    if (clicked.GetComponent<EdgeController>())
                    {
                        print("REMOVING");
                        edges.Remove(clicked.GetComponent<EdgeController>());
                        GameObject.Destroy(clicked);
                    }

                    break;
                default:
                    break;
            }

        }
        else
        {
            //Remove selection if no objects hit (edge case)
            ClearSelection();
        }
    }

    private void HandleMouseRelease()
    {
        if (state == State.Selected)
        {
            //Check whether on new node or not:
            GameObject released = HandleRaycast();

            if (released)
            {
                if (released != selected && released.tag == "Selectable")
                {
                    print("Released: Drop a new target!");
                    AddNewEdge(selected, released);
                    //Drop a new edge!
                }
            }
        }
        state = State.None;
        ClearSelection();
    }

    /// <summary>
    /// RAYCAST FUNCTIONS
    /// </summary>
    /// 
    private Vector3 GetRaycastPoint()
    {
        Vector3 payload = Vector3.zero;

        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 200))
        {
            payload = hit.point;
        };

        return payload;
    }

    private GameObject HandleRaycast()
    {
        //Only return selectable objects

        GameObject payload = null;

        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 200))
        {
            if (hit.collider.gameObject.tag == "Selectable")
            {
                //Node Case
                payload = hit.collider.gameObject;
            }

            if (hit.collider.gameObject.tag == "Shootable")
            {
                //Node Case
                payload = hit.collider.gameObject;
            }

            if (hit.collider.gameObject.tag == "Shootable")
            {
                //Node Case
                payload = hit.collider.gameObject;
            }
        };

        return payload;
    }

    /// <summary>
    ///  ULTILITY
    /// </summary>

    private void ClearSelection()
    {
        lineRenderer.enabled = false;
    }

}



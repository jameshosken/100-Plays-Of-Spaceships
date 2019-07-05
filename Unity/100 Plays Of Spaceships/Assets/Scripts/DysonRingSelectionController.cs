using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DysonRingSelectionController : MonoBehaviour
{

    [SerializeField] float moveThreshhold = 1f;
    [SerializeField] Text coordsText;

    HexTileSelection hovered = null;
    Vector3 pMouse = Vector3.zero;

    bool selecting = false;

    List<HexTileSelection> selectedTiles = new List<HexTileSelection>();
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        selecting = Input.GetMouseButton(0);
        
        if (CheckMouseMoved() || Time.frameCount%5 == 0 || Input.GetMouseButtonDown(0))
        {
            RayCastFromMouse();
        }

    }

    private void RayCastFromMouse()
    {

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            print("HIT");
            if (hit.collider.GetComponentInParent<HexTileSelection>())
            {
                HexTileSelection newTile = hit.collider.GetComponentInParent<HexTileSelection>();

                if (hovered && hovered != newTile)
                {
                    hovered.SetHover(false);
                }
                
                

                if (!newTile.isHovered)
                {
                    newTile.SetHover(true);

                    int[] coords = newTile.GetComponent<HexTileBase>().GetCoords();
                    coordsText.text = coords[0].ToString() + "," + coords[1].ToString();

                    coordsText.rectTransform.position = RectTransformUtility.WorldToScreenPoint(cam, newTile.transform.position);


                    
                }

                hovered = newTile;

                if (selecting && hovered.isSelected == false)
                {
                    
                    hovered.SetSelection(true);
                    selectedTiles.Add(hovered);
                    hovered = null;
                }
            }
        }

        else
        {
            coordsText.text = "";
            if (selecting)
            {
                ClearSelection();
            }
        }
            

    }

    private bool CheckMouseMoved()
    {

        if (Vector3.Distance(Input.mousePosition, pMouse) > moveThreshhold)
        {
            pMouse = Input.mousePosition;
            return true;
        }
        return false;
    }

    public void ClearSelection()
    {
        for (int i = 0; i < selectedTiles.Count; i++)
        {
            selectedTiles[i].SetSelection(false);

        }

        selectedTiles.Clear();
    }
}

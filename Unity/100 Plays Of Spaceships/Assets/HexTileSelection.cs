using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTileSelection : MonoBehaviour
{
    [SerializeField] GameObject selectionIndicator;
    [ColorUsage(true, true)]
    [SerializeField] Color selectionColour;

    public bool isHovered = false;
    public bool isSelected = false;

    Renderer selectionRenderer;
    Color hoverColour;

    // Start is called before the first frame update
    void Start()
    {
        

        selectionRenderer = selectionIndicator.GetComponent< Renderer > ();
        hoverColour = selectionRenderer.materials[0].GetColor("_EmissionColor");

        SetSelection(false);
    }

    public void SetHover(bool status)
    {
        if (!isSelected)
        {
            selectionIndicator.SetActive(status);
            selectionRenderer.materials[0].SetColor("_EmissionColor", hoverColour);
        }
        
        isHovered = status;
    }

    public void SetSelection(bool status)
    {
        selectionIndicator.SetActive(status);
        selectionRenderer.materials[0].SetColor("_EmissionColor", selectionColour); ;
        isSelected = status;
    }

}

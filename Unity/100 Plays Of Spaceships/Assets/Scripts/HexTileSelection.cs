using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTileSelection : MonoBehaviour
{
    [SerializeField] GameObject selectionIndicator;
    [ColorUsage(true, true)]
    [SerializeField] Color selectionColour;

    [SerializeField] GameObject demoCube;

    public bool isHovered = false;
    public bool isSelected = false;

    Renderer selectionRenderer;
    Color hoverColour;

    int fadeOut;

    // Start is called before the first frame update
    void Start()
    {
        fadeOut = Random.Range(10, 50);

        selectionRenderer = selectionIndicator.GetComponent< Renderer > ();
        hoverColour = selectionRenderer.materials[0].GetColor("_EmissionColor");

        SetSelection(false);

        StartCoroutine(FadeOutSideColour());

        

    }

    IEnumerator FadeOutSideColour()
    {
        selectionIndicator.SetActive(true);
        selectionRenderer.materials[0].SetColor("_EmissionColor", hoverColour);

        for (int i = 0; i < fadeOut; i++)
        {
            float amt = (float)i / (float)fadeOut;
            Color col = Color.Lerp(hoverColour, Color.clear, amt);
            selectionRenderer.materials[0].SetColor("_EmissionColor", col);
            yield return new WaitForSeconds(.01f);
        }

        selectionIndicator.SetActive(false);
        selectionRenderer.materials[0].SetColor("_EmissionColor", Color.black);


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

        if (status == true)
        {
            GameObject cln = Instantiate(demoCube) as GameObject;
            cln.transform.position = transform.position + transform.up * Random.Range(10, 50);
            cln.transform.Rotate(new Vector3(Random.Range(0, 90), Random.Range(0, 90), Random.Range(0, 90)));

        }
    }

}

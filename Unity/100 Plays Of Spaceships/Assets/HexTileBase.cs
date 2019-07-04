using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class HexTileBase : MonoBehaviour
{
    [SerializeField] bool hasText = false;
    [SerializeField] bool delayStart = false;
    public int xCoord;
    public int yCoord;

    // Start is called before the first frame update
    void Start()
    {
        if (delayStart)
        {
            Invoke("Activate", Random.Range(0, 0.2f));
            gameObject.SetActive(false);
        }
    }

    void Activate()
    {
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCoords(int x, int y)
    {
        xCoord = x;
        yCoord = y;
        if (hasText)
        {
            GetComponentInChildren<Text>().text = x.ToString() + ", " + y.ToString();
        }
    }

    public int[] GetCoords()
    {
        return new int[] { xCoord, yCoord };
    }

}

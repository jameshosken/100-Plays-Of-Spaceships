using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionHandler : MonoBehaviour
{
    [SerializeField] GameObject selectionIndicatorTemplate;

    GameObject selectionIndicator;
    // Start is called before the first frame update
    void Start()
    {
        selectionIndicator = Instantiate(selectionIndicatorTemplate) as GameObject;
        selectionIndicator.transform.parent = transform;
        selectionIndicator.transform.position = transform.position;
        selectionIndicator.SetActive(false);
    }

    private void Update()
    {
        selectionIndicator.transform.rotation = Quaternion.Euler(Vector3.left*90) ;
    }

    public void SetSelectionIndicator(bool status)
    {
        selectionIndicator.SetActive(status);
    }
}

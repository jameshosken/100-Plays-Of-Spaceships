using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeText : MonoBehaviour
{

    [SerializeField] string newText;
    // Start is called before the first frame update
    public void Change()
    {
        GetComponent<Text>().text = newText;
    }
}

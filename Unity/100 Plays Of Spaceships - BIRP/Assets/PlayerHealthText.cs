using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthText : MonoBehaviour
{


    public void UpdateHealthText(int i)
    {
        GetComponent<Text>().text = "Health: " + i.ToString();
    }
}

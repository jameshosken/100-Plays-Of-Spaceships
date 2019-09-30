using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearEquationRepeater : MonoBehaviour
{

    [SerializeField] GameObject lineTemplate;
    [SerializeField] int number;
    [SerializeField] Vector3 positionOffset;

    [SerializeField] float offset;
    [SerializeField] float offsetSpeed;

    [SerializeField] float offsetIncrement;
    [SerializeField] float offsetSpeedIncrement;


    List<LineEquations> lines = new List<LineEquations>();
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < number; i++)
        {
            GameObject clone = Instantiate(lineTemplate);
            clone.transform.position = positionOffset * i;
            lines.Add(clone.GetComponent<LineEquations>());
            lines[i].offset = offset *i;
            lines[i].offsetSpeed = offsetSpeed;
        }
    }

    private void OnValidate()
    {
        for (int i = 0; i < number; i++)
        {

            lines[i].gameObject.transform.position = positionOffset * i;
            lines[i].offset = offset + i * offsetIncrement;
            lines[i].offsetSpeed = offsetSpeed + i * offsetSpeedIncrement;
        }
    }


}

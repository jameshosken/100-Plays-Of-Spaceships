using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField] float selfDestructTime;
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Destroy(gameObject, selfDestructTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipX : MonoBehaviour
{
    Transform target;
    Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
        target = transform.root;    
    }

    void Update()
    {
        if(target.localScale.x < 0)
        {
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, 1);
        }
        else
        {
            transform.localScale = new Vector3(originalScale.x, originalScale.y, 1);
        }
    }
}

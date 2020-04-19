using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSorter : MonoBehaviour
{

    [SerializeField]
    int SortingBaseValue = 5000;
    [SerializeField]
    int OriginOffsetY;
    [SerializeField]
    bool RunOnce = false;
    // Start is called before the first frame update
    private Renderer SpriteRenderer;
    void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        SpriteRenderer.sortingOrder = (int)(SortingBaseValue - transform.position.y - OriginOffsetY);
        if (RunOnce)
            Destroy(this);
    }
}

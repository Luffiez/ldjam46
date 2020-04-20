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
        //Debug.Log(  gameObject + " " + (int)( SortingBaseValue - transform.position.y - OriginOffsetY) + "position:" + transform.position);
        SpriteRenderer.sortingOrder = 5000 + (int)Camera.main.WorldToScreenPoint(SpriteRenderer.bounds.min).y * -3;
        if (RunOnce)
            Destroy(this);
    }
}

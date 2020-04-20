using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCanSpriteSorter : MonoBehaviour
{
    [SerializeField]
    int SortingBaseValue = 5000;
    [SerializeField]
    SpriteRenderer PlayerSpriteRender;
    int OrderOffset = 1;
    // Start is called before the first frame update
    private Renderer SpriteRenderer;
    void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void RenderFront()
    {
        OrderOffset = 1;
    }

    public void RenderBack()
    {
        OrderOffset = -1;
    }

    private void LateUpdate()
    {
        SpriteRenderer.sortingOrder = PlayerSpriteRender.sortingOrder +OrderOffset;
    }
}

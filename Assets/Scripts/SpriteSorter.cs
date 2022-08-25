using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSorter : MonoBehaviour
{
    private int sortOrder;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private int offset;
    [SerializeField] private bool isMobile = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = (int)(transform.position.y * -100) + offset;
    }

    private void Update()
    {
        if (isMobile)
        {
            spriteRenderer.sortingOrder = (int)(transform.position.y * -100) + offset;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    private Item item;
    public Item thisItem { get { return item; } }

    public void SetItem(Item item)
    {
        this.item = item;
        GetComponent<SpriteRenderer>().sprite = item.icon;
    }

    private void OnEnable()
    {
        GameManager.instance.baitObjects.Add(transform);
    }

    private void OnDisable()
    {
        GameManager.instance.baitObjects.Remove(transform);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item 
{
    public string itemName;

    public int quantity;

    public Sprite icon;

    public int foodPoints = 10;

    public enum ItemType { FOOD, RESOURCE, ITEM }
    public ItemType itemType;

    public Item()
    {

    }

    public Item(string newName)
    {
        itemName = newName;
    }

    public Item(string newName, ItemType itemType, Sprite newIcon)
    {
        itemName = newName;
        this.itemType = itemType;
        icon = newIcon;
    }
}

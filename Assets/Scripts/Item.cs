using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Item 
{
    public string itemName;

    public int quantity;

    public Sprite icon;

    public int foodPoints = 10;

    public Item()
    {

    }

    public Item(string newName)
    {
        itemName = newName;
    }
}

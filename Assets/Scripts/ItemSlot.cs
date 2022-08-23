using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlot : MonoBehaviour
{
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemQuantityText;
    public Image itemIcon;

    private Item item;

    public void SetItem(Item _item)
    {
        item = _item;
        itemNameText.text = item.itemName;
        itemQuantityText.text = ""+item.quantity;
        itemIcon.sprite = item.icon;
    }
}

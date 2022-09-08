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

    public void SelectThisItem()
    {
        GameManager.instance.itemManager.SelectItem(item);
    }

    public void PlaceThisItem()
    {
        Dialog newDialog = new Dialog("Would you like to place a " + item.itemName + " on the ground as bait?", "", null, false, "Yes", "No", "", "");
        DialogManager.instance.ShowDialog(newDialog, () => {
            GameManager.instance.PlaceItem(item, (Vector2)GameManager.instance.Player.position + GameManager.instance.Player.GetComponent<PlayerControls>().FacingVector);
            item.quantity--;
        }, () => {

        });

    }
        
}

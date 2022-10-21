using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeSlot : MonoBehaviour
{

    private PurchaseableUpgrade upgrade;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI upgradeNameText;
    [SerializeField] private Image cost1Icon;
    [SerializeField] private Image cost2Icon;
    [SerializeField] private Image cost3Icon;
    [SerializeField] private TextMeshProUGUI cost1Text;
    [SerializeField] private TextMeshProUGUI cost2Text;
    [SerializeField] private TextMeshProUGUI cost3Text;

    public void SetUpgrade(PurchaseableUpgrade upgrade)
    {
        Image[] icons = new Image[3];
        TextMeshProUGUI[] costTexts = new TextMeshProUGUI[3];
        icons[0] = cost1Icon;
        icons[1] = cost2Icon;
        icons[2] = cost3Icon;
        costTexts[0] = cost1Text;
        costTexts[1] = cost2Text;
        costTexts[2] = cost3Text;
        this.upgrade = upgrade;
        int costIndex = 0;

        icon.sprite = upgrade.upgradeIcon;
        icon.enabled = true;
        upgradeNameText.text = upgrade.upgradeName;

        cost1Icon.enabled = true;
        cost2Icon.enabled = true;
        cost3Icon.enabled = true;

        if (upgrade.woodCost > 0)
        {
            icons[costIndex].sprite = ItemManager.wood.icon;
            costTexts[costIndex].text = "" + upgrade.woodCost;
            costIndex++;
        }
        if (upgrade.stoneCost > 0)
        {
            icons[costIndex].sprite = ItemManager.stone.icon;
            costTexts[costIndex].text = "" + upgrade.stoneCost;
            costIndex++;
        }
        if (upgrade.clayCost > 0)
        {
            icons[costIndex].sprite = ItemManager.clay.icon;
            costTexts[costIndex].text = "" + upgrade.clayCost;
            costIndex++;
        }
        if (upgrade.ironCost > 0)
        {
            icons[costIndex].sprite = ItemManager.iron.icon;
            costTexts[costIndex].text = "" + upgrade.ironCost;
            costIndex++;
        }
        if (upgrade.crystalCost > 0)
        {
            icons[costIndex].sprite = ItemManager.crystal.icon;
            costTexts[costIndex].text = "" + upgrade.crystalCost;
            costIndex++;
        }
        if (upgrade.coalCost > 0)
        {
            icons[costIndex].sprite = ItemManager.coal.icon;
            costTexts[costIndex].text = "" + upgrade.coalCost;
            costIndex++;
        }
        if(costIndex < 2)
        {
            cost2Icon.enabled = false;
            cost3Icon.enabled = false;
            cost2Text.text = "";
            cost3Text.text = "";
        }else if (costIndex < 3)
        {
            cost3Icon.enabled = false;
            cost3Text.text = "";
        }
    }

    public void AttemptPurchase()
    {
        GameManager.instance.HideUpgrades();
        if (upgrade.isActive)
        {
            DialogManager.instance.ShowSimpleDialog("Oh, didn't you already purchase the " + upgrade.upgradeName + "?");
        }
        else
        {
            DialogManager.instance.ShowSimpleDialog("Oh, the " + upgrade.upgradeName + " huh?  Good eye!");
            if (CanAfford())
            {
                Dialog confirmDialog = new Dialog("Yeah, this baby will increase the amount of food you can store, sound good?", "Shop Keeper", null, true, "Yes", "No", "", "");

                DialogManager.instance.ShowDialog(confirmDialog, () => {
                    DialogManager.instance.ShowSimpleDialog("Great! I should be able to whip this up for you by this time tomorrow.  Thanks bud!");
                    GameManager.instance.itemManager.PayUpgradeCost(upgrade);
                }, () => {
                    DialogManager.instance.ShowSimpleDialog("Ah, thought better of it?  No worries, let me know if you need anything else!");
                });
            }
            else
            {
                DialogManager.instance.ShowSimpleDialog("Oh shoot, don't have enough resources for it right now huh?  No worries, come back when you do!");
            }
        }
    }

    public void Deactivate()
    {
        cost1Icon.enabled = false;
        cost2Icon.enabled = false;
        cost3Icon.enabled = false;
        upgradeNameText.text = "";
        icon.enabled = false;
        cost1Text.text = "";
        cost2Text.text = "";
        cost3Text.text = "";
    }

    private bool CanAfford()
    { 
        return ItemManager.wood.quantity >= upgrade.woodCost &&
        ItemManager.stone.quantity >= upgrade.stoneCost &&
        ItemManager.clay.quantity >= upgrade.clayCost &&
        ItemManager.crystal.quantity >= upgrade.crystalCost &&
        ItemManager.iron.quantity >= upgrade.ironCost &&
        ItemManager.coal.quantity >= upgrade.coalCost;
    }

}

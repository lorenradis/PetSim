using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager
{

    private static UpgradeManager instance;
    public static UpgradeManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new UpgradeManager();
            }
            return instance;
        }
        set { }
    }

    public List<PurchaseableUpgrade> foodStorageUpgrades = new List<PurchaseableUpgrade>();
    public List<PurchaseableUpgrade> resourceStorageUpgrades = new List<PurchaseableUpgrade>();
    public List<PurchaseableUpgrade> homeUpgrades = new List<PurchaseableUpgrade>();

    public UpgradeManager()
    {

        PurchaseableUpgrade crateUpgrade = new PurchaseableUpgrade();
        crateUpgrade.upgradeName = "Crate";
        crateUpgrade.woodCost = 10;
        crateUpgrade.onUpgrade = () => {
            GameManager.instance.itemManager.maxFoods = 16;
            crateUpgrade.isActive = true;
        };

        PurchaseableUpgrade bigCrateUpgrade = new PurchaseableUpgrade();
        crateUpgrade.upgradeName = "Big Crate";
        crateUpgrade.woodCost = 50;
        crateUpgrade.onUpgrade = () => {
            GameManager.instance.itemManager.maxFoods = 24;
            bigCrateUpgrade.isActive = true;
        };

        foodStorageUpgrades.Add(crateUpgrade);
        foodStorageUpgrades.Add(bigCrateUpgrade);

    }
}


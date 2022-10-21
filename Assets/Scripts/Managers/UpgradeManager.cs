using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
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

    public List<PurchaseableUpgrade> foodStorageUpgrades;
    public List<PurchaseableUpgrade> resourceStorageUpgrades;
    public List<PurchaseableUpgrade> homeUpgrades;

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
        bigCrateUpgrade.upgradeName = "Big Crate";
        bigCrateUpgrade.woodCost = 50;
        bigCrateUpgrade.stoneCost = 10;
        bigCrateUpgrade.onUpgrade = () => {
            GameManager.instance.itemManager.maxFoods = 24;
            bigCrateUpgrade.isActive = true;
        };

        PurchaseableUpgrade iceboxUpgrade = new PurchaseableUpgrade();
        iceboxUpgrade.upgradeName = "Icebox";
        iceboxUpgrade.stoneCost = 60;
        iceboxUpgrade.clayCost = 20;
        iceboxUpgrade.ironCost = 10;
        iceboxUpgrade.onUpgrade = () => {
            GameManager.instance.itemManager.maxFoods = 32;
            iceboxUpgrade.isActive = true;
        };

        PurchaseableUpgrade bigIceboxUpgrade = new PurchaseableUpgrade();
        bigIceboxUpgrade.upgradeName = "Big Icebox";
        bigIceboxUpgrade.stoneCost = 100;
        bigIceboxUpgrade.clayCost = 40;
        bigIceboxUpgrade.coalCost = 20;
        bigIceboxUpgrade.onUpgrade = () => {
            GameManager.instance.itemManager.maxFoods = 64;
            bigIceboxUpgrade.isActive = true;
        };

        PurchaseableUpgrade walkinFreezerUpgrade = new PurchaseableUpgrade();
        walkinFreezerUpgrade.upgradeName = "Walk-In Freezer";
        walkinFreezerUpgrade.ironCost = 100;
        walkinFreezerUpgrade.coalCost = 100;
        walkinFreezerUpgrade.crystalCost = 10;
        walkinFreezerUpgrade.onUpgrade = () => {
            GameManager.instance.itemManager.maxFoods = 256;
            walkinFreezerUpgrade.isActive = true;
        };

        foodStorageUpgrades = new List<PurchaseableUpgrade>();
        resourceStorageUpgrades = new List<PurchaseableUpgrade>();
        homeUpgrades = new List<PurchaseableUpgrade>();

        foodStorageUpgrades.Add(crateUpgrade);
        foodStorageUpgrades.Add(bigCrateUpgrade);
        foodStorageUpgrades.Add(iceboxUpgrade);
        foodStorageUpgrades.Add(bigIceboxUpgrade);
        foodStorageUpgrades.Add(walkinFreezerUpgrade);

    }
}


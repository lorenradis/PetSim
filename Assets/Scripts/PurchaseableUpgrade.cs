using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseableUpgrade
{

    public string upgradeName;
    public Sprite upgradeIcon;
    public Sprite upgradeSprite;
    public int woodCost;
    public int stoneCost;
    public int clayCost;
    public int crystalCost;
    public int ironCost;
    public int coalCost;

    public bool isActive = false;

    public Action onUpgrade;

    public PurchaseableUpgrade() { }

}

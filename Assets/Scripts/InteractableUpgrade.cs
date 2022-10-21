using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableUpgrade : Interactable
{
    public override void OnInteract()
    {
        GameManager.instance.ShowFoodUpgrades();
    }
}

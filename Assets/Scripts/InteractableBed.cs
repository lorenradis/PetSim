using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBed : Interactable
{
    public Dialog sleepDialog = new Dialog("Would you like to get some rest?", "", null, false, "Yes", "No", "", "");

    public override void OnInteract()
    {
        base.OnInteract();
        DialogManager.instance.ShowDialog(sleepDialog, () => {
            DialogManager.instance.ShowSimpleDialog("Sleep well!");
            GameManager.instance.GoToSleep();
        }, () => {
            DialogManager.instance.ShowSimpleDialog("See you later, bed...");
        });
    }
}

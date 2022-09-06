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
            GameManager.instance.GoToSleep();
        }, () => {

        });
    }
}

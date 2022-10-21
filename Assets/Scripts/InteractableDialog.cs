using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDialog : Interactable
{
    public Dialog dialog;

    /*
     * we need dialogs to be able to hold full conversations (multiple options for dialog, multiple messages with responses 
     * we also need to be able to change dialogs based on story progression
     */

    public override void OnInteract()
    {
        DialogManager.instance.ShowDialog(dialog);
        base.OnInteract();
    }
}

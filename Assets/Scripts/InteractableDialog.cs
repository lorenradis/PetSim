using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDialog : Interactable
{
    public Dialog dialog;

    public override void OnInteract()
    {
        DialogManager.instance.ShowDialog(dialog);
        base.OnInteract();
    }
}

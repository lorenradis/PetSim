using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePet : Interactable
{
    private PetInfo petInfo;

    public Dialog interactDialog;

    private void Start()
    {
        interactDialog = new Dialog(
            "Your pet " + petInfo.petName + " looks happy!  What would you like to do?","", null, false, "Check", "Assign", "More", "Cancel");
    }

    public override void OnInteract()
    {
        base.OnInteract();

        DialogManager.instance.ShowDialog(interactDialog, () =>{
            GameManager.instance.ShowPetInfo(petInfo);
        }, () => {
            //bring up the assign task menu
        }, () => {
            //show whatever the hell we end up putting here
        }, () => {
            //hide the prompt    
        });
    }

    public void SetPetInfo(PetInfo newPet)
    {
        petInfo = newPet;
    }

}

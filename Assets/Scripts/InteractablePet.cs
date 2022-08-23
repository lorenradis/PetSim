using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePet : Interactable
{
    private PetInfo petInfo;

    public Dialog interactDialog;

    private void Start()
    {
    }

    public override void OnInteract()
    {
        base.OnInteract();

        GameManager.instance.petManager.SelectPet(petInfo);

        DialogManager.instance.ShowDialog(interactDialog, () =>{
            GameManager.instance.ShowPetInfo(petInfo);
        }, () => {
            //assign to a task (show the map first)
            GameManager.instance.ShowMapForAssignment();
        }, () => {
            //make the pet your companion
        }, () => {
            DialogManager.instance.ShowSimpleDialog("See ya later " + petInfo.petName + "!");
        });
    }

    public void SetPetInfo(PetInfo newPet)
    {
        petInfo = newPet;
    }

}

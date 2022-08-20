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

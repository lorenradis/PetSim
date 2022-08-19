using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePet : Interactable
{
    private PetInfo petInfo;

    public override void OnInteract()
    {
        base.OnInteract();

        GameManager.instance.ShowPetInfo(petInfo);
    }

    public void SetPetInfo(PetInfo newPet)
    {
        petInfo = newPet;
    }

}

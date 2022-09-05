using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePet : Interactable
{
    private PetInfo petInfo;

    public enum PetState { WILD, TAME, PARTNER }
    public PetState petState;

    private void Start()
    {
    }

    public override void OnInteract()
    {
        base.OnInteract();
        ShowInteractOptions();
    }

    public void ShowInteractOptions()
    {
        Dialog interactDialog;
        switch(petState)
        {
            case PetState.WILD:

                interactDialog = new Dialog("", petInfo.petName, null, false, "Check", "Give", "Invite", "Cancel");

                DialogManager.instance.ShowDialog(interactDialog, () => {
                    //if check
                    GameManager.instance.ShowPetInfo(petInfo);
                }, () => {
                    //if give
                    //show inventory, allow player to offer a food item or osmething to the pet to make it happy.
                }, () => {
                    //if invite
                    //randomly decide if the pet will join up or not, or is it based on some stat, is it based on what the farm's got going on?
                    DialogManager.instance.ShowSimpleDialog("You gently stretch your hand out towards the " + petInfo.petName + "...");
                    int roll = Random.Range(1, 100);
                    {
                        if(roll < 50)
                        {
                            DialogManager.instance.ShowSimpleDialog("...and they nuzzle your outstretched hand!  You befriended the " + petInfo.petName);
                            DialogManager.instance.ShowSimpleDialog("The " + petInfo.petName + " heads back to the farm.");
                            GameManager.instance.petManager.AddPetToList(petInfo);
                            Destroy(gameObject);
                        }
                        else
                        {
                            DialogManager.instance.ShowSimpleDialog("...but they back away nervously, they aren't ready to join you yet.");
                            GetComponent<OverworldPet>().StartFleeing(GameManager.instance.Player);
                            Destroy(gameObject, 10f);
                        }
                    }
                }, () => {
                    //if cancel
                    DialogManager.instance.ShowSimpleDialog("You give the " + petInfo.petName + " a little space as it watches you curiously.");
                });

                break;
            case PetState.TAME:
                GameManager.instance.petManager.SelectPet(petInfo);

                interactDialog = new Dialog("", petInfo.petName, null, false, "Check", "Assign", "Partner", "Cancel");

                DialogManager.instance.ShowDialog(interactDialog, () => {
                    GameManager.instance.ShowPetInfo(petInfo);
                }, () => {
                    //assign to a task (show the map first)
                    GameManager.instance.ShowMapForAssignment();
                }, () => {
                    if (GameManager.instance.petManager.SetPartnerPet(petInfo))
                    {
                        DialogManager.instance.ShowSimpleDialog("You invited " + petInfo.petName + " to join you on your adventure, welcome to the team " + petInfo.petName + "!");
                        GameManager.instance.SpawnPartnerPets();
                        Destroy(gameObject);
                    }
                }, () => {
                    DialogManager.instance.ShowSimpleDialog("See ya later " + petInfo.petName + "!");
                });
                break;
            case PetState.PARTNER:
                GameManager.instance.petManager.SelectPet(petInfo);

                interactDialog = new Dialog("", petInfo.petName, null, false, "Check", "Dismiss", "Cancel", "");

                DialogManager.instance.ShowDialog(interactDialog, () =>
                {
                    GameManager.instance.ShowPetInfo(petInfo);
                }, () =>
                {
                    GameManager.instance.petManager.DismissPartnerPet(petInfo);
                    DialogManager.instance.ShowSimpleDialog("You sent " + petInfo.petName + " back to the farm.  See you at home " + petInfo.petName + "!");
                    GameManager.instance.SpawnPartnerPets();
                }, () =>
                {
                    DialogManager.instance.ShowSimpleDialog("See ya later " + petInfo.petName + "!");
                });

                break;
            default:
                break;
        }
    }

    public void SetPetInfo(PetInfo newPet)
    {
        petInfo = newPet;
    }

}

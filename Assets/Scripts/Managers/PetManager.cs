using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class PetManager  {

/*
what is the pet manager responsible for?
keeping a list of all available pet templates, yeah?
templates are used for instantiating specific instances of pets
does pet manager also keep track of the player's roster of pets?
is each pet a petinfo specifically declared in the pet manager as a private or public instance of the class?
spawn regions will have a list of templates to spawn, right?
we'll have a dedicated scene for choosing your starter which will... i don't know.. something.
the player will be presented with a graphical representation of the 3 pets.
*/

    public List<PetInfo> petTemplates = new List<PetInfo>();

    public List<PetInfo> currentPets = new List<PetInfo>();

    public static PetInfo Bulbos;
    public static PetInfo Charby;
    public static PetInfo Squirt;
    public static PetInfo Stunky;

    private PetInfo selectedPet;
    public PetInfo SelectedPet { get { return selectedPet; } set { } }

    private PetInfo partnerPet1 = null;
    public PetInfo PartnerPet1
    {
        get
        {
            return partnerPet1;
        }
    }
    private PetInfo partnerPet2 = null;
    public PetInfo PartnerPet2
    {
        get
        {
            return partnerPet2;
        }
    }

    //overworld animator controllers
    [SerializeField] private RuntimeAnimatorController bulbosOverworldAnimator;
    [SerializeField] private RuntimeAnimatorController charbyOverworldAnimator;
    [SerializeField] private RuntimeAnimatorController squirtOverworldAnimator;
    [SerializeField] private RuntimeAnimatorController stunkyOverworldAnimator;

    public delegate void OnPartnerChanged();
    public static OnPartnerChanged onPartnerChangedCallback;

    public void SetupPets()
    {
        Bulbos = new PetInfo("Bulbos", 18, 12, 6, AffinityManager.grassAffinity, 
        "Strong and intelligent, but far from the fastest of beasts - Bulbos are most at home in the forests and other areas full of green growing things.", bulbosOverworldAnimator);
        Charby = new PetInfo("Charby", 12, 6, 18, AffinityManager.fireAffinity,
        "Fast and powerful, but not the brightest of bulbs - if you'll forgive the pun - Charby are most at home in sweltering temperatures and dry climes", charbyOverworldAnimator);
        Squirt = new PetInfo("Squirt", 6, 18, 12, AffinityManager.waterAffinity,
        "Cunning and quick, but a tad lacking in the brawn department - Squirt are most at home in lakes, rivers, oceans and wetlands of the world", squirtOverworldAnimator);

        Stunky = new PetInfo("Stunky", 12, 12, 12, AffinityManager.fireAffinity, "He's a real stinker!", stunkyOverworldAnimator);

        petTemplates.Add(Bulbos);
        petTemplates.Add(Charby);
        petTemplates.Add(Squirt);
        petTemplates.Add(Stunky);

        //for testing purposes only
        Task task = new Task();
        task.TaskName = "";
        PetInfo starter = new PetInfo(Bulbos.petName, Bulbos.Strength.BaseValue, Bulbos.Smarts.BaseValue, Bulbos.Speed.BaseValue, Bulbos.affinity, Bulbos.description, Bulbos.overworldAnimator);
        starter.currentTask = task;
        starter.overworldAnimator = bulbosOverworldAnimator;
        AddPetToList(starter);
        PetInfo starter2 = new PetInfo(Charby.petName, Charby.Strength.BaseValue, Charby.Smarts.BaseValue, Charby.Speed.BaseValue, Charby.affinity, Charby.description, Charby.overworldAnimator);
        starter2.currentTask = task;
        starter2.overworldAnimator = charbyOverworldAnimator;
        AddPetToList(starter2);
        PetInfo starter3 = new PetInfo(Squirt.petName, Squirt.Strength.BaseValue, Squirt.Smarts.BaseValue, Squirt.Speed.BaseValue, Squirt.affinity, Squirt.description, Squirt.overworldAnimator);
        starter3.currentTask = task;
        starter3.overworldAnimator = squirtOverworldAnimator;
        AddPetToList(starter3);
    }

    public void AddPetToList(PetInfo newPet)
    {
        currentPets.Add(newPet);
        GameClock.onMinuteChangedCallback += newPet.IncrementNeeds;
    }

    public void SelectPet(PetInfo petInfo)
    {
        selectedPet = petInfo;
    }

    public bool SetPartnerPet(PetInfo petInfo)
    {
        if(partnerPet1 != null)
        {
            if(partnerPet2 != null)
            {
                DialogManager.instance.ShowSimpleDialog("You already have the maximum number of pets, dismiss one or more before inviting someone else to join you.");
                return false;
            }
            else
            {
                partnerPet2 = petInfo;
                partnerPet2.SetPartner(true);
            }
        }
        else
        {
            partnerPet1 = petInfo;
            partnerPet1.SetPartner(true);
        }

        if(onPartnerChangedCallback != null)
        {
            onPartnerChangedCallback.Invoke();
        }
        return true;
    }

    public void DismissPartnerPet(PetInfo petInfo)
    {
        if(partnerPet1 == petInfo)
        {
            partnerPet1 = null;
            petInfo.SetPartner(false);
        }else if(partnerPet2 == petInfo)
        {
            partnerPet2 = null;
            petInfo.SetPartner(false);
        }
        else
        {
            Debug.Log("Unable to dismiss " + petInfo.petName + ", they do not appear to be a pet");
        }
        if(onPartnerChangedCallback != null)
        {
            onPartnerChangedCallback.Invoke();
        }
    }

}
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

    public int maxPets = 7;
    public int level2Max = 15;
    public int level3Max = 20;
    public int level4Max = 24;
    public int level5Max = 26;

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
    [SerializeField] private RuntimeAnimatorController bulbosBattleAnimator;
    [SerializeField] private Sprite bulbosIcon;
    [SerializeField] private Sprite bulbosPortrait;

    [SerializeField] private RuntimeAnimatorController charbyOverworldAnimator;
    [SerializeField] private RuntimeAnimatorController charbyBattleAnimator;
    [SerializeField] private Sprite charbyIcon;
    [SerializeField] private Sprite charbyPortrait;

    [SerializeField] private RuntimeAnimatorController squirtOverworldAnimator;
    [SerializeField] private RuntimeAnimatorController squirtBattleAnimator;
    [SerializeField] private Sprite squirtIcon;
    [SerializeField] private Sprite squirtPortrait;

    [SerializeField] private RuntimeAnimatorController stunkyOverworldAnimator;
    [SerializeField] private RuntimeAnimatorController stunkyBattleAnimator;
    [SerializeField] private Sprite stunkyIcon;
    [SerializeField] private Sprite stunkyPortrait;

    public delegate void OnPartnerChanged();
    public static OnPartnerChanged onPartnerChangedCallback;

    public void SetupPets()
    {
        Bulbos = new PetInfo("Bulbos", 18, 12, 6, 50, 50, AffinityManager.grassAffinity, 
        "Strong and intelligent, but far from the fastest of beasts - Bulbos are most at home in the forests and other areas full of green growing things.",
        bulbosOverworldAnimator, bulbosOverworldAnimator, bulbosIcon, bulbosPortrait, ItemManager.berries, ItemManager.mushrooms);

        Charby = new PetInfo("Charby", 12, 6, 18, 25, 75, AffinityManager.fireAffinity,
        "Fast and powerful, but not the brightest of bulbs - if you'll forgive the pun - Charby are most at home in sweltering temperatures and dry climes",
        charbyOverworldAnimator, charbyOverworldAnimator,charbyIcon, charbyPortrait, ItemManager.succulent, ItemManager.berries);

        Squirt = new PetInfo("Squirt", 6, 18, 12, 75, 25, AffinityManager.waterAffinity,
        "Cunning and quick, but a tad lacking in the brawn department - Squirt are most at home in lakes, rivers, oceans and wetlands of the world",
        squirtOverworldAnimator, squirtOverworldAnimator,squirtIcon, squirtPortrait, ItemManager.mushrooms, ItemManager.succulent);


        Stunky = new PetInfo("Stunky", 12, 12, 12, 25, 75, AffinityManager.fireAffinity, "He's a real stinker!",
            stunkyOverworldAnimator, stunkyOverworldAnimator, stunkyIcon, stunkyPortrait, ItemManager.mushrooms, ItemManager.succulent);

        petTemplates.Add(Bulbos);
        petTemplates.Add(Charby);
        petTemplates.Add(Squirt);
        petTemplates.Add(Stunky);

    }

    public bool AddPetToList(PetInfo newPet)
    {
        if (currentPets.Count < maxPets)
        {
            currentPets.Add(newPet);
            GameClock.onMinuteChangedCallback += newPet.IncrementNeeds;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SelectPet(PetInfo petInfo)
    {
        selectedPet = petInfo;
    }

    public PetInfo CopyPetFromTemplate(PetInfo pet)
    {
        PetInfo newPet = new PetInfo(pet.petName, pet.Strength.BaseValue, pet.Smarts.BaseValue, pet.Speed.BaseValue, pet.Anxiety, pet.Aggression, pet.affinity, pet.description, pet.overworldAnimator, pet.battleAnimator, pet.icon, pet.portrait,pet.lovedFood, pet.hatedFood);
        return newPet;
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

    public bool IncreasePetKnowledge(string petName, int amount)
    {
        foreach(PetInfo pet in petTemplates)
        {
            if(pet.petName == petName)
            {
                int level = pet.researchLevel;
                pet.GainResearchEXP(amount);
                if(pet.researchLevel > level)
                {
                    return true;
                }
                return false;
            }
        }
        Debug.Log("Pet not found");
        return false;
    }

    public string GetPetKnowledge(string petName)
    {
        foreach(PetInfo pet in petTemplates)
        {
            if(pet.petName == petName)
            {
                return pet.GetLevelKnowledge(pet.researchLevel);
            }
        }
        Debug.Log("Pet Not Found");
        return "";
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
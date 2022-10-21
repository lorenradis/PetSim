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
    public static PetInfo Bird;
    public static PetInfo EarthMole;
    public static PetInfo GrassOpossum;
    public static PetInfo WaterRaccoon;
    public static PetInfo Stunky;
    public static PetInfo Earthworm;
    public static PetInfo Owl;
    public static PetInfo Toadstool;
    public static PetInfo Rocky;
    public static PetInfo FireDog;
    public static PetInfo Gremlin;
    public static PetInfo Tumbleweed;
    public static PetInfo Shrub;
    public static PetInfo Penguin;
    public static PetInfo Crab;
    public static PetInfo Cloud;
    public static PetInfo Bug;
    public static PetInfo Geist;
    public static PetInfo Sprite;

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

    //Pet Graphics to be set in inspector
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

    [SerializeField] private RuntimeAnimatorController birdOverworldAnimator;
    [SerializeField] private RuntimeAnimatorController birdBattleAnimator;
    [SerializeField] private Sprite birdIcon;
    [SerializeField] private Sprite birdPortrait;

    [SerializeField] private RuntimeAnimatorController earthMoleOverworldAnimator;
    [SerializeField] private RuntimeAnimatorController earthMoleBattleAnimator;
    [SerializeField] private Sprite earthMoleIcon;
    [SerializeField] private Sprite earthMolePortrait;

    [SerializeField] private RuntimeAnimatorController grassOpossumOverworldAnimator;
    [SerializeField] private RuntimeAnimatorController grassOpossumBattleAnimator;
    [SerializeField] private Sprite grassOpossumIcon;
    [SerializeField] private Sprite grassOpossumPortrait;

    [SerializeField] private RuntimeAnimatorController waterRaccoonOverworldAnimator;
    [SerializeField] private RuntimeAnimatorController waterRaccoonBattleAnimator;
    [SerializeField] private Sprite waterRaccoonIcon;
    [SerializeField] private Sprite waterRaccoonPortrait;

    [SerializeField] private RuntimeAnimatorController stunkyOverworldAnimator;
    [SerializeField] private RuntimeAnimatorController stunkyBattleAnimator;
    [SerializeField] private Sprite stunkyIcon;
    [SerializeField] private Sprite stunkyPortrait;

    [SerializeField] private RuntimeAnimatorController earthwormOverworldAnimator;
    [SerializeField] private RuntimeAnimatorController earthwormBattleAnimator;
    [SerializeField] private Sprite earthwormIcon;
    [SerializeField] private Sprite earthwormPortrait;

    [SerializeField] private RuntimeAnimatorController owlOverworldAnimator;
    [SerializeField] private RuntimeAnimatorController owlBattleAnimator;
    [SerializeField] private Sprite owlIcon;
    [SerializeField] private Sprite owlPortrait;

    [SerializeField] private RuntimeAnimatorController toadstoolOverworldAnimator;
    [SerializeField] private RuntimeAnimatorController toadstoolBattleAnimator;
    [SerializeField] private Sprite toadstoolIcon;
    [SerializeField] private Sprite toadstoolPortrait;

    [SerializeField] private RuntimeAnimatorController rockyOverworldAnimator;
    [SerializeField] private RuntimeAnimatorController rockyBattleAnimator;
    [SerializeField] private Sprite rockyIcon;
    [SerializeField] private Sprite rockyPortrait;

    [SerializeField] private RuntimeAnimatorController fireDogOverworldAnimator;
    [SerializeField] private RuntimeAnimatorController fireDogBattleAnimator;
    [SerializeField] private Sprite fireDogIcon;
    [SerializeField] private Sprite fireDogPortrait;

    [SerializeField] private RuntimeAnimatorController gremlinOverworldAnimator;
    [SerializeField] private RuntimeAnimatorController gremlinBattleAnimator;
    [SerializeField] private Sprite gremlinIcon;
    [SerializeField] private Sprite gremlinPortrait;

    [SerializeField] private RuntimeAnimatorController tumbleweedOverworldAnimator;
    [SerializeField] private RuntimeAnimatorController tumbleweedBattleAnimator;
    [SerializeField] private Sprite tumbleweedIcon;
    [SerializeField] private Sprite tumbleweedPortrait;

    [SerializeField] private RuntimeAnimatorController shrubOverworldAnimator;
    [SerializeField] private RuntimeAnimatorController shrubBattleAnimator;
    [SerializeField] private Sprite shrubIcon;
    [SerializeField] private Sprite shrubPortrait;

    [SerializeField] private RuntimeAnimatorController penguinOverworldAnimator;
    [SerializeField] private RuntimeAnimatorController penguinBattleAnimator;
    [SerializeField] private Sprite penguinIcon;
    [SerializeField] private Sprite penguinPortrait;

    [SerializeField] private RuntimeAnimatorController crabOverworldAnimator;
    [SerializeField] private RuntimeAnimatorController crabBattleAnimator;
    [SerializeField] private Sprite crabIcon;
    [SerializeField] private Sprite crabPortrait;

    [SerializeField] private RuntimeAnimatorController cloudOverworldAnimator;
    [SerializeField] private RuntimeAnimatorController cloudBattleAnimator;
    [SerializeField] private Sprite cloudIcon;
    [SerializeField] private Sprite cloudPortrait;

    [SerializeField] private RuntimeAnimatorController bugOverworldAnimator;
    [SerializeField] private RuntimeAnimatorController bugBattleAnimator;
    [SerializeField] private Sprite bugIcon;
    [SerializeField] private Sprite bugPortrait;

    [SerializeField] private RuntimeAnimatorController geistOverworldAnimator;
    [SerializeField] private RuntimeAnimatorController geistBattleAnimator;
    [SerializeField] private Sprite geistIcon;
    [SerializeField] private Sprite geistPortrait;

    [SerializeField] private RuntimeAnimatorController spriteOverworldAnimator;
    [SerializeField] private RuntimeAnimatorController spriteBattleAnimator;
    [SerializeField] private Sprite spriteIcon;
    [SerializeField] private Sprite spritePortrait;

    public delegate void OnPartnerChanged();
    public static OnPartnerChanged onPartnerChangedCallback;

    public void SetupPets()
    {
        Bulbos = new PetInfo("Bulbos", 18, 12, 6, 50, 50, AffinityManager.grassAffinity, 
        "Strong and intelligent, but far from the fastest of beasts - Bulbos are most at home in the forests and other areas full of green growing things.",
        bulbosOverworldAnimator, bulbosOverworldAnimator, bulbosIcon, bulbosPortrait, ItemManager.grassFood, ItemManager.waterFood);

        Charby = new PetInfo("Charby", 12, 6, 18, 25, 75, AffinityManager.fireAffinity,
        "Fast and powerful, but not the brightest of bulbs - if you'll forgive the pun - Charby are most at home in sweltering temperatures and dry climes",
        charbyOverworldAnimator, charbyOverworldAnimator,charbyIcon, charbyPortrait, ItemManager.fireFood, ItemManager.grassFood);

        Squirt = new PetInfo("Squirt", 6, 18, 12, 75, 25, AffinityManager.waterAffinity,
        "Cunning and quick, but a tad lacking in the brawn department - Squirt are most at home in lakes, rivers, oceans and wetlands of the world",
        squirtOverworldAnimator, squirtOverworldAnimator,squirtIcon, squirtPortrait, ItemManager.waterFood, ItemManager.fireFood);


        Stunky = new PetInfo("Stunky", 12, 12, 12, 25, 75, AffinityManager.fireAffinity, "He's a real stinker!",
            stunkyOverworldAnimator, stunkyOverworldAnimator, stunkyIcon, stunkyPortrait, ItemManager.waterFood, ItemManager.fireFood);

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
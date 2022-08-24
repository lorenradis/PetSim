using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class Region
{

    /*
    every region will have a unique resource, food, and animal type
    regions have affinities, creatures with matching affinities do better on tasks  in these regions
    player can venture in to regions to hunt for new pets of the affinity of the region
    pets can be sent into regions to perform tasks (gather resources, forage for food, hunt animals)
    resources are brought back to camp and used to advance ()
    */

    public Affinity affinity;

    public string regionName;

    private List<PetInfo> availableSpawns = new List<PetInfo>();

    public Region() { }

    public bool isUnlocked = false;

    public Item resource;
    public Item food;

    private int experience;
    public int Experience { get { return experience; } }

    private int level;
    public int Level { get { return level; } }

    public int ToNextLevel { get { return level * level * level; } set { } }

    public List<LevelUnlock> levelUnlocks = new List<LevelUnlock>();

    public List<bool> levelSpecificObjects = new List<bool>();

    private List<PetInfo> commonSpawnsDay = new List<PetInfo>();
    private List<PetInfo> unCommonSpawnsDay = new List<PetInfo>();
    private List<PetInfo> rareSpawnsDay = new List<PetInfo>();
    private List<PetInfo> commonSpawnsNight = new List<PetInfo>();
    private List<PetInfo> unCommonSpawnsNight = new List<PetInfo>();
    private List<PetInfo> rareSpawnsNight = new List<PetInfo>();

    /*
     * 
     * regions will have level unlocks with actions.
     * they will also have a list of levelspecificgameobjects and bools - at level 3 this exit block gets disabled, at level 10 this secret entrance gets enabled, etc.
     * levelspecificobjects in a scene will have a set region and level, and when the scene loads they will poll the region manager to see if they're unlocked or not.
     */

    public Region(string newName, Affinity newAffinity, Item newResource, Item newFood)
    {
        regionName = newName;
        affinity = newAffinity;
        resource = newResource;
        food = newFood;
    }

    public bool IsObjectActive(int index)
    {
        return levelSpecificObjects[index];
    }

    public void AddLevelUnlock(LevelUnlock levelUnlock, bool startsActive)
    {
        levelUnlocks.Add(levelUnlock);
        levelSpecificObjects.Add(startsActive);
    }

    public void GainExperience(int amount)
    {
        experience += amount;
        if (experience >= ToNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        level++;
        if (experience >= ToNextLevel)
        {
            LevelUp();
        }
        CheckUnlocks();
    }

    private void CheckUnlocks()
    {
        for (int i = 0; i < levelUnlocks.Count; i++)
        {
            if(level >= levelUnlocks[i].levelToUnlock)
            {
                levelUnlocks[i].Unlock();
                DialogManager.instance.ShowSimpleDialog("You've made a new discovery in the " + regionName + " region, check it out when you have time!");
            }
        }
    }

    public void AddSpawn(PetInfo petInfo, bool nocturnal, int rarity)
    {
        switch(rarity)
        {
            case 0:
                if(nocturnal)
                {
                    commonSpawnsNight.Add(petInfo);
                }
                else
                {
                    commonSpawnsDay.Add(petInfo);
                }
                break;
            case 1:
                if (nocturnal)
                {
                    unCommonSpawnsNight.Add(petInfo);
                }
                else
                {
                    unCommonSpawnsDay.Add(petInfo);
                }
                break;
            case 2:
                if (nocturnal)
                {
                    rareSpawnsNight.Add(petInfo);
                }
                else
                {
                    rareSpawnsDay.Add(petInfo);
                }
                break;
            default:
                break;
        }
    }

    public PetInfo GetSpawn()
    {
        int hour = GameManager.instance.gameClock.Hour;

        bool night = true;
        if (hour > 5 && hour < 18) night = false;

        int commonChance = 100;
        int uncommonChance = 40;
        int rareChance = 10;

        int roll = UnityEngine.Random.Range(1, commonChance);

        if(roll < rareChance)
        {
            if(night)
            {
                return rareSpawnsNight[UnityEngine.Random.Range(0, rareSpawnsNight.Count)];
            }
            else
            {
                return rareSpawnsDay[UnityEngine.Random.Range(0, rareSpawnsDay.Count)];
            }
        }else if(roll < uncommonChance)
        {
            if (night)
            {
                return unCommonSpawnsNight[UnityEngine.Random.Range(0, unCommonSpawnsNight.Count)];
            }
            else
            {
                return unCommonSpawnsDay[UnityEngine.Random.Range(0, unCommonSpawnsDay.Count)];
            }
        }
        else
        {
            if (night)
            {
                return commonSpawnsNight[UnityEngine.Random.Range(0, commonSpawnsNight.Count)];
            }
            else
            {
                return commonSpawnsDay[UnityEngine.Random.Range(0, commonSpawnsDay.Count)];
            }
        }
    }

    public void AddPetToList(PetInfo petInfo)
    {
        availableSpawns.Add(petInfo);
    }

    public void UnlockRegion()
    {
        isUnlocked = true;
    }

    public PetInfo GetRandomPet()
    {
        return null;
    }
}

[Serializable]
public struct LevelUnlock
{
    public int levelToUnlock;
    public bool isUnlocked;
    public Action unlockAction;

    public LevelUnlock(int level, Action action, bool defaultState)
    {
        isUnlocked = defaultState;
        levelToUnlock = level;
        unlockAction = action;
    }

    public void Unlock()
    {
        if (isUnlocked)
            return;
        isUnlocked = true;
        unlockAction();
    }
}
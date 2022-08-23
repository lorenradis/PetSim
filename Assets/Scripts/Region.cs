using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Region  {

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

    public Region(){}

    public bool isUnlocked = false;

    public Item resource;
    public Item food;

    private int experience;
    private int level;
    public int ToNextLevel { get { return level * level * level; } set { } }

    public Region(string newName, Affinity newAffinity, Item newResource, Item newFood)
    {
        regionName = newName;
        affinity = newAffinity;
        resource = newResource;
        food = newFood;
    }

    public void GainExperience(int amount)
    {
        experience += amount;
        if(experience >= ToNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        level++;
        if(experience >= ToNextLevel)
        {
            LevelUp();
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

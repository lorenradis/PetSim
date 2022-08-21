using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Task {

    /*
    tasks will have a baseDuration (amount of time it should take to complete)
    tasks will have an associated stat (strength, smarts or speed) to determine how well a pet performs at it
    tasks will have a resource associated, a food or a building /crafting resource

    the behaviour i'm looking for from tasks is to be able to select a critter and assign them a task.
    a cut scene will play where you with the critter well as it leaves
    they'll be gone for some amount of time (determined by the task, distance to region)
    when they complete the task they bring back some amount of the relevant resource (amount determined by pet's stats and some chance, also by the affinity/element of the region.

    should the task have a base difficulty as well, to roll the pet's relevant stat against?

    interacting with a pet brings up the prompt > Check, ASSIGN, SOMETHING ELSE, CANCEL

        Check > Show the stats menu

        Assign > Show prompt > Gather Supplies, FOrage for Food, Make Friends, Cancel
    
            Selecting any of the first three > Show Map (Where would you like your pet to " + task.taskName + "?");

                Click location on map > Show Confirm Prompt > Yes, No.

        where is a task completed? in the pet? In the clock? In the task itself? what needs to know the task has been completed?

        make friends tasks will need a chance of success and a list of pets to potentially befriend.
    
    
    
    */

    private string taskName;
    public string TaskName { get{return taskName;} set{ taskName = value; }}

    public enum StatRequired{STRENGTH, SMARTS, SPEED}
    public StatRequired statRequired;

    private int baseDuration; //in minutes
    public int BaseDuration{get{return baseDuration;} set{ baseDuration = value; }}

    private int elapsedTime = 0;

    private int difficulty;
    public int Difficulty {get{return difficulty;}set{ difficulty = value; }}

    private Item resource;
    public Item Resource { get { return resource; } set { resource = value; } }

    private Region region;
    public Region myRegion { get { return region; } set { region = value; } }

    private PetInfo petInfo;
    public PetInfo myPetInfo { get { return petInfo; } set { petInfo = value; } }

    public Task()
    {

    }

    public Task(string newName, int newDur, int newDiff, Item newResource, Region newRegion, PetInfo newPet)
    {
        taskName = newName;
        baseDuration = newDur;
        difficulty = newDiff;
        resource = newResource;
        region = newRegion;
        petInfo = newPet;
        Debug.Log("I'm subscribing to the minute changed callback");
        GameClock.onMinuteChangedCallback += AdvanceTimer;
        
    }

    public void AdvanceTimer()
    {
        //should subscribe to gameClock's onminutechanged callback
        elapsedTime++;
        if(elapsedTime >= baseDuration)
        {
            CompleteThisTask();
        }
    }

    private void CompleteThisTask()
    {
        DialogManager.instance.ShowSimpleDialog(petInfo.petName + " completed their task to " + TaskName + "!  Welcome back " + petInfo.petName + "!");
        petInfo.CompleteCurrentTask();
        
    }

}

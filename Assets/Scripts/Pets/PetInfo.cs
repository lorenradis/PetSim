using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PetInfo
{
    public string petName;

    public Stat Strength;
    public Stat Smarts;
    public Stat Speed;
    public Stat Luck;

    private int anxiety;
    public int Anxiety
    {
        get { return anxiety; }
        set { anxiety = Mathf.Clamp(value, 0, 100); }
    }
    private int aggression;
    public int Aggression
    {
        get { return aggression; }
        set { aggression = Mathf.Clamp(value, 0, 100); }
    }
    public int health;
    public int maxHealth;
    public int energy;
    public int maxEnergy;
    public int stamina;
    public int maxStamina;

    public int experience;

    public int level;

    public int researchEXP;
    public int researchLevel;
    public int EXPToNextResearchLevel { get { return researchLevel * researchLevel * researchLevel * 10; } }
    //level 1 learn pet name and affinity
    //level 2 learn hated food
    //level 3 learn loved food
    //level 4 learn ??
    //level 5 learn ??

    public int happiness;
    public int maxHappiness;

    public int playerDamageAmount = 1;

    public int food;
    public int play;
    public int clean;

    private int needFrequency = 27;
    private int ticks = 0;

    public int loyalty;

    public string description;

    public Affinity affinity;

    public Sprite icon;
    public Sprite portrait;

    public RuntimeAnimatorController overworldAnimator;
    public RuntimeAnimatorController battleAnimator;

    public float moveMod = 1f;

    public Task currentTask;

    public delegate void OnAssignTask();
    public static OnAssignTask onAssignTaskCallback;

    public enum PetState { IDLE, ONTASK, PARTNER, OTHER }
    public PetState petState;

    public Item lovedFood;
    public Item hatedFood;

    private void ChangeState(PetState newState)
    {
        if (petState != newState)
        {
            petState = newState;
        }
    }

    public PetInfo()
    {

    }

    public PetInfo(string newName, int newStr, int newSmrt, int newSpd, int newAnx, int newAggro, Affinity newAffinity, string desc,
        RuntimeAnimatorController newAnimator, RuntimeAnimatorController newBattleAnimator,Sprite newIcon, Sprite newPortrait, Item newLovedFood, Item newHatedFood)
    {
        petName = newName;

        Strength = new Stat("Strength", newStr);
        Smarts = new Stat("Smarts", newSmrt);
        Speed = new Stat("Speed", newSpd);
        Luck = new Stat("Luck", 0);

        anxiety = newAnx;
        aggression = newAggro;

        maxHealth = Mathf.FloorToInt(Strength.Value * Random.Range(1.95f, 2.05f));
        health = maxHealth;
        maxEnergy = Mathf.FloorToInt(Smarts.Value * Random.Range(1.95f, 2.05f));
        energy = maxEnergy;
        maxStamina = Mathf.FloorToInt(Speed.Value * Random.Range(1.95f, 2.05f));
        stamina = maxStamina;

        experience = 0;
        level = 1;

        food = 100;
        play = 100;
        clean = 100;

        loyalty = 0;

        description = desc;

        affinity = newAffinity;

        overworldAnimator = newAnimator;
        battleAnimator = newBattleAnimator;

        icon = newIcon;
        portrait = newPortrait;

        hatedFood = newHatedFood;
        lovedFood = newLovedFood;
    }

    public void IncrementNeeds()
    {
        Debug.Log("Incrementing needs");
        ticks++;
        if (ticks >= needFrequency)
        {
            ticks = 0;
            food--;
            play--;
            clean--;
        }
    }

    public void SetPartner(bool isPartner)
    {
        if (isPartner)
        {
            ChangeState(PetState.PARTNER);
        }
        else
        {
            ChangeState(PetState.IDLE);
        }
    }

    public void SetAggressionAndAnxiety()
    {
        anxiety = Mathf.FloorToInt(Random.Range(.75f, 1.025f) * anxiety);
        aggression = Mathf.FloorToInt(Random.Range(.75f, 1.25f) * aggression);
    }

    public void AssignTask(Task task, Region region)
    {
        currentTask = new Task(task.TaskName, task.BaseDuration, task.Difficulty, region, this, task.taskType);
        currentTask.myRegion = region;

        if (currentTask.taskType != Task.TaskType.IDLE)
        {
            ChangeState(PetState.ONTASK);
            GameClock.onMinuteChangedCallback += AdvanceTaskTimer;
        }
        if (onAssignTaskCallback != null)
        {
            onAssignTaskCallback.Invoke();
        }

    }

    private void AdvanceTaskTimer()
    {
        currentTask.AdvanceTimer();
    }

    public void CompleteCurrentTask()
    {
        ChangeState(PetState.IDLE);
        int amount = Random.Range(1, 10);
        int secondaryAmount = 0;
        int roll = Random.Range(1, 100);
        if (roll < 50)
        {
            secondaryAmount = Mathf.Clamp(amount / 2, 1, amount / 2);
        }
        string message = petName + " brought back " + amount + " ";
        string secondaryMessage = "What luck!  While they were out they also found ";

        DialogManager.instance.ShowSimpleDialog(petName + " completed their task to " + currentTask.TaskName + "!  Welcome back " + petName + "!");

        switch (currentTask.taskType)
        {
            case Task.TaskType.GATHER:
                message += currentTask.myRegion.resource.itemName;
                DialogManager.instance.ShowSimpleDialog(message);
                GameManager.instance.itemManager.AddResource(currentTask.Resource, amount);
                if (secondaryAmount > 0)
                {
                    GameManager.instance.itemManager.AddFood(currentTask.Food, secondaryAmount);
                    secondaryMessage += secondaryAmount + " " + currentTask.Food.itemName + "!";
                    DialogManager.instance.ShowSimpleDialog(secondaryMessage);
                }
                break;
            case Task.TaskType.FOOD:
                message += currentTask.myRegion.food.itemName;
                DialogManager.instance.ShowSimpleDialog(message);
                GameManager.instance.itemManager.AddFood(currentTask.Food, amount);
                if (secondaryAmount > 0)
                {
                    GameManager.instance.itemManager.AddResource(currentTask.Resource, secondaryAmount);
                    secondaryMessage += secondaryAmount + " " + currentTask.Resource.itemName + "!";
                    DialogManager.instance.ShowSimpleDialog(secondaryMessage);
                }
                break;
            case Task.TaskType.EXPLORE:
                message = petName + " explored the " + currentTask.myRegion.regionName + " region, increasing your knowledge by " + amount;
                currentTask.myRegion.GainExperience(amount);
                if(secondaryAmount>0)
                {
                    roll = Random.Range(1, 100);
                    if (roll < 50)
                    {
                        GameManager.instance.itemManager.AddResource(currentTask.Resource, secondaryAmount);
                        secondaryMessage += secondaryAmount + " " + currentTask.Resource.itemName + "!";
                    }
                    else
                    {
                        GameManager.instance.itemManager.AddFood(currentTask.Food, secondaryAmount);
                        secondaryMessage += secondaryAmount + " " + currentTask.Food.itemName + "!";
                    }
                    DialogManager.instance.ShowSimpleDialog(secondaryMessage);
                }

                break;
            default:
                break;
        }

        AssignTask(new Task(), null);

        GameClock.onMinuteChangedCallback -= AdvanceTaskTimer;

        if (onAssignTaskCallback != null)
            onAssignTaskCallback.Invoke();
    }

    public void GainResearchEXP(int amount)
    {
        researchEXP += amount;
        if(researchEXP >= EXPToNextResearchLevel)
        {
            IncreaseResearchLevel();
        }
    }

    public string GetLevelKnowledge(int _level)
    {
        switch(_level)
        {
            case 1:
                return "This pet is called " + petName + ", and they have an affinity for " + affinity.affinityName;
                break;
            case 2:
                return "They don't seem to care for " + hatedFood.itemName + " very much at all.";
                break;
            case 3:
                return "They REALLY love " + lovedFood.itemName + "!";
                break;
            case 4:
                return "level 4, i dunno";
                break;
            case 5:
                return "level 5, wow";
                break;
            default:break;
        }
        return "something sure broke in here!";
    }

    private void IncreaseResearchLevel()
    {
        researchLevel++;
        if(researchEXP  >= EXPToNextResearchLevel)
        {
            IncreaseResearchLevel();
        }
    }
}
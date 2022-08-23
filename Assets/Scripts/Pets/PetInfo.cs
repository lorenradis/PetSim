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

    public int health;
    public int maxHealth;
    public int energy;
    public int maxEnergy;
    public int stamina;
    public int maxStamina;

    public int experience;

    public int level;

    public int food;
    public int play;
    public int clean;

    private int needFrequency = 27;
    private int ticks = 0;

    public int loyalty;

    public string description;

    public Affinity affinity;

    public RuntimeAnimatorController overworldAnimator;

    public float moveMod = 1f;

    public Task currentTask;

    public delegate void OnAssignTask();
    public static OnAssignTask onAssignTaskCallback;

    public enum PetState { IDLE, ONTASK, OTHER }//maybe we'll use this to clean up what a pet is doing and where they are?

    public PetInfo()
    {

    }

    public PetInfo(string newName, int newStr, int newSmrt, int newSpd, Affinity newAffinity, string desc)
    {
        petName = newName;

        Strength = new Stat("Strength", newStr);
        Smarts = new Stat("Smarts", newSmrt);
        Speed = new Stat("Speed", newSpd);
        Luck = new Stat("Luck", 0);

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
    }

    public void IncrementNeeds()
    {
        ticks++;
        if (ticks >= needFrequency)
        {
            food--;
            play--;
            clean--;
        }
    }

    public void AssignTask(Task task, Region region)
    {
        currentTask = new Task(task.TaskName, task.BaseDuration, task.Difficulty, region, this, task.taskType);
        currentTask.myRegion = region;

        if (currentTask.taskType != Task.TaskType.IDLE)
            GameClock.onMinuteChangedCallback += AdvanceTaskTimer;
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
        int amount = Random.Range(1, 10);
        int secondaryAmount = 0;
        int roll = Random.Range(1, 100);
        if (roll < 50)
        {
            secondaryAmount = Mathf.Clamp(amount / 2, 1, amount / 2);
        }
        string message = petName + " brought back " + amount + " ";
        switch (currentTask.taskType)
        {
            case Task.TaskType.GATHER:
                message += currentTask.myRegion.resource.itemName;
                GameManager.instance.itemManager.AddResource(currentTask.Resource, amount);

                GameManager.instance.itemManager.AddFood(currentTask.Food, secondaryAmount);

                break;
            case Task.TaskType.FOOD:
                message += currentTask.myRegion.food.itemName;
                GameManager.instance.itemManager.AddFood(currentTask.Food, amount);
                GameManager.instance.itemManager.AddResource(currentTask.Resource, secondaryAmount);
                break;
            case Task.TaskType.EXPLORE:
                message = petName + " explored the " + currentTask.myRegion.regionName + " region, increasing your knowledge by " + amount;
                currentTask.myRegion.GainExperience(amount);
                roll = Random.Range(1, 100);
                if(roll < 50)
                {
                    GameManager.instance.itemManager.AddResource(currentTask.Resource, secondaryAmount);
                }
                else
                {
                    GameManager.instance.itemManager.AddFood(currentTask.Food, secondaryAmount);
                }
                break;
            default:
                break;
        }

        DialogManager.instance.ShowSimpleDialog(petName + " completed their task to " + currentTask.TaskName + "!  Welcome back " + petName + "!");
        DialogManager.instance.ShowSimpleDialog(message);

        AssignTask(new Task(), null);

        GameClock.onMinuteChangedCallback -= AdvanceTaskTimer;

        if (onAssignTaskCallback != null)
            onAssignTaskCallback.Invoke();
    }
}
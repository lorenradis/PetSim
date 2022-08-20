using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PetInfo{

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

    public int loyalty;

    public string description;

    public Affinity affinity;

    public RuntimeAnimatorController overworldAnimator;

    public float moveMod = 1f;

    public Task currentTask;
    
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

    public void AssignTask(Task task)
    {
        currentTask = task;
    }
}
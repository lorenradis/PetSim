using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo 
{
    public int health;
    public int maxHealth;
    public int energy;
    public int maxEnergy;

    public delegate void OnEnergyChanged();
    public static OnEnergyChanged onEnergyChangedCallback;

    public PlayerInfo() { }

    public PlayerInfo(int newHealth, int newEnergy)
    {
        maxHealth = newHealth;
        health = maxHealth;
        maxEnergy = newEnergy;
        energy = newEnergy;
        GameClock.onMinuteChangedCallback += DecreaseEnergy;
    }

    public void DecreaseEnergy()
    {
        energy--;
        Reconcile();
    }

    public void DecreaseEnergy(int amount)
    {
        energy -= amount;
        Reconcile();
    }

    public void IncreaseEnergy()
    {
        energy++;
        Reconcile();
    }

    public void IncreaseEnergy(int amount)
    {
        energy += amount;
        Reconcile();
    }

    public bool HasEnergy()
    {
        return energy > 0;
    }

    public bool HasEnergy(int amount)
    {
        return energy >= amount;
    }

    private void Reconcile()
    {
        energy = Mathf.Clamp(energy, 0, maxEnergy);
        if (onEnergyChangedCallback != null)
            onEnergyChangedCallback.Invoke();
    }
}

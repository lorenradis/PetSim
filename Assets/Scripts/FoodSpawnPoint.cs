using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawnPoint
{
    //has a unique integer id, gameObjects will use this to find their info from the manager
    //keeps track of which fruit i spawn, how many, how long before re-growing
    private int uniqueID;
    public int UniqueID { get { return uniqueID; } set { } }
    [SerializeField] private int ticksToRefresh;
    private int ticks = 0;
    public int Ticks { get { return ticks; } set { ticks = value; } }

    public Item foodToSpawn;

    public FoodSpawnPoint() { }

    public FoodSpawnPoint(int newID, int newTicks, Item newFood)
    {
        uniqueID = newID;
        foodToSpawn = newFood;
        ticksToRefresh = newTicks;
        ticks = newTicks;
    }

    public void OnTimeChanged()
    {
        ticks++;
    }

    public bool HasFood()
    {
        return ticks >= ticksToRefresh;
    }
}
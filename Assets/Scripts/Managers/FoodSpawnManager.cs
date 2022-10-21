using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawnManager
{
    //keeps track of all food spawners, which are picked, which should produce fruit, etc.
    //subscribes to clock time changed, updates all food spawn points so they know to spawn fruit if enough time has passed

    private static FoodSpawnManager instance;
    public static FoodSpawnManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new FoodSpawnManager();
            }
            return instance;
        }
        set { }
    }

    public List<FoodSpawnPoint> foodSpawnPoints = new List<FoodSpawnPoint>();

    public FoodSpawnManager()
    {
        //create all food spawn points here and add to list
        //subscribe our updatetime function to the gameclock's ontimechanged delegate
        FoodSpawnPoint forestSpawn1 = new FoodSpawnPoint(00000, 24 * 60, ItemManager.grassFood);
        FoodSpawnPoint forestSpawn2 = new FoodSpawnPoint(00001, 24 * 60, ItemManager.grassFood);
        FoodSpawnPoint forestSpawn3 = new FoodSpawnPoint(00002, 24 * 60, ItemManager.grassFood);

        foodSpawnPoints.Add(forestSpawn1);
        foodSpawnPoints.Add(forestSpawn2);
        foodSpawnPoints.Add(forestSpawn3);

        GameClock.onMinuteChangedCallback += OnTimeChanged;
    }

    private void OnTimeChanged()
    {
        foreach (FoodSpawnPoint spawner in foodSpawnPoints)
        {
            spawner.OnTimeChanged();
        }
    }

    public FoodSpawnPoint GetFoodSpawnByID(int newID)
    {
        foreach (FoodSpawnPoint spawner in foodSpawnPoints)
        {
            if (newID == spawner.UniqueID)
            {
                return spawner;
            }
        }
        Debug.Log("Error finding spawner");
        return null;
    }
}
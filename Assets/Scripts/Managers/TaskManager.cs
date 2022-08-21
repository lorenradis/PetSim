using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager 
{

    public Task GatherResources;
    public Task ForageFood;
    public Task FindFriends; //i think only the player can find new pets, this should be changed to "explore" or something, and it'll be how we unlock new regions and new areas within regions

    public List<Task> tasks = new List<Task>();

    public void SetupTasks()
    {
        GatherResources = new Task();
        GatherResources.TaskName = "Gather Resources";
        GatherResources.BaseDuration = 5;
        ForageFood = new Task();
        ForageFood.TaskName = "Forage for Food";
        FindFriends = new Task();
        FindFriends.TaskName = "Find New Friends";
    }
}

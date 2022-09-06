using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager
{

    public Task GatherResources;
    public Task ForageFood;
    public Task Explore;

    public List<Task> tasks = new List<Task>();

    public void SetupTasks()
    {
        GatherResources = new Task("Gather Resources", 60, 10,null, null, Task.TaskType.GATHER);
        ForageFood = new Task("Forage for Food", 60, 10, null, null, Task.TaskType.FOOD);
        Explore = new Task("Explore", 60, 1, null, null, Task.TaskType.EXPLORE);
    }
}

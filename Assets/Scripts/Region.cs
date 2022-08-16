using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    public List<PetInfo> availableSpawns = new List<PetInfo>();

    public Region(){}



}

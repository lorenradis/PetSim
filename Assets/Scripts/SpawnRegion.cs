using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnRegion : MonoBehaviour {

    public List<PetInfo> availableSpawns = new List<PetInfo>(); //if possible we'll populate this in the inspector with templates from the pet manager.  if not, we'll use integers.

    [SerializeField] private GameObject overworldPetPrefab;

    private void SpawnPet()
    {
        int index = Random.Range(0, availableSpawns.Count);
        PetInfo petToSpawn = availableSpawns[index];
        PetInfo newPet = new PetInfo(petToSpawn.petName, petToSpawn.Strength.BaseValue, petToSpawn.Smarts.BaseValue, petToSpawn.Speed.BaseValue,
        petToSpawn.affinity, petToSpawn.description);
        GameObject newSpawn = Instantiate(overworldPetPrefab) as GameObject;
        newSpawn.GetComponent<OverworldPet>().SetPetInfo(newPet);
    }
}
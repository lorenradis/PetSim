using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TamedPetSpawner : MonoBehaviour
{

    [SerializeField] private GameObject tamePetPrefab;
    private List<GameObject> activePetObjects = new List<GameObject>();
    private List<PetInfo> activePets = new List<PetInfo>();

    private void Start()
    {
        PetInfo.onAssignTaskCallback += RefreshAllSpawns;
    }

    private void OnDisable()
    {
        PetInfo.onAssignTaskCallback -= RefreshAllSpawns;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            SpawnPresentPets();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            RemoveAllSpawns();
        }
    }

    private void SpawnPresentPets()
    {
        List<PetInfo> pets = GameManager.instance.petManager.currentPets;
        for (int i = 0; i < pets.Count; i++)
        {
//            Debug.Log(pets[i].petName + "'s task is " + pets[i].currentTask.TaskName);
            if (pets[i].currentTask.taskType == Task.TaskType.IDLE)
            {
                SpawnPet(pets[i]);
            }
            else
            {

            }
        }
    }

    private void SpawnPet(PetInfo petInfo)
    {
        GameObject newPet = Instantiate(tamePetPrefab);
        newPet.transform.position = transform.position;
        newPet.transform.SetParent(transform);
        newPet.GetComponent<OverworldPet>().SetPetInfo(petInfo);
        newPet.GetComponent<OverworldPet>().SetPetToTame();
        activePets.Add(petInfo);
        activePetObjects.Add(newPet);
    }

    public void RemoveAssignedPets()
    {

        for (int i = activePets.Count-1; i >= 0; i--)
        {
            if(activePets[i].currentTask.TaskName != "")
            {
                Destroy(activePetObjects[i]);
                activePetObjects.RemoveAt(i);
                activePets.RemoveAt(i);
            }
        }
    }

    private void RemoveAllSpawns()
    {
        for (int i = activePetObjects.Count - 1; i >= 0; i--)
        {
            Destroy(activePetObjects[i]);
            activePetObjects.RemoveAt(i);
            activePets.RemoveAt(i);
        }
    }


    public void RefreshAllSpawns()
    {
        RemoveAllSpawns();
        SpawnPresentPets();
    }
}

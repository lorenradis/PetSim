using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TamedPetSpawner : MonoBehaviour
{

    [SerializeField] private GameObject tamePetPrefab;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            List<PetInfo> pets = GameManager.instance.petManager.currentPets;
            for (int i = 0; i < pets.Count; i++)
            {
                if (pets[i].currentTask.TaskName == "")
                {
                    SpawnPet(pets[i]);
                }
                else
                {

                }
            }
        }
    }

    private void SpawnPet(PetInfo petInfo)
    {
        GameObject newPet = Instantiate(tamePetPrefab);
        newPet.transform.position = transform.position;
        newPet.transform.SetParent(transform);
        newPet.GetComponent<OverworldPet>().SetPetInfo(petInfo);
    }
}

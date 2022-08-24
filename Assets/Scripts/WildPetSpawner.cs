using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildPetSpawner : MonoBehaviour
{
    [SerializeField] private float elapsedTime = 0;
    [SerializeField] private float frequency = 1f;
    [SerializeField] private float spawnTime = 10f;
    [SerializeField] private int spawnChance = 10;

    [SerializeField] private int regionIndex;

    [SerializeField] private GameObject wildPetPrefab;

    private bool isActive = true;

    private BoxCollider2D bounds;

    private void Start()
    {
        bounds = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            isActive = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isActive = true;
        }
    }

    private void Update()
    {
        if (GameManager.instance == null || GameManager.instance.gameState == GameManager.GameState.NORMAL)
        {
            if (isActive)
            {
                elapsedTime += Time.deltaTime * frequency;
                if (elapsedTime > spawnTime)
                {
                    elapsedTime -= spawnTime;
                    RollForSpawn();
                }
            }
        }
    }

    private void RollForSpawn()
    {
        int roll = Random.Range(1, 100);
        if(roll < spawnChance)
        {
            SpawnNewPet();
        }
    }

    private void SpawnNewPet()
    {
        float randomX = Random.Range(-bounds.size.x / 2, bounds.size.x / 2);
        float randomY = Random.Range(-bounds.size.y * .5f, bounds.size.y * .5f);
        Vector2 newPosition = new Vector2(transform.position.x + randomX, transform.position.y + randomY);
        PetInfo petToSpawn = GameManager.instance.regionManager.regions[regionIndex].GetSpawn();
        PetInfo petInfo = new PetInfo(petToSpawn.petName, petToSpawn.Strength.BaseValue, petToSpawn.Smarts.BaseValue, petToSpawn.Speed.BaseValue, petToSpawn.affinity, petToSpawn.description, petToSpawn.overworldAnimator);
        GameObject newPetObject = Instantiate(wildPetPrefab, newPosition, Quaternion.identity) as GameObject;
        newPetObject.transform.SetParent(transform);
        newPetObject.GetComponent<OverworldPet>().SetPetInfo(petInfo);
        newPetObject.GetComponent<OverworldPet>().SetPetToWild();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawnObject : Interactable
{

    //has a unique integer id number, will pull its info from manager on start
    private Item food;

    private FoodSpawnPoint spawner;
    [SerializeField]
    private int uniqueID;

    [SerializeField] private Sprite emptySprite;
    [SerializeField] private Sprite fullSprite;

    private void Start()
    {
        SetSpawner(FoodSpawnManager.Instance.GetFoodSpawnByID(uniqueID));
    }

    private void SetSpawner(FoodSpawnPoint spawner)
    {
        this.spawner = spawner;
        food = spawner.foodToSpawn;
        GetComponent<SpriteRenderer>().sprite = spawner.HasFood() ? fullSprite : emptySprite;
        GetComponent<LootDrop>().alwaysDrops.Add(food);
    }

    public override void OnInteract()
    {
        //animator.SetTrigger("shake");
        if (spawner.HasFood())
        {
            GetComponent<LootDrop>().DropLoot();
            GetComponent<SpriteRenderer>().sprite = emptySprite;
            spawner.Ticks = 0;
        }
        else
        {
        }
    }
}
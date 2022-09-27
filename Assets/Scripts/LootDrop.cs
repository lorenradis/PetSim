using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LootDrop : MonoBehaviour
{

    public List<Item> alwaysDrops = new List<Item>(); //100%
    public List<Item> commonDrops = new List<Item>(); //60%
    public List<Item> rareDrops = new List<Item>();   //30%
    public List<Item> epicDrops = new List<Item>();   //10%

    [SerializeField] private int alwaysWoodCount;
    [SerializeField] private int alwaysWaterCount;
    [SerializeField] private int alwaysSandCount;

    [SerializeField] private int alwaysBerriesCount;
    [SerializeField] private int alwaysMushroomsCount;
    [SerializeField] private int alwaysSucculentsCount;

    [SerializeField] private int commonWoodCount;
    [SerializeField] private int commonWaterCount;
    [SerializeField] private int commonSandCount;

    [SerializeField] private int commonBerriesCount;
    [SerializeField] private int commonMushroomsCount;
    [SerializeField] private int commonSucculentsCount;

    [SerializeField] private int rareWoodCount;
    [SerializeField] private int rareWaterCount;
    [SerializeField] private int rareSandCount;

    [SerializeField] private int rareBerriesCount;
    [SerializeField] private int rareMushroomsCount;
    [SerializeField] private int rareSucculentsCount;

    [SerializeField] private int epicWoodCount;
    [SerializeField] private int epicWaterCount;
    [SerializeField] private int epicSandCount;

    [SerializeField] private int epicBerriesCount;
    [SerializeField] private int epicMushroomsCount;
    [SerializeField] private int epicSucculentsCount;

    public int lootChance = 75;

    public GameObject dropObjectPrefab;

    private void Start()
    {
        for(int i = 0; i < 10; i++)
        {
            if(i < alwaysWoodCount) alwaysDrops.Add(ItemManager.wood);
            if(i < commonWoodCount) commonDrops.Add(ItemManager.wood);
            if(i < rareWoodCount) rareDrops.Add(ItemManager.wood);
            if(i < epicWoodCount) epicDrops.Add(ItemManager.wood);

            if(i < alwaysWaterCount) alwaysDrops.Add(ItemManager.water);
            if(i < commonWaterCount) commonDrops.Add(ItemManager.water);
            if(i < rareWaterCount) rareDrops.Add(ItemManager.water);
            if(i < epicWaterCount) epicDrops.Add(ItemManager.water);
            
            if(i < alwaysSandCount) alwaysDrops.Add(ItemManager.sand);
            if(i < commonSandCount) commonDrops.Add(ItemManager.sand);
            if(i < rareSandCount) rareDrops.Add(ItemManager.sand);
            if(i < epicSandCount) epicDrops.Add(ItemManager.sand);
            
            if(i < alwaysBerriesCount) alwaysDrops.Add(ItemManager.berries);
            if(i < commonBerriesCount) commonDrops.Add(ItemManager.berries);
            if(i < rareBerriesCount) rareDrops.Add(ItemManager.berries);
            if(i < epicBerriesCount) epicDrops.Add(ItemManager.berries);
            
            if(i < alwaysMushroomsCount) alwaysDrops.Add(ItemManager.mushrooms);
            if(i < commonMushroomsCount) commonDrops.Add(ItemManager.mushrooms);
            if(i < rareMushroomsCount) rareDrops.Add(ItemManager.mushrooms);
            if(i < epicMushroomsCount) epicDrops.Add(ItemManager.mushrooms);
            
            if(i < alwaysSucculentsCount) alwaysDrops.Add(ItemManager.succulent);
            if(i < commonSucculentsCount) commonDrops.Add(ItemManager.succulent);
            if(i < rareSucculentsCount) rareDrops.Add(ItemManager.succulent);
            if(i < epicSucculentsCount) epicDrops.Add(ItemManager.succulent);
        }
    }

    public void DropLoot()
    {
        //drop alwaysdrops
        for (int i = 0; i < alwaysDrops.Count; i++)
        {
            GameObject newDrop = Instantiate(dropObjectPrefab, transform.position, Quaternion.identity) as GameObject;
            newDrop.GetComponent<DroppedLoot>().SetItem(alwaysDrops[i]);
        }

        int lootRoll = Random.Range(1, 100);

        if (lootRoll < lootChance)
        {
            GameObject newLoot = Instantiate(dropObjectPrefab, transform.position, Quaternion.identity) as GameObject;
            int roll = Random.Range(1, 100);

            if (roll < 60)
            {
                if (commonDrops.Count > 0)
                {
                    newLoot.GetComponent<DroppedLoot>().SetItem(commonDrops[Random.Range(0, commonDrops.Count)]);
                }
            }
            else if (roll < 90)
            {
                if (rareDrops.Count > 0)
                {
                    newLoot.GetComponent<DroppedLoot>().SetItem(rareDrops[Random.Range(0, rareDrops.Count)]);
                }
            }
            else
            {
                if (epicDrops.Count > 0)
                {
                    newLoot.GetComponent<DroppedLoot>().SetItem(epicDrops[Random.Range(0, epicDrops.Count)]);
                }
            }
        }
    }
}

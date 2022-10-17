using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LootDrop : MonoBehaviour
{

    public List<Item> alwaysDrops = new List<Item>(); //100%
    public List<Item> commonDrops = new List<Item>(); //60%
    public List<Item> rareDrops = new List<Item>();   //30%
    public List<Item> epicDrops = new List<Item>();   //10%

    public int alwaysWood;
    public int alwaysStone;
    public int alwaysClay;
    public int alwaysIron;
    public int alwaysCoal;
    public int alwaysCrystal;

    public int lootChance = 75;

    public GameObject dropObjectPrefab;

    private void Start()
    {
        for (int i = 0; i < alwaysWood; i++)
        {
            alwaysDrops.Add(ItemManager.wood);
        }

        for (int i = 0; i < alwaysStone; i++)
        {
            alwaysDrops.Add(ItemManager.stone);
        }

        for (int i = 0; i < alwaysClay; i++)
        {
            alwaysDrops.Add(ItemManager.clay);
        }

        for (int i = 0; i < alwaysCoal; i++)
        {
            alwaysDrops.Add(ItemManager.coal);
        }

        for (int i = 0; i < alwaysIron; i++)
        {
            alwaysDrops.Add(ItemManager.iron);
        }

        for (int i = 0; i < alwaysCrystal; i++)
        {
            alwaysDrops.Add(ItemManager.crystal);
        }
    }

    public void DelayedDrop(float delay)
    {
        Invoke("DropLoot", delay);
    }

    public void DropLoot()
    {
        //drop alwaysdrops
        for (int i = 0; i < alwaysDrops.Count; i++)
        {
            GameObject newDrop = Instantiate(dropObjectPrefab, transform.position, Quaternion.identity) as GameObject;
            newDrop.GetComponent<DroppedLoot>().SetItem(alwaysDrops[i]);
        }

        if (commonDrops.Count > 0 && rareDrops.Count > 0 && epicDrops.Count > 0)
        {


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
                    //drop common
                }
                else if (roll < 90)
                {
                    if (rareDrops.Count > 0)
                    {
                        newLoot.GetComponent<DroppedLoot>().SetItem(rareDrops[Random.Range(0, rareDrops.Count)]);
                    }
                    //drop rare
                }
                else
                {
                    //drop epic
                    if (epicDrops.Count > 0)
                    {

                        newLoot.GetComponent<DroppedLoot>().SetItem(epicDrops[Random.Range(0, epicDrops.Count)]);
                    }


                }
            }
        }
    }
}

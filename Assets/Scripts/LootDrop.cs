using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LootDrop : MonoBehaviour
{

    public List<Item> alwaysDrops = new List<Item>(); //100%
    public List<Item> commonDrops = new List<Item>(); //60%
    public List<Item> rareDrops = new List<Item>();   //30%
    public List<Item> epicDrops = new List<Item>();   //10%

    public int lootChance = 75;

    public GameObject dropObjectPrefab;

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

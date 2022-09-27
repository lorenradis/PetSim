using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenObstacleObject : MonoBehaviour
{
    public int durability = 10;

    public void TakeDamage()
    {
        durability--;
        if (durability <= 0)
            BreakObstacle();
    }

    public void TakeDamage(int amount)
    {
        durability -= amount;
        if (durability <= 0)
            BreakObstacle();
    }

    public void BreakObstacle()
    { 
        int x = (int)transform.position.x- FarmManager.startX;
        int y = (int)transform.position.y - FarmManager.startY;


        for (int i = 0; i < GameManager.instance.farmManager.CurrentObstacles.Count; i++)
        {
            if((int)transform.position.x -FarmManager.startX == GameManager.instance.farmManager.CurrentObstacles[i].gridPosition.x && (int)transform.position.y - FarmManager.startY == GameManager.instance.farmManager.CurrentObstacles[i].gridPosition.y)
            {

                GameManager.instance.farmManager.CurrentObstacles.RemoveAt(i);
                GameManager.instance.farmManager.gridTiles[x, y] = FarmManager.TileState.BASIC;
                GameManager.instance.farmManager.gridTiles[x+1, y] = FarmManager.TileState.BASIC;
                GameManager.instance.farmManager.gridTiles[x, y+1] = FarmManager.TileState.BASIC;
                GameManager.instance.farmManager.gridTiles[x+1, y+1] = FarmManager.TileState.BASIC;
                break;
            }
        }

        if(GetComponent<LootDrop>())
        {
            GetComponent<LootDrop>().DropLoot();
        }
        Destroy(gameObject, 4f/12f);
    }
}

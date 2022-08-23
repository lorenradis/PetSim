using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ItemManager
{
    public List<Item> allItems = new List<Item>();
    public List<Item> allResources = new List<Item>();
    public List<Item> allFoods = new List<Item>();

    public static Item dummyItem;
    public Sprite dummyIcon;

    public static Item wood; //forest resource
    public Sprite woodSprite;
    public static Item berries; //forest food
    public Sprite berrySprite;
    public static Item water; //water resource
    public Sprite waterSprite;
    public static Item mushrooms; //water food
    public Sprite mushroomSprite;
    public static Item sand; //desert resource
    public Sprite sandSprite;
    public static Item succulent; //desert food
    public Sprite succulentSprite;

    public ItemManager()
    {

    }

    public void SetupItems()
    {
        dummyItem = new Item("Dummy");
        dummyItem.icon = dummyIcon;
        dummyItem.quantity = 10;

        wood = new Item("Wood");
        wood.icon = woodSprite;
        berries = new Item("Berries");
        berries.icon = berrySprite;
        water = new Item("Water");
        water.icon = waterSprite;
        mushrooms = new Item("Mushrooms");
        mushrooms.icon = mushroomSprite;
        sand = new Item("Sand");
        sand.icon = sandSprite;
        succulent = new Item("Succulent");
        succulent.icon = succulentSprite;

        allItems.Add(dummyItem);

        allResources.Add(wood);
        allFoods.Add(berries);
        allResources.Add(water);
        allFoods.Add(mushrooms);
        allResources.Add(sand);
        allFoods.Add(succulent);
    }

    public void AddFood(Item item, int amount)
    {
        item.quantity += amount;
    }

    public void AddResource(Item item, int amount)
    {
        item.quantity += amount;
    }

    public void AddItem(Item item, int amount)
    {

    }
}

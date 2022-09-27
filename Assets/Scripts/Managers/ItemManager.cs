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

    private Item selectedItem;
    public Item SelectedItem { get { return selectedItem; } }

    public int maxItems = 99;
    public int maxResources = 50;
    public int maxFoods = 9;

    public ItemManager()
    {

    }

    public void SetupItems()
    {
        dummyItem = new Item("Dummy");
        dummyItem.icon = dummyIcon;
        dummyItem.quantity = 10;
        dummyItem.itemType = Item.ItemType.ITEM;

        wood = new Item("Wood");
        wood.icon = woodSprite;
        wood.itemType = Item.ItemType.RESOURCE;

        berries = new Item("Berries");
        berries.icon = berrySprite;
        berries.itemType = Item.ItemType.FOOD;

        water = new Item("Water");
        water.icon = waterSprite;
        water.itemType = Item.ItemType.RESOURCE;

        mushrooms = new Item("Mushrooms");
        mushrooms.icon = mushroomSprite;
        mushrooms.itemType = Item.ItemType.FOOD;

        sand = new Item("Sand");
        sand.icon = sandSprite;
        sand.itemType = Item.ItemType.RESOURCE;

        succulent = new Item("Succulent");
        succulent.icon = succulentSprite;
        succulent.itemType = Item.ItemType.FOOD;

        allItems.Add(dummyItem);

        allResources.Add(wood);
        allFoods.Add(berries);
        allResources.Add(water);
        allFoods.Add(mushrooms);
        allResources.Add(sand);
        allFoods.Add(succulent);
    }

    public bool AddItem(Item item, int amount)
    {
        item.quantity += amount;

        switch(item.itemType)
        {
            case Item.ItemType.FOOD:
                if (item.quantity > maxFoods)
                {
                    item.quantity = maxFoods;
                    return false;
                }
                break;
            case Item.ItemType.RESOURCE:
                if (item.quantity > maxResources)
                {
                    item.quantity = maxResources;
                    return false;
                }
                break;
            case Item.ItemType.ITEM:

                break;
        }
        return true;
    }

    public void SelectItem(Item item)
    {
        selectedItem = item;
    }
}

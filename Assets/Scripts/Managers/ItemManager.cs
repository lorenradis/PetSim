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

    //resources
    public static Item wood; //forest resource
    public Sprite woodSprite;

    public static Item stone; //forest resource
    public Sprite stoneSprite;

    public static Item clay; //forest resource
    public Sprite claySprite;

    public static Item coal; //forest resource
    public Sprite coalSprite;

    public static Item iron; //forest resource
    public Sprite ironSprite;

    public static Item crystal; //forest resource
    public Sprite crystalSprite;

    public static Item grassFood;
    public Sprite grassFoodSprite;
    public static Item fireFood;
    public Sprite fireFoodSprite;
    public static Item waterFood;
    public Sprite waterFoodSprite;
    public static Item earthFood;
    public Sprite earthFoodSprite;
    public static Item windFood;
    public Sprite windFoodSprite;
    public static Item healthyFood;
    public Sprite healthyFoodSprite;
    public static Item yummyFood;
    public Sprite yummyFoodSprite;
    public static Item neutralFood;
    public Sprite neutralFoodSprite;

    public static Item berries; //forest food
    public Sprite berrySprite;

    public static Item mushrooms; //water food
    public Sprite mushroomSprite;

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

        stone = new Item("Stone");
        stone.icon = stoneSprite;
        stone.itemType = Item.ItemType.RESOURCE;

        clay = new Item("Clay");
        clay.icon = claySprite;
        clay.itemType = Item.ItemType.RESOURCE;

        coal = new Item("Coal");
        coal.icon = coalSprite;
        coal.itemType = Item.ItemType.RESOURCE;

        iron = new Item("Iron");
        iron.icon = ironSprite;
        iron.itemType = Item.ItemType.RESOURCE;

        crystal = new Item("Crystal");
        crystal.icon = crystalSprite;
        crystal.itemType = Item.ItemType.RESOURCE;

        grassFood = new Item("Grass Food", Item.ItemType.FOOD, grassFoodSprite);
        fireFood = new Item("fire Food", Item.ItemType.FOOD, fireFoodSprite);
        waterFood = new Item("water Food", Item.ItemType.FOOD, waterFoodSprite);
        earthFood = new Item("earth Food", Item.ItemType.FOOD, earthFoodSprite);
        windFood = new Item("wind Food", Item.ItemType.FOOD, windFoodSprite);
        yummyFood = new Item("yummy Food", Item.ItemType.FOOD, yummyFoodSprite);
        healthyFood = new Item("healthy Food", Item.ItemType.FOOD, healthyFoodSprite);
        neutralFood = new Item("neutral Food", Item.ItemType.FOOD, neutralFoodSprite);

        berries = new Item("Berries");
        berries.icon = berrySprite;
        berries.itemType = Item.ItemType.FOOD;

        mushrooms = new Item("Mushrooms");
        mushrooms.icon = mushroomSprite;
        mushrooms.itemType = Item.ItemType.FOOD;

        succulent = new Item("Succulent");
        succulent.icon = succulentSprite;
        succulent.itemType = Item.ItemType.FOOD;

        allItems.Add(dummyItem);

        allResources.Add(wood);
        allResources.Add(stone);
        allResources.Add(clay);
        allResources.Add(coal);
        allResources.Add(iron);
        allResources.Add(crystal);
        allFoods.Add(berries);
        allFoods.Add(mushrooms);
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

    public void PayUpgradeCost(PurchaseableUpgrade upgrade)
    {

    }
}

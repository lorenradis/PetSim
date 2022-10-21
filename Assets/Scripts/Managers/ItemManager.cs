using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager
{

    public List<Item> allItems = new List<Item>();
    public List<Item> allResources = new List<Item>();
    public List<Item> allFoods = new List<Item>();

    public static Item dummyItem;
    public Sprite dummyIcon;

    //resources
    public static Item wood; //forest resource
    public static Item stone; //forest resource

    public static Item clay; //forest resource

    public static Item coal; //forest resource

    public static Item iron; //forest resource

    public static Item crystal; //forest resource

    public static Item grassFood;

    public static Item fireFood;

    public static Item waterFood;

    public static Item earthFood;

    public static Item windFood;

    public static Item lightFood;

    public static Item darkFood;

    public static Item healthyFood;

    public static Item yummyFood;

    public static Item neutralFood;


    private Item selectedItem;
    public Item SelectedItem { get { return selectedItem; } }

    public int maxItems = 99;
    public int maxResources = 50;
    public int maxFoods = 9;

    public ItemManager() { Debug.Log("I exist"); }

    public void SetupItems()
    {
        dummyItem = new Item("Dummy");
        dummyItem.icon = dummyIcon;
        dummyItem.quantity = 10;
        dummyItem.itemType = Item.ItemType.ITEM;

        //Resource Instantiation
        wood = new Item("Wood");
        wood.icon = ItemAssets.Instance.woodSprite;
        wood.itemType = Item.ItemType.RESOURCE;

        stone = new Item("Stone");
        stone.icon = ItemAssets.Instance.stoneSprite;
        stone.itemType = Item.ItemType.RESOURCE;

        clay = new Item("Clay");
        clay.icon = ItemAssets.Instance.claySprite;
        clay.itemType = Item.ItemType.RESOURCE;

        coal = new Item("Coal");
        coal.icon = ItemAssets.Instance.coalSprite;
        coal.itemType = Item.ItemType.RESOURCE;

        iron = new Item("Iron");
        iron.icon = ItemAssets.Instance.ironSprite;
        iron.itemType = Item.ItemType.RESOURCE;

        crystal = new Item("Crystal");
        crystal.icon = ItemAssets.Instance.crystalSprite;
        crystal.itemType = Item.ItemType.RESOURCE;

        //Food Instantiation
        grassFood = new Item("Grass Berries");
        grassFood.icon = ItemAssets.Instance.grassFoodSprite;
        grassFood.itemType = Item.ItemType.FOOD;

        waterFood = new Item("Dew Drops");
        waterFood.icon = ItemAssets.Instance.waterFoodSprite;
        waterFood.itemType = Item.ItemType.FOOD;

        fireFood = new Item("Emberries");
        fireFood.icon = ItemAssets.Instance.fireFoodSprite;
        fireFood.itemType = Item.ItemType.FOOD;

        earthFood = new Item("Stone Fruit");
        earthFood.icon = ItemAssets.Instance.earthFoodSprite;
        earthFood.itemType = Item.ItemType.FOOD;

        windFood = new Item("Piccolo Grass");
        windFood.icon = ItemAssets.Instance.windFoodSprite;
        windFood.itemType = Item.ItemType.FOOD;

        lightFood = new Item("Starfruit");
        lightFood.icon = ItemAssets.Instance.lightFoodSprite;
        lightFood.itemType = Item.ItemType.FOOD;

        darkFood = new Item("Hellerbross");
        darkFood.icon = ItemAssets.Instance.darkFoodSprite;
        darkFood.itemType = Item.ItemType.FOOD;

        yummyFood = new Item("Candy");
        yummyFood.icon = ItemAssets.Instance.yummyFoodSprite;
        yummyFood.itemType = Item.ItemType.FOOD;

        healthyFood = new Item("Brussel Sprouts");
        healthyFood.icon = ItemAssets.Instance.healthyFoodSprite;
        healthyFood.itemType = Item.ItemType.FOOD;

        neutralFood = new Item("Crackers");
        neutralFood.icon = ItemAssets.Instance.neutralFoodSprite;
        neutralFood.itemType = Item.ItemType.FOOD;

        //Lists
        allItems.Add(dummyItem);

        allResources.Add(wood);
        allResources.Add(stone);
        allResources.Add(clay);
        allResources.Add(coal);
        allResources.Add(iron);
        allResources.Add(crystal);

        allFoods.Add(grassFood);
        allFoods.Add(fireFood);
        allFoods.Add(waterFood);
        allFoods.Add(windFood);
        allFoods.Add(earthFood);
        allFoods.Add(lightFood);
        allFoods.Add(darkFood);
        allFoods.Add(healthyFood);
        allFoods.Add(yummyFood);
        allFoods.Add(neutralFood);
    }

    public bool AddItem(Item item, int amount)
    {
        //Debug.Log("I'm adding " + amount + " " + item.itemName + " to the inventory");

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

        //Debug.Log("We should have " + item.quantity + " total " + item.itemName + " now");

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

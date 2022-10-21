using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    public static ItemAssets Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    //Food Sprites
    public Sprite grassFoodSprite;
    public Sprite waterFoodSprite;
    public Sprite fireFoodSprite;
    public Sprite earthFoodSprite;
    public Sprite windFoodSprite;
    public Sprite lightFoodSprite;
    public Sprite darkFoodSprite;
    public Sprite yummyFoodSprite;
    public Sprite healthyFoodSprite;
    public Sprite neutralFoodSprite;


    //Resource Sprites
    public Sprite woodSprite;
    public Sprite stoneSprite;
    public Sprite claySprite;
    public Sprite coalSprite;
    public Sprite ironSprite;
    public Sprite crystalSprite;

}

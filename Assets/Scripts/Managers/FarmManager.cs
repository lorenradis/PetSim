using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class FarmManager
{

    public enum TileState { BASIC, LONGGRASS, DESERT, WATER }
    public TileState[,] gridTiles;

    private int width;
    public int Width { get { return width; } set { } }
    private int height;
    public int Height { get { return height; } set { } }

    private const int startX = 32;
    private const int startY = 18;

    public delegate void OnTileChanged();
    public static OnTileChanged onTileChangedCallback;

    public Sprite basicSprite;
    public Sprite longGrassSprite;
    public Sprite desertSprite;
    public Sprite waterSprite;

    public int farmTilesCount { get {
            return width * height;
        } set { } }
    public int longGrassTilesCount { get {
            return CountTilesOfType(TileState.LONGGRASS);
        } set { } }
    public int desertTilesCount { get {
            return CountTilesOfType(TileState.DESERT);
        } set { } }
    public int waterTilesCount { get {
            return CountTilesOfType(TileState.WATER);
        } set { } }

    public FarmManager()
    {

    }

    public void SetupFarm(int wide, int high)
    {
        width = wide;
        height = high;
        gridTiles = new TileState[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                gridTiles[x, y] = TileState.BASIC;
            }
        }
    }

    public void SetTileState(int x, int y, TileState newTileState)
    {
        x -= startX;
        y -= startY;

        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            gridTiles[x, y] = newTileState;
            if (onTileChangedCallback != null)
                onTileChangedCallback.Invoke();
        }
    }

    public TileState GetTileStateAtCoords(int x, int y)
    {
        return gridTiles[x, y];
    }

    public Sprite GetSprite(TileState tile)
    {
        switch(tile)
        {
            case TileState.BASIC:
                return basicSprite;
            case TileState.LONGGRASS:
                return longGrassSprite;
            case TileState.WATER:
                return waterSprite;
            case TileState.DESERT:
                return desertSprite;
        }
        return basicSprite;
    }

    private int CountTilesOfType(TileState tile)
    {
        int count = 0;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if(tile == gridTiles[x, y])
                {
                    count++;
                }
            }
        }
        return count;
    }
}

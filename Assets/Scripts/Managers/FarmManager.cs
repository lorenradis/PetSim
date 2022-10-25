    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class FarmManager
{

    public enum TileState { BASIC, LONGGRASS, DESERT, WATER, OBSTRUCTED }
    public TileState[,] gridTiles;

    private int width;
    public int Width { get { return width; } set { } }
    private int height;
    public int Height { get { return height; } set { } }

    public const int startX = 32;
    public const int startY = 18;

    public delegate void OnTileChanged();
    public static OnTileChanged onTileChangedCallback;

    public Sprite basicSprite;
    public Sprite longGrassSprite;
    public Sprite desertSprite;
    public Sprite waterSprite;

    private List<GardenObstacle> gardenObstacles = new List<GardenObstacle>();
    private List<GardenObstacle> currentObstacles = new List<GardenObstacle>();
    public List<GardenObstacle> CurrentObstacles { get { return currentObstacles; } } 

    public int farmTilesCount
    {
        get
        {
            return width * height;
        }
        set { }
    }
    public int longGrassTilesCount
    {
        get
        {
            return CountTilesOfType(TileState.LONGGRASS);
        }
        set { }
    }
    public int desertTilesCount
    {
        get
        {
            return CountTilesOfType(TileState.DESERT);
        }
        set { }
    }
    public int waterTilesCount
    {
        get
        {
            return CountTilesOfType(TileState.WATER);
        }
        set { }
    }

    public FarmManager()
    {

    }

    public void SetupFarm(int wide, int high)
    {
        width = wide;
        height = high;
        gridTiles = new TileState[width, height];

        GardenObstacle rockObstacle = new GardenObstacle(Vector2Int.zero, GardenObstacle.ObstacleType.ROCK);
        
        GardenObstacle stumpObstacle = new GardenObstacle(Vector2Int.zero, GardenObstacle.ObstacleType.STUMP);

        gardenObstacles.Add(rockObstacle);
        gardenObstacles.Add(stumpObstacle);

        int obstacleChance = 15;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if(x > 0 && x < width-1 && y > 0 && y < height-1 && gridTiles[x, y] == TileState.BASIC)
                {
                    int roll = Random.Range(1, 100);
                    if(roll <= obstacleChance)
                    {
                        gridTiles[x, y] = TileState.OBSTRUCTED;
                        gridTiles[x + 1, y] = TileState.OBSTRUCTED;
                        gridTiles[x, y + 1] = TileState.OBSTRUCTED;
                        gridTiles[x + 1, y + 1] = TileState.OBSTRUCTED;
                        GardenObstacle newObstacle = gardenObstacles[Random.Range(0, gardenObstacles.Count)];
                        newObstacle.gridPosition = new Vector2Int(x, y);
                        currentObstacles.Add(newObstacle);
                    }
                }
            }
        }
    }

    public bool SetTileState(int x, int y, TileState newTileState)
    {
        x -= startX;
        y -= startY;

        if (gridTiles[x, y] == TileState.OBSTRUCTED)
            return false;

        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            gridTiles[x, y] = newTileState;
            if (onTileChangedCallback != null)
                onTileChangedCallback.Invoke();
            return true;
        }
        return false;
    }

    public TileState GetTileStateAtCoords(int x, int y)
    {
        return gridTiles[x, y];
    }

    public Sprite GetSprite(TileState tile)
    {
        switch (tile)
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
                if (tile == gridTiles[x, y])
                {
                    count++;
                }
            }
        }
        return count;
    }
}

[System.Serializable]
public struct GardenObstacle
{
    public Vector2Int gridPosition;
    public enum ObstacleType { ROCK, STUMP }
    public ObstacleType obstacleType;

    public GardenObstacle(Vector2Int newPosition, ObstacleType newType)
    {
        gridPosition = newPosition;
        obstacleType = newType;
    }
}

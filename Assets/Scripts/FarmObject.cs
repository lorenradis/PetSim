using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FarmObject : MonoBehaviour
{

    public Tilemap groundTileMap;
    public Tilemap objectsBelowPlayerTilemap;
    public Tilemap objectsAbovePlayerTilemap;
    public RuleTile longGrassRuleTile;
    public RuleTile dessertRuleTile;
    public RuleTile waterRuleTile;
    public RuleTile basicTile;

    private BoxCollider2D bounds;

    public RuleTile fenceTile;

    //obstacle region
    public GameObject rockPrefab;
    public GameObject stumpPrefab;

    private void OnEnable()
    {
        FarmManager.onTileChangedCallback += DrawFarm;
    }

    private void OnDisable()
    {
        FarmManager.onTileChangedCallback -= DrawFarm;
    }

    private void Start()
    {
        DrawFarm();
        bounds = GetComponent<BoxCollider2D>();
        bounds.size = new Vector2(GameManager.instance.farmManager.Width, GameManager.instance.farmManager.Height);
        bounds.offset = bounds.size * .5f;
    }

    private void DrawFarm()
    {
        FarmManager farm = GameManager.instance.farmManager;
        //WITH FENCING
        /*        for (int x = -1; x <= farm.Width; x++)
                {
                    for (int y = -1; y <= farm.Height; y++)
                    {
                        if (x < 0 || x == farm.Width || y < 00 || y == farm.Height)
                        {
                            if (y > -1 || x > 3)
                                objectsBelowPlayerTilemap.SetTile(new Vector3Int(x, y), fenceTile);
                        }
                        else
                        {
                            DrawTileAtCoords(farm.gridTiles[x, y], x, y);
                        }
                    }
                }
        */
        //WITHOUT FENCING
        for (int x = 0; x < farm.Width; x++)
        {
            for (int y = 0; y < farm.Height; y++)
            {
                DrawTileAtCoords(farm.gridTiles[x, y], x, y);
            }
        }
        for (int i = 0; i < farm.CurrentObstacles.Count; i++)
        {
            GameObject newObstacle;
            switch (farm.CurrentObstacles[i].obstacleType)
            {
                case GardenObstacle.ObstacleType.ROCK:
                    newObstacle = Instantiate(rockPrefab, new Vector2(farm.CurrentObstacles[i].gridPosition.x + FarmManager.startX, farm.CurrentObstacles[i].gridPosition.y + FarmManager.startY), Quaternion.identity) as GameObject;
                    break;
                case GardenObstacle.ObstacleType.STUMP:
                    newObstacle = Instantiate(stumpPrefab, new Vector2(farm.CurrentObstacles[i].gridPosition.x + FarmManager.startX, farm.CurrentObstacles[i].gridPosition.y + FarmManager.startY), Quaternion.identity) as GameObject;
                    break;
                default: break;
            }
        }
    }

    private void DrawTileAtCoords(FarmManager.TileState tileState, int x, int y)
    {
        Vector3Int newPosition = new Vector3Int(x, y, 0);

        switch (tileState)
        {
            case FarmManager.TileState.BASIC:
            case FarmManager.TileState.OBSTRUCTED:
                groundTileMap.SetTile(newPosition, basicTile);
                break;
            case FarmManager.TileState.LONGGRASS:

                groundTileMap.SetTile(newPosition, longGrassRuleTile);
                break;
            case FarmManager.TileState.WATER:

                groundTileMap.SetTile(newPosition, waterRuleTile);
                break;
            case FarmManager.TileState.DESERT:

                groundTileMap.SetTile(newPosition, dessertRuleTile);
                break;
            default:
                break;
        }
    }
}

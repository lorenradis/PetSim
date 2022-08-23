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
    public Tile basicTile;

    public RuleTile fenceTile;

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

    }

    private void DrawFarm()
    {
        FarmManager farm = GameManager.instance.farmManager;
        for (int x = 0; x < farm.Width; x++)
        {
            for (int y = 0; y < farm.Height; y++)
            {
                DrawTileAtCoords(farm.gridTiles[x, y], x, y);
            }
        }
    }

    private void DrawTileAtCoords(FarmManager.TileState tileState, int x, int y)
    {
        Vector3Int newPosition = new Vector3Int(x, y, 0);

        switch (tileState)
        {
            case FarmManager.TileState.BASIC:
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

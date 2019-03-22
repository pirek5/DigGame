using System.Collections.Generic;
using UnityEngine;

public class GridData : MonoBehaviour
{
    //config
#pragma warning disable 0649
    [SerializeField] private int tileHealth;
#pragma warning restore 0649

    //basic data structure that contains all Tile classes
    public static Dictionary<Vector2Int, Tile> GridDictionary { get; private set; }

    //First tile in gridDictionary - to avoid null exception in comparison
    public static Tile DefaultTile { get; private set; }

    //singleton
    public static GridData Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            Init();
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    void Init()
    {
        FillDictionary();
        AssignNeighbors();
        MapDisplay.Instance.DisplayMap(GridDictionary);
    }

    private void FillDictionary()
    {
        var gridArray = MapGenerator.Instance.GenerateMap();
        int width = gridArray.GetLength(0);
        int height = gridArray.GetLength(1);

        GridDictionary = new Dictionary<Vector2Int, Tile>();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                TileType tileType = (TileType)gridArray[x, y]; //int to enum
                Tile newTile = new Tile(tileType, tileHealth);
                newTile.Position = new Vector3(x, y, 0);
                Vector2Int position = new Vector2Int(x, y);

                if (!GridDictionary.ContainsKey(position))
                {
                    GridDictionary.Add(position, newTile);
                }

                if (DefaultTile == null)
                {
                    DefaultTile = newTile;
                }
            }
        }
    }

    private void AssignNeighbors()
    {
        foreach (var tile in GridDictionary)
        {
            foreach (Vector2Int direction in Utilities.basicDirections)
            {
                if (GridDictionary.ContainsKey(tile.Key + direction))
                {
                    tile.Value.Neighbors.Add(GridDictionary[tile.Key + direction]);

                }
            }
        }
    }

    public void MarkTileToDig(Vector2Int tilePosition)
    {
        if (GridDictionary.ContainsKey(tilePosition))
        {
            Tile tile = GridDictionary[tilePosition];
            if (tile.DigIt == false && tile.TileType == TileType.full)
            {
                tile.DigIt = true;
                DigManager.Instance.MarkTileToDig(tile);
                MapDisplay.Instance.DisplayTile(tile);
            }
        }
    }

    public void EraseMark(Vector2Int tilePosition)
    {
        if (GridDictionary.ContainsKey(tilePosition))
        {
            Tile tile = GridDictionary[tilePosition];
            if (tile.DigIt == true)
            {
                tile.DigIt = false;
                DigManager.Instance.EraseTileToDig(tile);
                MapDisplay.Instance.DisplayTile(tile);
            }
            else if (tile.InfrastructureToBuild == true)
            {
                tile.InfrastructureToBuild = false;
                InfrastructureBuildManager.Instance.EraseTileToBuild(tile);
                MapDisplay.Instance.DisplayTile(tile);
            }
        }
    }

    public void MarkTileAsInfrastructureToBuild(Vector2Int tilePosition)
    {
        Vector2Int lowerTilePos = tilePosition + Vector2Int.down;
        if (GridDictionary.ContainsKey(tilePosition) && GridDictionary.ContainsKey(lowerTilePos))
        {
            Tile tile = GridDictionary[tilePosition];
            Tile lowerTile = GridDictionary[lowerTilePos];
            if (tile.InfrastructureToBuild == false && tile.HasInfrastructure == false && tile.TileType == TileType.empty && lowerTile.TileType == TileType.full)
            {
                tile.InfrastructureToBuild = true;
                InfrastructureBuildManager.Instance.MarkTileToBuild(tile);
                MapDisplay.Instance.DisplayTile(tile);
            }
        }
    }

    public void TileDigged(Tile tile)
    {
        if (GridDictionary.ContainsValue(tile))
        {
            tile.TileType = TileType.empty;
            tile.DigIt = false;
            MapDisplay.Instance.DisplayTile(tile);
            DigManager.Instance.EraseTileToDig(tile);
        }

        Vector2Int upperTilePos = Vector2Int.FloorToInt(tile.Position) + Vector2Int.up;
        if (GridDictionary.ContainsKey(upperTilePos))
        {
            Tile upperTile = GridDictionary[upperTilePos];
            upperTile.HasInfrastructure = false;
            upperTile.InfrastructureToBuild = false;
            MapDisplay.Instance.DisplayTile(upperTile);
        }
    }

    public void InfrastructureBuilt(Tile tile)
    {
        if (GridDictionary.ContainsValue(tile))
        {
            tile.InfrastructureToBuild = false;
            tile.HasInfrastructure = true;
            MapDisplay.Instance.DisplayTile(tile);
            InfrastructureBuildManager.Instance.EraseTileToBuild(tile);
        }
    }
}

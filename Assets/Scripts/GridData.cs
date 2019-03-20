using System.Collections.Generic;
using UnityEngine;

public class GridData : MonoBehaviour
{
    //config
    #pragma warning disable 0649
    [SerializeField] private int tileHealth;
    #pragma warning restore 0649

    //basic data structure that contains all Tile classes
    public static Dictionary<Vector2Int, Tile> gridDictionary; 

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
        MapDisplay.Instance.DisplayMap(gridDictionary);
    }

    private void FillDictionary()
    {
        var gridArray = MapGenerator.Instance.GenerateMap();
        int width = gridArray.GetLength(0);
        int height = gridArray.GetLength(1);

        gridDictionary = new Dictionary<Vector2Int, Tile>();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                TileType tileType = (TileType)gridArray[x, y]; //int to enum
                Tile newTile = new Tile(tileType, tileHealth);
                newTile.Position = new Vector3(x, y, 0);
                Vector2Int position = new Vector2Int(x, y);
                
                if (!gridDictionary.ContainsKey(position))
                {
                    gridDictionary.Add(position, newTile);
                }
            }
        }
    }

    private void AssignNeighbors()
    {
        foreach(var tile in gridDictionary)
        {
            foreach(Vector2Int direction in Utilities.basicDirections)
            {
                if(gridDictionary.ContainsKey(tile.Key + direction))
                {
                    tile.Value.Neighbors.Add(gridDictionary[tile.Key + direction]);
                
                }
            }
        }
    }

    public void MarkTileToDig(Vector2Int tilePosition)
    {
        if (gridDictionary.ContainsKey(tilePosition))
        {
            Tile tile = gridDictionary[tilePosition];
            if (tile.DigIt == false && tile.TileType == TileType.full)
            {
                tile.DigIt = true;
                DigManager.Instance.MarkTileToDig(tile);
                MapDisplay.Instance.DisplayTile(tile);
            }
        }
    }

    public void EraseTileToDig(Vector2Int tilePosition)
    {
        if (gridDictionary.ContainsKey(tilePosition))
        {
            Tile tile = gridDictionary[tilePosition];         
            if (tile.DigIt == true)
            {
                tile.DigIt = false;
                DigManager.Instance.EraseTileToDig(tile);
                MapDisplay.Instance.DisplayTile(tile);
            }
            
        }
    }

    public void DigTile(Vector2Int tilePosition) //TODO do wyjebania raczej
    {
        if (gridDictionary.ContainsKey(tilePosition))
        {
            gridDictionary[tilePosition].TileType = TileType.empty;
            gridDictionary[tilePosition].DigIt = false;
            MapDisplay.Instance.DisplayTile(gridDictionary[tilePosition]);
            DigManager.Instance.EraseTileToDig(gridDictionary[tilePosition]);
        }
    }

    public void TileDigged(Tile tile)
    {
        if (gridDictionary.ContainsValue(tile))
        {
            tile.TileType = TileType.empty;
            tile.DigIt = false;
            MapDisplay.Instance.DisplayTile(tile);
            DigManager.Instance.EraseTileToDig(tile);
        }
    }
}

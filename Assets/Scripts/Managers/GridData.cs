using System.Collections.Generic;
using UnityEngine;
using Zenject;

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

    //dependencies
    [Inject] MapGenerator mapGenerator;
    [Inject] MapDisplay mapDisplay;

    private void Awake()
    {
        Init();
    }

    void Init()
    {
        FillDictionary();
        AssignNeighbors();
        mapDisplay.DisplayMap(GridDictionary);
    }

    private void FillDictionary()
    {
        var gridArray = mapGenerator.GenerateMap();
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

    public bool CheckPlaceToBuild(List<Vector2Int> tilesPos)
    {
        foreach (var tilePos in tilesPos)
        {
            if (!GridDictionary.ContainsKey(tilePos)) { return false; }
            if (GridDictionary[tilePos].TileType != TileType.empty) { return false; }
            if (GridDictionary[tilePos].BuildingOnTile) { return false; }
            //if(!GridDictionary[tilePos].HasInfrastructure && tilePos.x == 0) { return false; }
        }
        return true;
    }
}

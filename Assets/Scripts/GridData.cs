using System.Collections.Generic;
using UnityEngine;

public class Vectors
{
    public static Vector2Int[] directions = { new Vector2Int(1, 0), new Vector2Int(-1,0), new Vector2Int(0,-1), new Vector2Int(0,1) };
}

public class GridData : MonoBehaviour
{
    //state
    public static Dictionary<Vector2Int, Tile> gridDictionary; 

    //cached
    private MapGenerator mapGenerator;
    private MapDisplay mapDisplay;

    void Awake()
    {
        mapGenerator = FindObjectOfType<MapGenerator>();
        mapDisplay = FindObjectOfType<MapDisplay>();
    }

    private void Start()
    {
        Init();
    }

    void Init()
    {
        FillDictionary();
        AssignNeighbors();
        mapDisplay.DisplayMap(gridDictionary);
    }

    private void FillDictionary()
    {
        var gridArray = mapGenerator.GenerateMap();
        int width = gridArray.GetLength(0);
        int height = gridArray.GetLength(1);

        gridDictionary = new Dictionary<Vector2Int, Tile>();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                TileType tileType = (TileType)gridArray[x, y]; //int to enum
                Tile newTileInfo = new Tile(tileType);
                newTileInfo.position = new Vector3(x, y, 0);
                Vector2Int position = new Vector2Int(x, y);
                
                if (!gridDictionary.ContainsKey(position))
                {
                    gridDictionary.Add(position, newTileInfo);
                }
            }
        }
    }

    private void AssignNeighbors()
    {
        foreach(var tile in gridDictionary)
        {
            foreach(Vector2Int direction in Vectors.directions)
            {
                if(gridDictionary.ContainsKey(tile.Key + direction))
                {
                    tile.Value.neighbors.Add(gridDictionary[tile.Key + direction]);
                }
            }
        }
    }

    public void DigTile(Vector2Int tilePosition, bool isDiging)
    {
        if (gridDictionary.ContainsKey(tilePosition))
        {
            gridDictionary[tilePosition].digIt = isDiging;
            mapDisplay.DisplayTile(tilePosition, gridDictionary[tilePosition]);
        }
    }

    public static Tile FindClosestTile(Vector3 position)
    {
        Vector2Int positionInt = Vector2Int.FloorToInt(position);
        List<Tile> possibleClosestTiles;
        if (gridDictionary.ContainsKey(positionInt)) 
        {
            possibleClosestTiles = gridDictionary[positionInt].neighbors;
            possibleClosestTiles.Add(gridDictionary[positionInt]);
        }
        else //much slower 'emergency' way
        {
            Debug.LogError("position out of map!");
            possibleClosestTiles = new List<Tile>(gridDictionary.Values);
        }

        var closestTile = possibleClosestTiles[0];
        for(int i = 1; i< possibleClosestTiles.Count; i++)
        {
            float currentClosestDistance = Vector3.Distance(position, closestTile.position);
            float testDistance = Vector3.Distance(position, possibleClosestTiles[i].position);
            if (testDistance <= currentClosestDistance)
            {
                closestTile = possibleClosestTiles[i];
            }
        }
        return closestTile;
    }
}

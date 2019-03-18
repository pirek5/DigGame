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
    private DigController digController;

    private void Awake()
    {
        mapGenerator = FindObjectOfType<MapGenerator>(); //TODO
        mapDisplay = FindObjectOfType<MapDisplay>(); //TODO
        digController = FindObjectOfType<DigController>(); //TODO
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
                Tile newTile = new Tile(tileType);
                newTile.position = new Vector3(x, y, 0);
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
            
            mapDisplay.DisplayTile(tilePosition, gridDictionary[tilePosition]);
            if (isDiging && gridDictionary[tilePosition].digIt == false)
            {
                digController.AddTileToDig(gridDictionary[tilePosition]);
            }
            else if(!isDiging && gridDictionary[tilePosition].digIt == true)
            {
                digController.DeleteTileToDig(gridDictionary[tilePosition]);
            }
            gridDictionary[tilePosition].digIt = isDiging;
        }
    }

    public void DeleteTile(Vector2Int tilePosition)
    {
        if (gridDictionary.ContainsKey(tilePosition))
        {
            gridDictionary[tilePosition].m_tileType = TileType.empty;
            gridDictionary[tilePosition].digIt = false;
            mapDisplay.DisplayTile(tilePosition, gridDictionary[tilePosition]);
            digController.DeleteTileToDig(gridDictionary[tilePosition]);
        }
    }

    public void DeleteTile(Tile tile)
    {
        DeleteTile(Vector2Int.FloorToInt(tile.position));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridData : MonoBehaviour
{
    //state
    public Dictionary<Vector2Int, TileInfo> gridDictionary; 

    //cached
    private MapGenerator mapGenerator;


    // Start is called before the first frame update
    void Awake()
    {
        mapGenerator = FindObjectOfType<MapGenerator>();
    }

    private void Start()
    {
        Init();
    }

    void Init()
    {
        var mapArray = mapGenerator.GenerateMap();
        int width = mapArray.GetLength(0);
        int height = mapArray.GetLength(1);

        gridDictionary = new Dictionary<Vector2Int, TileInfo>();
        for(int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                TileType tileType = (TileType)mapArray[x, y]; //int to enum
                TileInfo newTileInfo = new TileInfo(tileType);
                Vector2Int position = new Vector2Int(x, y);
                if(!gridDictionary.ContainsKey(position)){
                    gridDictionary.Add(position, newTileInfo);
                }
            } 
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    //config
    #pragma warning disable 0649
    [SerializeField] int mapHeight;
    [SerializeField] int mapWidth;
    #pragma warning restore 0649

    //singleton
    public static MapGenerator Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public int[,] GenerateMap()
    {
        int[,] mapArray = new int[mapWidth, mapHeight];
        for(int x = 0; x < mapWidth; x++)
        {
            for(int y = 0; y < mapHeight; y++)
            {
                if (y == mapHeight - 1)
                {
                    mapArray[x, y] = 0;
                }
                else
                {
                    mapArray[x, y] = 1;
                }
            }
        }
        return mapArray;
    }
}

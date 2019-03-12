﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] int mapHeight;
    [SerializeField] int mapWidth;

    public int[,] GenerateMap()
    {
        int[,] mapArray = new int[mapWidth, mapHeight];
        for(int x = 0; x < mapWidth; x++)
        {
            for(int y = 0; y < mapHeight; y++)
            {
                mapArray[x, y] = 1;
            }
        }
        return mapArray;
    }
}
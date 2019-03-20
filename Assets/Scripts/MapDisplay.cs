﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapDisplay : MonoBehaviour
{
    //config
    #pragma warning disable 0649
    [SerializeField] private TileBase fullTile;
    [SerializeField] private TileBase backgroundTile;
    [SerializeField] private TileBase digSelectionTile;

    //references set in editor
    [SerializeField] private Tilemap foreground;
    [SerializeField] private Tilemap background;
    [SerializeField] private Tilemap digSelection;
    #pragma warning restore 0649

    //singleton
    public static MapDisplay Instance { get; private set; }

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

    public void DisplayMap(Dictionary<Vector2Int, Tile> gridDictionary)
    {
        foreach(KeyValuePair<Vector2Int, Tile> tile in gridDictionary)
        {
            if(tile.Value.TileType == TileType.full)
            {
                foreground.SetTile((Vector3Int)tile.Key, fullTile);
            }
        }
    }

    public void DisplayTile(Vector2Int position, Tile tile)
    {
        if(tile.digIt)
        {
            digSelection.SetTile((Vector3Int)position, digSelectionTile);
        }
        else
        {
            digSelection.SetTile((Vector3Int)position, null);
        }

        if(tile.TileType == TileType.empty)
        {
            background.SetTile((Vector3Int)position, backgroundTile);
            foreground.SetTile((Vector3Int)position, null);
        }
    }
}

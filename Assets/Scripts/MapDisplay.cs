using System.Collections;
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
    [SerializeField] private TileBase infrastructureTile;
    [SerializeField] private TileBase infrastructureSelectionTile;

    //references set in editor
    [SerializeField] private Tilemap foreground;
    [SerializeField] private Tilemap background;
    [SerializeField] private Tilemap digSelection;
    [SerializeField] private Tilemap infrastructure;
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
                var ttile = background.GetTile((Vector3Int)tile.Key);
            }
            else if (tile.Value.TileType == TileType.empty)
            {
                background.SetTile((Vector3Int)tile.Key, backgroundTile);
                foreground.SetTile((Vector3Int)tile.Key, null);
            }
        }
    }

    public void DisplayTile(Tile tile)
    {
        if(tile.DigIt)
        {
            digSelection.SetTile(Vector3Int.FloorToInt(tile.Position), digSelectionTile);
        }
        else
        {
            digSelection.SetTile(Vector3Int.FloorToInt(tile.Position), null);
        }

        if (tile.HasInfrastructure)
        {
            infrastructure.SetTile(Vector3Int.FloorToInt(tile.Position), infrastructureTile);
        }
        else
        {
            infrastructure.SetTile(Vector3Int.FloorToInt(tile.Position), null);
        }

        if(tile.TileType == TileType.empty)
        {
            background.SetTile(Vector3Int.FloorToInt(tile.Position), backgroundTile);
            foreground.SetTile(Vector3Int.FloorToInt(tile.Position), null);
        }
    }

    public void TemporaryTileDisplay(Tile currentTile, Tile previousTile, State currentState)
    {
        if(currentState == State.dig && currentTile.TileType == TileType.full)
        {
            digSelection.SetTile(Vector3Int.FloorToInt(currentTile.Position), digSelectionTile);
            DisplayTile(previousTile);
        }
        else if(currentState == State.erase)
        {
            digSelection.SetTile(Vector3Int.FloorToInt(currentTile.Position), null);
            DisplayTile(previousTile);
        }
        else if(currentState == State.infrastructure && currentTile.TileType == TileType.empty)
        {
            digSelection.SetTile(Vector3Int.FloorToInt(currentTile.Position), infrastructureSelectionTile);
            DisplayTile(previousTile);
        }
        else
        {
            DisplayTile(previousTile);
            DisplayTile(currentTile);
        }
    }
}

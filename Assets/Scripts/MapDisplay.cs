using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapDisplay : MonoBehaviour
{
    //config
    [SerializeField] private TileBase fullTile;
    [SerializeField] private TileBase backgroundTile;
    [SerializeField] private TileBase digSelectionTile;

    //references set in editor
    [SerializeField] private Tilemap foreground;
    [SerializeField] private Tilemap background;
    [SerializeField] private Tilemap digSelection;


    public void DisplayMap(Dictionary<Vector2Int, Tile> gridDictionary)
    {
        foreach(KeyValuePair<Vector2Int, Tile> tile in gridDictionary)
        {
            background.SetTile((Vector3Int)tile.Key, backgroundTile);
            foreground.SetTile((Vector3Int)tile.Key, fullTile);
        }
    }

    public void DisplayTile(Vector2Int position, Tile tileInfo)
    {
        if(tileInfo.digIt)
        {
            digSelection.SetTile((Vector3Int)position, digSelectionTile);
        }
        else
        {
            digSelection.SetTile((Vector3Int)position, null);
        }
    }
}

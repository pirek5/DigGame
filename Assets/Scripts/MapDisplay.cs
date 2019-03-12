using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapDisplay : MonoBehaviour
{
    //config
    [SerializeField] private TileBase fullTile;
    [SerializeField] private TileBase backgroundTile;

    //references set in editor
    [SerializeField] private Tilemap foreground;
    [SerializeField] private Tilemap background;


    public void DisplayMap(Dictionary<Vector3Int, TileInfo> gridDictionary)
    {
        foreach(KeyValuePair<Vector3Int, TileInfo> tile in gridDictionary)
        {
            background.SetTile(tile.Key, backgroundTile);
            foreground.SetTile(tile.Key, fullTile);
        }
    }

    public void DisplayTile(Vector3Int position, TileType tileType)
    {
        if(tileType == TileType.empty)
        {
            foreground.SetTile(position, null);
        }
    }
}

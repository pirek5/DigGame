using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Utilities : MonoBehaviour
{
    public static Tile FindClosestTile(Vector3 position, List<Tile> tiles)
    {
        var closestTile = tiles[0];
        for (int i = 1; i < tiles.Count; i++)
        {
            float currentClosestDistance = Vector3.Distance(position, closestTile.position);
            float testDistance = Vector3.Distance(position, tiles[i].position);
            if (testDistance <= currentClosestDistance)
            {
                closestTile = tiles[i];
            }
        }
        return closestTile;
    }

    public static bool CheckIfNeighbour(Vector3 position, Tile tileToCheck)
    {
        var pos = Vector2Int.FloorToInt(position);
        if (GridData.gridDictionary.ContainsKey(pos))
        {
            Tile tile = GridData.gridDictionary[pos];
            foreach(Tile neighbour in tile.neighbors)
            {
                if(neighbour == tileToCheck) { return true; }
            }
        }
        return false;
    }

}

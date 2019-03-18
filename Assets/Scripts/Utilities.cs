using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Utilities : MonoBehaviour
{
    public static Tile TileFindClosestTile(Vector3 position, List<Tile> tiles)
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

}

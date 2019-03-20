using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Excavation
{
    public List<Tile> TilesInExcavation { get; private set; }
    public List<Tile> ExcavationEnternace { get; private set; } 

    public Excavation(Tile tile)
    {
        TilesInExcavation = new List<Tile>() { tile };
        UpdateDigEntrance();
    }

    public Excavation(List<Tile> tiles)
    {
        TilesInExcavation = new List<Tile>(tiles);
        UpdateDigEntrance();
    }

    public void AddTileToExcavation(Tile tile)
    {
        TilesInExcavation.Add(tile);
        UpdateDigEntrance();
    }

    public void UpdateDigEntrance()
    {
        ExcavationEnternace = new List<Tile>();
        foreach (Tile tile in TilesInExcavation)
        {
            foreach (Tile neighbor in tile.Neighbors)
            {
                if (neighbor.TileType == TileType.empty)
                {
                    ExcavationEnternace.Add(neighbor);
                }
            }
        }
    }

    public void DeleteTileInExcavation(Tile tileToDelete)
    {
         TilesInExcavation.Remove(tileToDelete);
         UpdateDigEntrance();
    }
}

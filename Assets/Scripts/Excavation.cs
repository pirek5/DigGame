using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Excavation
{
    public List<Tile> tilesInExcavation;
    public List<Tile> excavationEnternace = new List<Tile>(); 

    public Excavation(Tile tile)
    {
        tilesInExcavation = new List<Tile>() { tile };
        UpdateDigEntrance();
    }

    public void AddTileToExcavation(Tile tile)
    {
        tilesInExcavation.Add(tile);
        UpdateDigEntrance();
    }

    public void UpdateDigEntrance()
    {
        excavationEnternace.Clear();
        foreach (Tile tile in tilesInExcavation)
        {
            foreach (Tile neighbor in tile.neighbors)
            {
                if (neighbor.m_tileType == TileType.empty)
                {
                    excavationEnternace.Add(neighbor);
                }
            }
        }
    }

    public void DeleteTileInExcavation(Tile tileToDelete)
    {
         tilesInExcavation.Remove(tileToDelete);
         UpdateDigEntrance();
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DigManager : MonoBehaviour
{
    private void Awake()
    {
        TilesToDig = new List<Tile>();
    }

    public List<Tile> TilesToDig { get; private set; }

    public void MarkTileToDig(Tile tileToDig)
    {
        if (!TilesToDig.Contains(tileToDig))
        {
            TilesToDig.Add(tileToDig);
        }
    }

    public void EraseTileToDig(Tile tileToDelete)
    {
        if (TilesToDig.Contains(tileToDelete))
        {
            TilesToDig.Remove(tileToDelete);
        }
    }

    public List<Tile> GetPossibleEnternance(Tile destinationTile) 
    {
        List<Tile> exploredTiles = new List<Tile>();
        Queue<Tile> frontierTiles = new Queue<Tile>();
        frontierTiles.Enqueue(destinationTile);
        while (frontierTiles.Count > 0)
        {
            var currentTile = frontierTiles.Dequeue();
            foreach (Tile neighbour in currentTile.Neighbors)
            {
                if (!exploredTiles.Contains(neighbour) && !frontierTiles.Contains(neighbour) && neighbour.DigIt)
                {
                    frontierTiles.Enqueue(neighbour);
                }
            }
            exploredTiles.Add(currentTile);
        }

        List<Tile> excavationEnternace = new List<Tile>();
        foreach (Tile tile in exploredTiles)
        {
            foreach (Tile neighbor in tile.Neighbors)
            {
                if (neighbor.TileType == TileType.empty)
                {
                    excavationEnternace.Add(neighbor);
                }
            }
        }
        return excavationEnternace;
    }
}

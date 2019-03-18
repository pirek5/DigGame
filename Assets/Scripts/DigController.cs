using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DigController : MonoBehaviour
{
    public List<Tile> tilesToDig = new List<Tile>();
    public List<Tile> digEntrance = new List<Tile>();

    //separate areas which contains tiles to dig
    public List<Excavation> excavations = new List<Excavation>();


    public void AddTileToDig(Tile tileToDig)
    {
        List<Excavation> excavationNeighbors = new List<Excavation>();
        foreach(Excavation excavation in excavations)
        {
            foreach(Tile tileInExcavation in excavation.tilesInExcavation)
            {
                foreach(Tile tileToDigNeighbor in tileToDig.neighbors)
                {
                    if(tileToDigNeighbor == tileInExcavation)
                    {
                        if (!excavationNeighbors.Contains(excavation))
                        {
                            excavationNeighbors.Add(excavation);
                        }
                    }
                }
            }
        }

        //No neighbour excavation - made new excavation
        if (excavationNeighbors.Count == 0)
        {
            excavations.Add(new Excavation(tileToDig));
        }
        // 1 neighbour excavation - add tileToDig to existing excavation
        else if (excavationNeighbors.Count == 1)
        {
            excavationNeighbors[0].AddTileToExcavation(tileToDig);
        }
        // 2 or more excavation neighbours - merge excavations and add new tileToDig
        else if (excavationNeighbors.Count > 1)
        {
            excavationNeighbors[0].AddTileToExcavation(tileToDig);
            MergeExcavations(excavationNeighbors);
        }

        //tilesToDig.Add(tileToDig);
        //UpdateDigEntrance();
    }

    private void MergeExcavations(List<Excavation> neighbourExcavations)
    {
        for(int i = 1; i < neighbourExcavations.Count; i++)
        {
            neighbourExcavations[0].tilesInExcavation.AddRange(neighbourExcavations[i].tilesInExcavation);
            if (excavations.Contains(neighbourExcavations[i]))
            {
                excavations.Remove(neighbourExcavations[i]);
            }
        }
    }

    public void DeleteTileToDig(Tile tileToDelete)
    {
        foreach(Excavation excavation in excavations)
        {
            if (excavation.tilesInExcavation.Contains(tileToDelete))
            {
                excavation.DeleteTileInExcavation(tileToDelete);
                if(excavation.tilesInExcavation.Count == 0)
                {
                    excavations.Remove(excavation);
                }
                CheckIntegrity(excavation, tileToDelete);
                break; //excavation found, no need to continue function
            }
        }
    }

    public List<Tile> FindEnternace(Tile tile)
    {
        foreach (Excavation excavation in excavations)
        {
            if (excavation.tilesInExcavation.Contains(tile))
            {
                return excavation.excavationEnternace;
            }
        }
        return null;
    }

    public List<Tile> GetTilesToDig(Tile tile)
    {
        foreach (Excavation excavation in excavations)
        {
            if (excavation.tilesInExcavation.Contains(tile))
            {
                return excavation.tilesInExcavation;
            }
        }
        return null;
    }

    public void CheckIntegrity(Excavation excavation, Tile delatedTile)
    {
        List<Tile> neighboursToDelatedTile = new List<Tile>();
        foreach(Tile neighbour in delatedTile.neighbors)
        {
            if (excavation.tilesInExcavation.Contains(neighbour))
            {
                neighboursToDelatedTile.Add(neighbour);
            }
        }
        if(neighboursToDelatedTile.Count > 1) // 1 or less means that excavation preserved integrity
        {
            List<List<Tile>> newExcavations = new List<List<Tile>>();
            foreach(var tile in neighboursToDelatedTile)
            {
                if(!newExcavations.Any(s => s.Contains(tile)))
                {
                    var tilesInNewExcavation = BuildExcavation(tile, excavation.tilesInExcavation);
                    newExcavations.Add(tilesInNewExcavation);
                }
            }

            foreach(var newExcavation in newExcavations)
            {
                Excavation excav = new Excavation(newExcavation);
                excavations.Add(excav);
            }
        }
        excavations.Remove(excavation);
    }

    private List<Tile> BuildExcavation(Tile tile, List<Tile> tilesInExistingExcavation)
    {
        List<Tile> newExcavation = new List<Tile>();
        Queue<Tile> newTilesInNewExcavation = new Queue<Tile>();
        newTilesInNewExcavation.Enqueue(tile);
        while (newTilesInNewExcavation.Count > 0)
        {
            var currentTile = newTilesInNewExcavation.Dequeue();
            foreach(Tile neighbour in currentTile.neighbors)
            {
                if(!newExcavation.Contains(neighbour) && !newTilesInNewExcavation.Contains(neighbour) && tilesInExistingExcavation.Contains(neighbour))
                {
                    newTilesInNewExcavation.Enqueue(neighbour);
                }
            }
            newExcavation.Add(currentTile);
        }
        return newExcavation;
    }

    private void Update()
    {
        print(excavations.Count);
    }
}

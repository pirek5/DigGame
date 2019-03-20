using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DigManager : MonoBehaviour
{
    //separate areas which contains tiles to dig
    private List<Excavation> excavations = new List<Excavation>();

    //singleton
    public static DigManager Instance { get; private set; }

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

    public void AddTileToDig(Tile tileToDig)
    {
        List<Excavation> excavationNeighbors = new List<Excavation>();
        foreach(Excavation excavation in excavations)
        {
            foreach(Tile tileInExcavation in excavation.TilesInExcavation)
            {
                foreach(Tile tileToDigNeighbor in tileToDig.Neighbors)
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
            neighbourExcavations[0].TilesInExcavation.AddRange(neighbourExcavations[i].TilesInExcavation);
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
            if (excavation.TilesInExcavation.Contains(tileToDelete))
            {
                excavation.DeleteTileInExcavation(tileToDelete);
                if(excavation.TilesInExcavation.Count == 0)
                {
                    excavations.Remove(excavation);
                }
                CheckIntegrity(excavation, tileToDelete);
                return; //excavation found, no need to continue function
            }
        }
    }

    public List<Tile> FindPossibleEnternace(Tile tile)
    {
        foreach (Excavation excavation in excavations)
        {
            if (excavation.TilesInExcavation.Contains(tile))
            {
                return excavation.ExcavationEnternace;
            }
        }
        return null;
    }

    public Excavation GetExcavation(Tile tile)
    {
        foreach (Excavation excavation in excavations)
        {
            if (excavation.TilesInExcavation.Contains(tile))
            {
                return excavation;
            }
        }
        return null;
    }

    public void CheckIntegrity(Excavation excavation, Tile delatedTile)
    {
        List<Tile> neighboursToDelatedTile = new List<Tile>();
        foreach(Tile neighbour in delatedTile.Neighbors)
        {
            if (excavation.TilesInExcavation.Contains(neighbour))
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
                    var tilesInNewExcavation = BuildExcavation(tile, excavation.TilesInExcavation);
                    newExcavations.Add(tilesInNewExcavation);
                }
            }
            for(int i = 0; i < newExcavations.Count; i++)
            {
                if (i == 0) //first newExcavation replace old excavation
                {
                    excavation.TilesInExcavation.Clear();
                    excavation.TilesInExcavation.AddRange(newExcavations[i]);
                    excavation.UpdateDigEntrance();
                }
                else //add rest of new excavations
                {
                    Excavation excav = new Excavation(newExcavations[i]);
                    excavations.Add(excav);
                }
            }
        }
    }

    private List<Tile> BuildExcavation(Tile tile, List<Tile> tilesInExistingExcavation)
    {
        List<Tile> newExcavation = new List<Tile>();
        Queue<Tile> newTilesInNewExcavation = new Queue<Tile>();
        newTilesInNewExcavation.Enqueue(tile);
        while (newTilesInNewExcavation.Count > 0)
        {
            var currentTile = newTilesInNewExcavation.Dequeue();
            foreach(Tile neighbour in currentTile.Neighbors)
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
}

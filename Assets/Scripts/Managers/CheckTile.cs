using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CheckTile : MonoBehaviour
{
    public bool MarkToDig(Vector2Int tilePos)
    {
        Vector2Int upperTilePos = tilePos + Vector2Int.up;
        if (!GridData.GridDictionary.ContainsKey(tilePos) || !GridData.GridDictionary.ContainsKey(upperTilePos)) { return false; }

        Tile upperTile = GridData.GridDictionary[upperTilePos];
        Tile tile = GridData.GridDictionary[tilePos];
        return (upperTile.InfrastructureType == InfrastructureType.substructure || upperTile.BuildingOnTile 
                || tile.DigIt == true || tile.TileType != TileType.full)? false : true;
    }

    public bool BuildPipe(Vector2Int tilePos)
    {
        if(!GridData.GridDictionary.ContainsKey(tilePos)) { return false; }

        Tile tile = GridData.GridDictionary[tilePos];
        return (tile.InfrastructureToBuild != InfrastructureType.empty || tile.InfrastructureType != InfrastructureType.empty || tile.TileType != TileType.empty) ? false : true;
    }

    public bool BuildSubstructure(Vector2Int tilePos)
    {
        Vector2Int lowerTilePos = tilePos + Vector2Int.down;
        if(!GridData.GridDictionary.ContainsKey(tilePos) || !GridData.GridDictionary.ContainsKey(lowerTilePos)) { return false; }

        Tile lowerTile = GridData.GridDictionary[lowerTilePos];
        Tile tile = GridData.GridDictionary[tilePos];
        return (tile.InfrastructureToBuild != InfrastructureType.empty || tile.InfrastructureType != InfrastructureType.empty || tile.TileType != TileType.empty || lowerTile.TileType != TileType.full) ? false : true;
    }

    public bool CheckPlaceToBuild(List<Vector2Int> tilesPos)
    {
        var lowerTilePos = tilesPos[0].y;
        foreach(var tilePos in tilesPos)
        {
            if(tilePos.y < lowerTilePos)
            {
                lowerTilePos = tilePos.y;
            }
        }
        print("lower tile Pos: " + lowerTilePos);

        foreach (var tilePos in tilesPos)
        {
            print("tile pos y " + tilePos.y);
            if (!GridData.GridDictionary.ContainsKey(tilePos)) { return false; }
            if (GridData.GridDictionary[tilePos].TileType != TileType.empty) { return false; }
            if (GridData.GridDictionary[tilePos].BuildingOnTile) { return false; }
            if (GridData.GridDictionary[tilePos].InfrastructureType != InfrastructureType.substructure && tilePos.y == lowerTilePos) { return false; }
        }
        return true;
    }

    public List<Tile> GetConnectedTiles(Tile tile)
    {
        List<Tile> connectedTiles = new List<Tile>();
        foreach(var neighbour in tile.Neighbors)
        {
            print("cheching neighbours");
            if(tile.InfrastructureType == InfrastructureType.substructure && neighbour.InfrastructureType == InfrastructureType.pipe ||
               tile.InfrastructureType == InfrastructureType.pipe && neighbour.InfrastructureType == InfrastructureType.substructure)
            {
                connectedTiles.Add(neighbour);
                if (!connectedTiles.Contains(tile))
                {
                    connectedTiles.Add(tile);
                }
            }
        }
        foreach(var tileb in connectedTiles) { print(tileb.Position); }

        return connectedTiles;
    }
}

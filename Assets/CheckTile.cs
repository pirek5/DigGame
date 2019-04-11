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
}

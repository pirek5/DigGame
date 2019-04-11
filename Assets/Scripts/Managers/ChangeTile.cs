using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ChangeTile : MonoBehaviour
{
    //depedencies
    [Inject] MapDisplay mapDisplay;
    [Inject] DigManager digManager;
    [Inject] InfrastructureBuildManager infrastructureBM;
    [Inject] CheckTile checkTile;

    public void MarkTileToDig(Vector2Int tilePosition)
    {
        if (checkTile.MarkToDig(tilePosition))
        {
            Tile tile = GridData.GridDictionary[tilePosition];
            tile.DigIt = true;
            digManager.MarkTileToDig(tile);
            mapDisplay.DisplayTile(tile);
            
            Tile upperTile = GridData.GridDictionary[tilePosition + Vector2Int.up];
            if(upperTile.InfrastructureToBuild == InfrastructureType.substructure)
            {
                upperTile.InfrastructureToBuild = InfrastructureType.empty;
                infrastructureBM.EraseTileToBuild(upperTile);
                mapDisplay.DisplayTile(upperTile);
            }
        }
    }

    public void MarkTileAsInfrastructureToBuild(Vector2Int tilePosition, InfrastructureType infrastructureType)
    {
        if(infrastructureType == InfrastructureType.pipe && checkTile.BuildPipe(tilePosition))
        {
            Tile tile = GridData.GridDictionary[tilePosition];
            tile.InfrastructureToBuild = infrastructureType;
            infrastructureBM.MarkTileToBuild(tile);
            mapDisplay.DisplayTile(tile);
        }
        else if(infrastructureType == InfrastructureType.substructure && checkTile.BuildSubstructure(tilePosition))
        {
            Tile tile = GridData.GridDictionary[tilePosition];
            tile.InfrastructureToBuild = infrastructureType;
            
            infrastructureBM.MarkTileToBuild(tile);
            mapDisplay.DisplayTile(tile);
   
            Tile lowerTile = GridData.GridDictionary[tilePosition + Vector2Int.down];
            if(lowerTile.DigIt == true)
            {
                lowerTile.DigIt = false;
                digManager.EraseTileToDig(lowerTile);
                mapDisplay.DisplayTile(lowerTile);
            }
        }
    }

    public void EraseMark(Vector2Int tilePosition)
    {
        if (GridData.GridDictionary.ContainsKey(tilePosition))
        {
            Tile tile = GridData.GridDictionary[tilePosition];
            if (tile.DigIt == true)
            {
                tile.DigIt = false;
                digManager.EraseTileToDig(tile);
                mapDisplay.DisplayTile(tile);
            }
            else if (tile.InfrastructureToBuild != InfrastructureType.empty)
            {
                tile.InfrastructureToBuild = InfrastructureType.empty;
                infrastructureBM.EraseTileToBuild(tile);
                mapDisplay.DisplayTile(tile);
            }
        }
    }

    public void TileDigged(Tile tile)
    {
        if (GridData.GridDictionary.ContainsValue(tile))
        {
            tile.TileType = TileType.empty;
            tile.DigIt = false;
            mapDisplay.DisplayTile(tile);
            digManager.EraseTileToDig(tile);
        }
    }

    public void TileWithInfrastructure(Tile tile)
    {
        if (GridData.GridDictionary.ContainsValue(tile))
        {
            tile.InfrastructureType = tile.InfrastructureToBuild;
            tile.InfrastructureToBuild = InfrastructureType.empty;
            mapDisplay.DisplayTile(tile);
            infrastructureBM.EraseTileToBuild(tile);

            foreach(var connectedTile in checkTile.GetConnectedTiles(tile))
            {
                TileWithConnection(connectedTile);
            } 
        }
    }

    public void TileWithConnection(Tile tile)
    {
        tile.PipeConnection = true;
        mapDisplay.DisplayTile(tile);
    }

    public void TileWithBuilding(List<Vector2Int> tilesPos, GameObject building)
    {
        foreach (var tilePos in tilesPos)
        {
            if (!GridData.GridDictionary.ContainsKey(tilePos)) { continue; }
            GridData.GridDictionary[tilePos].BuildingOnTile = building;
        }
    }
}

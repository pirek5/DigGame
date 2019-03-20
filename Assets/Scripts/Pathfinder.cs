using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    public List<Tile> FindPath(Tile endTile)
    {
        PriorityQueue frontierTiles = new PriorityQueue();
        List<Tile> exploredTiles = new List<Tile>();
        Tile startTile = FindStartTile(transform.position);
        if(startTile == null)
        {
            //path not found - start tile is impossible to find
            return null;
        }
        startTile.DistanceTraveled = 0f;
        frontierTiles.Enqueue(startTile);
        while(frontierTiles.Count > 0)
        {
            Tile currentTile = frontierTiles.Dequeue();
            if (currentTile == endTile)
            {
                //path found
                return CreatePath(startTile, currentTile); 
            }
            foreach (var neighbor in currentTile.Neighbors)
            {
                if(!frontierTiles.Contains(neighbor) && !exploredTiles.Contains(neighbor) && neighbor.TileType == TileType.empty)
                {
                    neighbor.ExploredFrom = currentTile;
                    float newDistanceTraveled = currentTile.DistanceTraveled + 1f;
                    neighbor.DistanceTraveled = newDistanceTraveled;
                    neighbor.Priority = newDistanceTraveled + GetDistance(neighbor, endTile);
                    frontierTiles.Enqueue(neighbor);
                }
            }
            exploredTiles.Add(currentTile);
        }
        //path not found - end tile is impossible to reach
        return null;
    }

    private Tile FindStartTile(Vector3 position)
    {
        Vector2Int positionInt = Vector2Int.FloorToInt(position);
        List<Tile> possibleClosestTiles = new List<Tile>();
        if (GridData.gridDictionary.ContainsKey(positionInt))
        {
            Tile tile = GridData.gridDictionary[positionInt];
            possibleClosestTiles.Add(tile);
            possibleClosestTiles.AddRange(tile.Neighbors);
        }
        else //much slower 'emergency' way
        {
            possibleClosestTiles = new List<Tile>(GridData.gridDictionary.Values);
            Debug.LogError("position out of map!");
        }
        return Utilities.FindClosestTile(position, possibleClosestTiles);
    }

    private List<Tile> CreatePath(Tile startTile, Tile endTile)
    {
        List<Tile> path = new List<Tile>();
        path.Add(endTile);
        Tile currentTile = endTile;
        while (currentTile != startTile)
        {
            path.Add(currentTile.ExploredFrom);
            currentTile = currentTile.ExploredFrom;
        }
        path.Reverse();
        return path;
    }

    private float GetDistance(Tile source, Tile target)
    {
        float dx = Mathf.Abs(source.Position.x - target.Position.x);
        float dy = Mathf.Abs(source.Position.y - target.Position.y);
        return (dx + dy);
    }

}

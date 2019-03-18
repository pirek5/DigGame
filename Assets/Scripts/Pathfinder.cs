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
            return null;
        }
        startTile.distanceTraveled = 0f;
        frontierTiles.Enqueue(startTile);
        while(frontierTiles.Count > 0)
        {
            Tile currentTile = frontierTiles.Dequeue();
            if (currentTile == endTile)
            {
                //path found
                return CreatePath(startTile, currentTile); 
            }
            foreach (var neighbor in currentTile.neighbors)
            {
                if(!frontierTiles.Contains(neighbor) && !exploredTiles.Contains(neighbor) && neighbor.m_tileType == TileType.empty)
                {
                    neighbor.exploredFrom = currentTile;
                    float newDistanceTraveled = currentTile.distanceTraveled + 1f;
                    neighbor.distanceTraveled = newDistanceTraveled;
                    neighbor.priority = newDistanceTraveled + GetDistance(neighbor, endTile);
                    frontierTiles.Enqueue(neighbor);
                }
            }
            exploredTiles.Add(currentTile);
        }
        //path not found - end tile is impossible to reach
        return null;
    }

    public static Tile FindStartTile(Vector3 position)
    {
        Vector2Int positionInt = Vector2Int.FloorToInt(position);
        List<Tile> possibleClosestTiles = new List<Tile>();
        if (GridData.gridDictionary.ContainsKey(positionInt))
        {
            Tile tile = GridData.gridDictionary[positionInt];
            possibleClosestTiles.Add(tile);
            possibleClosestTiles.AddRange(tile.neighbors);
        }
        else //much slower 'emergency' way
        {
            possibleClosestTiles = new List<Tile>(GridData.gridDictionary.Values);
            Debug.LogError("position out of map!");
        }
        return Utilities.TileFindClosestTile(position, possibleClosestTiles);
    }

    public List<Tile> CreatePath(Tile startTile, Tile endTile)
    {
        List<Tile> path = new List<Tile>();
        path.Add(endTile);
        Tile currentTile = endTile;
        while (currentTile != startTile)
        {
            path.Add(currentTile.exploredFrom);
            currentTile = currentTile.exploredFrom;
        }
        path.Reverse();
        return path;
    }

    public static float GetDistance(Tile source, Tile target)
    {
        float dx = Mathf.Abs(source.position.x - target.position.x);
        float dy = Mathf.Abs(source.position.y - target.position.y);

        return (dx + dy);
    }

}

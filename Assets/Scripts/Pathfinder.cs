using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    public List<Tile> FindPath(Tile startTile, Tile endTile)
    {
        PriorityQueue frontierTiles = new PriorityQueue();
        List<Tile> exploredTiles = new List<Tile>();
        startTile.distanceTraveled = 0f;
        frontierTiles.Enqueue(startTile);
        while(frontierTiles.Count > 0)
        {
            Tile currentTile = frontierTiles.Dequeue();
            if(currentTile == endTile)
            {
                return CreatePath(startTile, currentTile); //right way of exit the loop
            }
            foreach (var neighbor in currentTile.neighbors)
            {
                if(!frontierTiles.Contains(neighbor) && exploredTiles.Contains(neighbor) && neighbor.m_tileType == TileType.empty)
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
        //shouldnt happen - in case of 'emergency'
        List<Tile> emergencyPath = new List<Tile>() { endTile };
        return emergencyPath;
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

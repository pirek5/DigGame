using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType { empty = 0, full = 1 };

public class Tile
{
    //state
    public TileType TileType { get; set; }
    public bool digIt { get; set; }
    public int Health { get; set; }

    //used to pathfinding
    public List<Tile> Neighbors { get; set; }
    public Vector3 Position { get; set; }
    public float DistanceTraveled { get; set; }
    public float Priority { get; set; }
    public Tile ExploredFrom { get; set; }

    //constructor
    public Tile(TileType tileType, int health) 
    {
        TileType = tileType;
        Health = health;
        Neighbors = new List<Tile>();
    }

    public void LoseHealth()
    {
        Health--;
    }

    //used to pathfinding
    public int CompareTo(Tile other)
    {
        if (this.Priority < other.Priority)
        {
            return -1;
        }
        else if (this.Priority > other.Priority)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
}

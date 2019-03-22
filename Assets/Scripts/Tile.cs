using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType { empty = 0, full = 1 };

public class Tile
{
    //state
    public TileType TileType { get; set; }
    public bool DigIt { get; set; }
    public bool InfrastructureToBuild { get; set; }
    public bool HasInfrastructure { get; set; }
    public int Health { get; set; }
    public int buildProgress { get; set; }
    

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
        buildProgress = 0;
        Neighbors = new List<Tile>();
        DigIt = false;
        InfrastructureToBuild = false;
        HasInfrastructure = false;
    }

    public void LoseHealth()
    {
        Health--;
    }

    public void BuildingInfrastructure()
    {
        buildProgress++;
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

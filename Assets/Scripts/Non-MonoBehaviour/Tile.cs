using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType { empty = 0, full = 1 };

[System.Serializable]
public enum InfrastructureType { empty, substructure, pipe }

public class Tile
{
    //state
    public TileType TileType { get; set; }
    public bool DigIt { get; set; }
    public bool PipeConnection { get; set; }
    public InfrastructureType InfrastructureToBuild { get; set; }
    public InfrastructureType InfrastructureType { get; set; }
    public GameObject BuildingOnTile { get; set; }
    public int Health { get; set; }
    public int InfrastructureBuildProgress { get; set; }
    

    //used to pathfinding
    public List<Tile> Neighbors { get; set; }
    public Vector3 Position { get; set; }
    public float DistanceTraveled { get; set; }
    public float Priority { get; set; }
    public Tile ExploredFrom { get; set; }

    //constructor
    public Tile(TileType tileType, int health) 
    {
        InfrastructureType = InfrastructureType.empty;
        InfrastructureToBuild = InfrastructureType.empty;
        TileType = tileType;
        Health = health;
        InfrastructureBuildProgress = 0;
        Neighbors = new List<Tile>();
    }

    public void LoseHealth()
    {
        Health--;
    }

    public void BuildingInfrastructure()
    {
        InfrastructureBuildProgress++;
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

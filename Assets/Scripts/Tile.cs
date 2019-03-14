using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType { full = 0, empty = 1 };

public class Tile
{
    //state
    public TileType m_tileType;
    public bool digIt = false;

    //used to pathfinding
    public List<Tile> neighbors = new List<Tile>();
    public Vector3 position;
    public float distanceTraveled;
    public float priority;
    public Tile exploredFrom;
   
    //constructor
    public Tile(TileType tileType) 
    {
        m_tileType = tileType;
    }

    //used to pathfinding
    public int CompareTo(Tile other)
    {
        if (this.priority < other.priority)
        {
            return -1;
        }
        else if (this.priority > other.priority)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
}

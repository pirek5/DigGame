using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType { full = 0, empty = 1 };

public class TileInfo
{
    public TileType m_tileType;
    public bool digIt = false;
   

    public TileInfo(TileType tileType)
    {
        m_tileType = tileType;
    }
}

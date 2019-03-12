using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType { full = 0, empty = 1 };

public class TileInfo
{
    TileType m_tileType;
    //int m_posX, m_posY;

    public TileInfo(TileType tileType/*, int posX, int posY*/)
    {
       // m_posX = posX;
       // m_posY = posY;
        m_tileType = tileType;
    }
}

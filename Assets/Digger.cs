using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Digger : MonoBehaviour
{
    //cached
    Movement movement;

    public void Awake()
    {
        movement = GetComponent<Movement>();
    }

    public void MoveOrDig(Vector2Int gridPos)
    {
        if (GridData.gridDictionary.ContainsKey(gridPos))
        {
            if (GridData.gridDictionary[gridPos].m_tileType == TileType.empty)
            {
                movement.FindAndFollowPath(GridData.gridDictionary[gridPos]);
            }
        }
    }
}

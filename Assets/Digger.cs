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
            movement.FindAndFollowPath(GridData.gridDictionary[gridPos]);
        }
    }
}

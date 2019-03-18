using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiggerMovement : Movement
{
    Digger digger;

    protected override void Awake()
    {
        base.Awake();
        digger = GetComponent<Digger>();
    }

    protected override IEnumerator FollowPath(List<Tile> path)
    {
        yield return StartCoroutine(base.FollowPath(path));
        if (digger.digging)
        {
            digger.StartDigging();
        }
    }
}

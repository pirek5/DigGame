﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Digger : MonoBehaviour
{
    //config
    [SerializeField] private float diggingPeriod = 1f;
    [SerializeField] private int autoDiggingRange = 10;

    //state
    public bool Digging { get; set; }

    //dependencies
    [Inject] DigManager digManager;
    private DiggerMovement movement;

    [Inject] private ChangeTile changeTile;

    public void Awake()
    {
        movement = GetComponent<DiggerMovement>();
    }

    public void StartDigging()
    {
        StartCoroutine(DiggingCoroutine());
    }

    IEnumerator DiggingCoroutine()
    {
        if(digManager.TilesToDig.Count <= 0)
        {
            yield break;
        }

        var closestTileToDig = Utilities.FindClosestTile(transform.position, digManager.TilesToDig);
        if(Utilities.CheckIfNeighbour(transform.position, closestTileToDig))
        {
            GetComponent<FlashingObject>().StartFlashing();
            while (closestTileToDig.DigIt == true)
            {
                closestTileToDig.LoseHealth();
                if(closestTileToDig.Health <= 0)
                {
                    changeTile.TileDigged(closestTileToDig);
                }
                yield return new WaitForSeconds(diggingPeriod);
            }
            GetComponent<FlashingObject>().StopFlashing();

            if(digManager.TilesToDig.Count <= 0 || movement.CurrentTile == null)
            {
                yield break;
            }

            Tile newTileToDig = Utilities.FindClosestTile(transform.position, digManager.TilesToDig);
            if (Utilities.GetDistance(movement.CurrentTile, newTileToDig) <= autoDiggingRange)
            {
                movement.MoveToPosition(newTileToDig);
            }
        }
    }

    public void StopDigging()
    {
        Digging = false;
        GetComponent<FlashingObject>().StopFlashing();
        StopAllCoroutines();
    }
}

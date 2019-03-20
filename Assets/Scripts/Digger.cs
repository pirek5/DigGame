using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Digger : MonoBehaviour
{
    //config
    [SerializeField] private float diggingPeriod = 1f;

    //state
    public bool Digging { get; set; }
    public Excavation CurrentExcavation { get; set; }

    //cached
    private DiggerMovement movement;

    public void Awake()
    {
        movement = GetComponent<DiggerMovement>();
    }

    public void StartDigging()
    {
        if(CurrentExcavation == null)
        {
            SearchForNewExcavation();
        }
        else if(CurrentExcavation.TilesInExcavation.Count > 0)
        {
            if(CurrentExcavation != null)
            {
                StartCoroutine(DiggingCoroutine());
            }
        }
    }

    IEnumerator DiggingCoroutine()
    {
        var closestTileToDig = Utilities.FindClosestTile(transform.position, CurrentExcavation.TilesInExcavation);
        if(Utilities.CheckIfNeighbour(transform.position, closestTileToDig))
        {
            GetComponent<FlashingObject>().StartFlashing();
            while (closestTileToDig.DigIt == true)
            {
                closestTileToDig.LoseHealth();
                if(closestTileToDig.Health <= 0)
                {
                    GridData.Instance.DeleteTile(closestTileToDig);
                }
                yield return new WaitForSeconds(diggingPeriod);
            }
            GetComponent<FlashingObject>().StopFlashing();

            if (CurrentExcavation != null)
            {
                if(CurrentExcavation.TilesInExcavation.Count > 0)
                {
                    Tile newTileToDig = Utilities.FindClosestTile(transform.position, CurrentExcavation.TilesInExcavation);
                    movement.MoveToPosition(newTileToDig);
                }
            }
        }
        else
        {
            SearchForNewExcavation();
        }
        
    }

    private void SearchForNewExcavation()
    {
        print("search");
        if(movement.CurrentTile == null){ return; }

        foreach (var neighbour in movement.CurrentTile.Neighbors)
        {
            if(neighbour.DigIt)
            {
                CurrentExcavation = DigManager.Instance.GetExcavation(neighbour);
                StartDigging();
                return; // new excavation found, no need to continue function
            }
        }
        Digging = false;
        CurrentExcavation = null;
    }

}

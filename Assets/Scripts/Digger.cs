using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Digger : MonoBehaviour
{
    //config
    [SerializeField] private float diggingPeriod = 1f;

    //state
    public bool digging = false;
    public Excavation currentExcavation;
    public Tile tileToDig;

    //cached
    DiggerMovement movement;
    DigController digController;
    GridData gridData;

    public void Awake()
    {
        movement = GetComponent<DiggerMovement>();
        digController = FindObjectOfType<DigController>(); //TODO
        gridData = FindObjectOfType<GridData>();
    }

    public void StartDigging()
    {
        if(tileToDig == null)
        {
            digging = false;
            return;
        }
        else if(tileToDig.digIt == true)
        {
            currentExcavation = digController.GetExcavation(tileToDig);
            if(currentExcavation != null)
            {
                StartCoroutine(DiggingCoroutine());
            }
        }
    }

    IEnumerator DiggingCoroutine()
    {
        var closestTileToDig = Utilities.FindClosestTile(transform.position, currentExcavation.tilesInExcavation);
        print(Utilities.CheckIfNeighbour(transform.position, closestTileToDig));
        print(closestTileToDig.position);
        if(Utilities.CheckIfNeighbour(transform.position, closestTileToDig))
        {
            GetComponent<FlashingObject>().StartFlashing();
            while (closestTileToDig.digIt == true)
            {
                closestTileToDig.LoseHealth();
                if(closestTileToDig.health <= 0)
                {
                    gridData.DeleteTile(closestTileToDig);
                }
                yield return new WaitForSeconds(diggingPeriod);
            }
            GetComponent<FlashingObject>().StopFlashing();

            if (currentExcavation != null)
            {
                if(currentExcavation.tilesInExcavation.Count > 0)
                {
                    Tile newTileToDig = Utilities.FindClosestTile(transform.position, currentExcavation.tilesInExcavation);
                    movement.MoveToPosition(newTileToDig);
                }
                else
                {
                    currentExcavation = null;
                }
            }
        }
        // wyszukanie nowej sciezki?
        
    }


    private void Update()
    {
        if(tileToDig != null)
        {
            //print("tileToDig: " + tileToDig.position);
        }
        if(currentExcavation != null)
        {
            //print("currentExcavation: " + currentExcavation);
        }
        
    }

}

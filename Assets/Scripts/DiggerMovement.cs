using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiggerMovement : Movement
{
    //cached
    private Digger digger;
    private DigController digController;

    protected override void Awake()
    {
        base.Awake();
        digger = GetComponent<Digger>();
        digController = FindObjectOfType<DigController>(); //TODO
    }

    public override void MoveToPosition(Vector2Int position)
    {
        digger.tileToDig = null;
        digger.currentExcavation = null;
        base.MoveToPosition(position); //move to specific tile or...
        if (GridData.gridDictionary[position].digIt == true) // ... move and dig
        {
            digger.tileToDig = GridData.gridDictionary[position];
            //tilesToDig = digController.GetTilesToDig(tileToDig);
            List<Tile> possibleEntrance = digController.FindPossibleEnternace(digger.tileToDig);

            if (possibleEntrance.Count > 0)
            {
                FindDigEntrance(possibleEntrance);
            }
            else
            {
                //tilesToDig.Clear();
                //nie da sie znalezc sciezki
            }
        }
    }

    public void FindDigEntrance(List<Tile> possibleDestinations)  //TODO move to new diggerPathfinder class?
    {
        Tile closestEnternance = Utilities.FindClosestTile(this.transform.position, possibleDestinations);
        if (FindAndFollowPath(closestEnternance)) //path founded
        {
            digger.digging = true;
        }
        else // coudnt find path, change entarnace to second closest
        {
            possibleDestinations.Remove(closestEnternance);
            if (possibleDestinations.Count > 0)
            {
                FindDigEntrance(possibleDestinations);
            }
            else
            {
               // tilesToDig.Clear();
                //nie da sie znalezx sciezki
            }
        }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiggerMovement : Movement
{
    //cached
    private Digger digger;

    protected override void Awake()
    {
        base.Awake();
        digger = GetComponent<Digger>();
    }

    public override void MoveToPosition(Vector2Int position)
    {
        digger.TileToDig = null; //TODO
        digger.CurrentExcavation = null; //TODO
        base.MoveToPosition(position); //move to specific tile or...
        if (GridData.gridDictionary[position].digIt == true) // ... move and dig
        {
            digger.TileToDig = GridData.gridDictionary[position];
            List<Tile> possibleEntrance = DigManager.Instance.FindPossibleEnternace(digger.TileToDig);

            if (possibleEntrance.Count > 0)
            {
                FindDigEntrance(possibleEntrance);
            }
            else
            {
                //nie da sie znalezc sciezki
            }
        }
    }

    public void FindDigEntrance(List<Tile> possibleDestinations)
    {
        Tile closestEnternance = Utilities.FindClosestTile(this.transform.position, possibleDestinations);
        if (FindAndFollowPath(closestEnternance)) //path founded
        {
            digger.Digging = true;
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
                //nie da sie znalezx sciezki
            }
        }
    }

    protected override IEnumerator FollowPath(List<Tile> path)
    {
        yield return StartCoroutine(base.FollowPath(path));
        if (digger.Digging)
        {
            digger.StartDigging();
        }
    }
}

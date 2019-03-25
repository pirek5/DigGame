using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DiggerMovement : Movement
{
    //dependencies
    private Digger digger;
    [Inject] private DigManager digManager;

    protected override void Awake()
    {
        base.Awake();
        digger = GetComponent<Digger>();
    }

    public override void MoveToPosition(Tile destinationTile)
    {
        digger.StopDigging();
        base.MoveToPosition(destinationTile); //move to specific tile or...
        if (destinationTile.DigIt == true) // ... move and dig
        {
            List<Tile> possibleEntrance = digManager.GetPossibleEnternance(destinationTile);

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

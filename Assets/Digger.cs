using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Digger : MonoBehaviour
{
    //cached
    Movement movement;
    DigController digController;

    public void Awake()
    {
        movement = GetComponent<Movement>();
        digController = FindObjectOfType<DigController>(); //TODO
    }

    public void MoveOrDig(Vector2Int gridPos)
    {
        if (GridData.gridDictionary.ContainsKey(gridPos))
        {
            if (GridData.gridDictionary[gridPos].m_tileType == TileType.empty)
            {
                movement.FindAndFollowPath(GridData.gridDictionary[gridPos]); // TODO zaznacz nieudane wyszukanie ściezki FIndAndFollowPath to bool
            }
            else if(GridData.gridDictionary[gridPos].digIt == true)
            {
                List<Tile> possibleDestinations = digController.FindEnternace(GridData.gridDictionary[gridPos]);
                if(possibleDestinations.Count > 0)
                {
                    FindDestination(possibleDestinations);
                }
                else
                {
                    //nie da sie znalezc sciezki
                }
                
            }
        }
    }

    public void FindDestination(List<Tile> possibleDestinations)
    {
        Tile closestEnternance = Utilities.TileFindClosestTile(this.transform.position, possibleDestinations);
        if (!movement.FindAndFollowPath(closestEnternance)) 
        {
            possibleDestinations.Remove(closestEnternance); // coudnt find path, change entarnace to second closest
            if (possibleDestinations.Count > 0)
            {
                FindDestination(possibleDestinations);
            }
            else
            {
                //nie da sie znalezx sciezki
            }
        }
    }
}

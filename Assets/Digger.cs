using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Digger : MonoBehaviour
{
    //config
    [SerializeField] private float diggingPeriod = 1f;

    //state
    public bool digging = false;
    List<Tile> tilesToDig;
    Tile tileToDig;

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

    public void Move(Vector2Int gridPos)
    {
        if (GridData.gridDictionary.ContainsKey(gridPos))
        {
            if (GridData.gridDictionary[gridPos].m_tileType == TileType.empty) // move to specific tile
            {
                movement.FindAndFollowPath(GridData.gridDictionary[gridPos]); // TODO zaznacz nieudane wyszukanie ściezki FIndAndFollowPath to bool
                digging = false;
            }
            else if(GridData.gridDictionary[gridPos].digIt == true) // move and dig
            {
                tileToDig = GridData.gridDictionary[gridPos];
                tilesToDig = digController.GetTilesToDig(tileToDig);
                List<Tile> possibleDestinations = digController.FindEnternace(tileToDig);

                if(possibleDestinations.Count > 0)
                {
                    FindDestination(possibleDestinations);
                }
                else
                {
                    tilesToDig.Clear();
                    //nie da sie znalezc sciezki
                }
                
            }
        }
    }

    public void FindDestination(List<Tile> possibleDestinations)  //TODO move to new diggerPathfinder class?
    {
        Tile closestEnternance = Utilities.TileFindClosestTile(this.transform.position, possibleDestinations);
        if (movement.FindAndFollowPath(closestEnternance)) //path founded
        {
            digging = true;
        }
        else // coudnt find path, change entarnace to second closest
        {
            possibleDestinations.Remove(closestEnternance); 
            if (possibleDestinations.Count > 0)
            {
                FindDestination(possibleDestinations);
            }
            else
            {
                tilesToDig.Clear();
                //nie da sie znalezx sciezki
            }
        }
    }

    public void StartDigging()
    {
        GetComponent<FlashingObject>().StartFlashing();
    }

}

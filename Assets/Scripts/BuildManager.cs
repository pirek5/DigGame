using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BuildManager : MonoBehaviour
{
    //set in editor
    [SerializeField] private string preBuildSortingLayer;
    [SerializeField] private string deffaultSortingLayer;

    [SerializeField] private Color preBuildColorPositive;
    [SerializeField] private Color preBuildColorNegative;

    //state
    private GameObject currentBuilding;
    private ConstructionPlan currentConstructionPlan;
    private SpriteRenderer currentSR;
    private List<Vector2Int> currentBuldingTiles = new List<Vector2Int>();
    private Vector2 offset;
    private bool canPlaceBuilding;

    //dependencies
    [Inject] private GridData gridData;
    [Inject] private PlayerInput playerInput;

    void Update()
    {
        if (currentConstructionPlan != null && currentSR != null || currentBuilding != null)
        {
            currentBuilding.transform.position = playerInput.MousePos2D + offset;
            currentBuldingTiles = AssignPosition(currentConstructionPlan.BuildingTiles);
            if (gridData.CheckPlaceToBuild(currentBuldingTiles))
            {
                canPlaceBuilding = true;
                currentSR.color = preBuildColorPositive;
            }
            else
            {
                canPlaceBuilding = false;
                currentSR.color = preBuildColorNegative;
            }
        }
    }

    public void TryBuild(GameObject building)
    {
        if (!building.GetComponent<ConstructionPlan>() || !building.GetComponentInChildren<SpriteRenderer>()) { return; }

        playerInput.CurrentState = State.build;
        currentBuilding = building;
        currentSR = currentBuilding.GetComponentInChildren<SpriteRenderer>();
        currentConstructionPlan = currentBuilding.GetComponent<ConstructionPlan>();
        offset = currentConstructionPlan.offset;

        currentSR.sortingLayerName = preBuildSortingLayer;
    }

    List<Vector2Int> AssignPosition(List<Vector2Int> buildingTiles)
    {
        List<Vector2Int> currentBuildingTiles = new List<Vector2Int>();
        foreach(var position in buildingTiles)
        {
            currentBuildingTiles.Add(position + Vector2Int.RoundToInt(playerInput.MousePos2D - offset));
        }
        return currentBuildingTiles;
    }

    public void PlaceBuilding()
    {
        if(!canPlaceBuilding) { return; } //TODO zasygnalizować że się nie da zbudować budynku

        gridData.MarkTilesAsOccupiedByBulding(currentBuldingTiles);

        currentSR.color = Color.white;
        currentSR.sortingLayerName = deffaultSortingLayer;
        Instantiate(currentBuilding, currentBuilding.transform.position, Quaternion.identity);
        playerInput.CurrentState = State.normal;

        currentBuilding.SetActive(false);
        currentBuilding = null;
        currentSR = null;
        currentConstructionPlan = null;
        offset = Vector2.zero;
        currentBuldingTiles.Clear();
    }

    public void CancelBuild()
    {
        playerInput.CurrentState = State.normal;

        currentBuilding.SetActive(false);
        currentBuilding = null;
        currentSR = null;
        currentConstructionPlan = null;
        offset = Vector2.zero;
    }
}

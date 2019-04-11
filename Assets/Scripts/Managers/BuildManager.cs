using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BuildManager : MonoBehaviour
{
    //state
    private GameObject currentBuilding;
    private ConstructionPlan currentConstructionPlan;
    private BuildingDisplay currentBuildingDisplay;
    private List<Vector2Int> currentBuldingTiles = new List<Vector2Int>();
    private Vector2 offset;
    private bool canPlaceBuilding;

    //dependencies
    [Inject] private PlayerInput playerInput;
    [Inject] private ChangeTile changeTile;
    [Inject] private GridData gridData;

    void Update()
    {
        if (currentConstructionPlan != null || currentBuildingDisplay != null || currentBuilding != null)
        {
            currentBuilding.transform.position = playerInput.MousePos2D + offset;
            currentBuldingTiles = AssignPosition(currentConstructionPlan.BuildingTiles);
            canPlaceBuilding = gridData.CheckPlaceToBuild(currentBuldingTiles);
            currentBuildingDisplay.BuildingCanBePlaced(canPlaceBuilding);
        }
    }

    public void TryBuild(GameObject building)
    {
        if (!building.GetComponent<ConstructionPlan>() || !building.GetComponentInChildren<SpriteRenderer>() ||!building.GetComponent<BuildingDisplay>()) { return; }

        playerInput.CurrentState = State.build;
        currentBuilding = building;
        currentBuildingDisplay = currentBuilding.GetComponent<BuildingDisplay>();
        currentBuildingDisplay.Init();
        currentConstructionPlan = currentBuilding.GetComponent<ConstructionPlan>();
        offset = currentConstructionPlan.offset;
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
        
        currentBuildingDisplay.BuildingPlaced();
        GameObject newBuilding = Instantiate(currentBuilding, currentBuilding.transform.position, Quaternion.identity);
        changeTile.TileWithBuilding(currentBuldingTiles, newBuilding);
        playerInput.CurrentState = State.normal;
        ResetCurrentBuilding();
    }

    public void CancelBuild()
    {
        playerInput.CurrentState = State.normal;

        ResetCurrentBuilding();
    }

    private void ResetCurrentBuilding()
    {
        currentBuilding.SetActive(false);
        currentBuilding = null;
        currentBuildingDisplay = null;
        currentConstructionPlan = null;
        offset = Vector2.zero;
        currentBuldingTiles.Clear();
    }
}

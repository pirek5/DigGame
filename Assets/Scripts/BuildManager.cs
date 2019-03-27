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
    private Vector2 offset;
    private bool canPlaceBuilding;

    //dependencies
    [Inject] private GridData gridData;
    [Inject] private PlayerInput playerInput;

    // Update is called once per frame
    void Update()
    {
        if (currentConstructionPlan != null && currentSR != null || currentBuilding != null)
        {
            currentBuilding.transform.position = playerInput.MousePos2D + offset;
            var positions = AssignPosition(currentConstructionPlan.BuildingTiles);
            if (gridData.CheckPlaceToBuild(positions))
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
        currentBuilding = Instantiate(building);
        currentSR = currentBuilding.GetComponentInChildren<SpriteRenderer>();
        currentConstructionPlan = currentBuilding.GetComponent<ConstructionPlan>();
        offset = currentConstructionPlan.offset;

        currentSR.sortingLayerName = preBuildSortingLayer;
    }

    List<Vector2Int> AssignPosition(List<Vector2Int> buildingTiles)
    {
        print(Vector2Int.RoundToInt(playerInput.MousePos2D - offset));
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

        playerInput.CurrentState = State.normal;
        currentSR.color = Color.white;
        currentSR.sortingLayerName = deffaultSortingLayer;

        currentBuilding = null;
        currentSR = null;
        currentConstructionPlan = null;
        offset = Vector2.zero;
    }

    public void CancelBuild()
    {
        playerInput.CurrentState = State.normal;

        Destroy(currentBuilding);
        currentBuilding = null;
        currentSR = null;
        currentConstructionPlan = null;
        offset = Vector2.zero;
    }
}

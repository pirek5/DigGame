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

    //cached
    private GameObject currentBuilding;
    private ConstructionPlan currentConstructionPlan;
    private SpriteRenderer currentSR;

    //dependencies
    [Inject] private GridData gridData;
    [Inject] private PlayerInput playerInput;



    // Update is called once per frame
    void Update()
    {
        if (currentConstructionPlan != null && currentSR != null || currentBuilding != null)
        {
            currentBuilding.transform.position = playerInput.MousePos2D;
            var positions = AssignPosition(currentConstructionPlan.BuildingTiles);
            if (gridData.CheckPlaceToBuild(positions))
            {
                currentSR.color = preBuildColorPositive;
            }
            else
            {
                currentSR.color = preBuildColorNegative;
            }
        }
    }

    public void CheckIfPossibleToBuild(GameObject building)
    {
        if (!building.GetComponent<ConstructionPlan>() || !building.GetComponentInChildren<SpriteRenderer>()) { return; }
        currentBuilding = Instantiate(building);
        currentSR = currentBuilding.GetComponentInChildren<SpriteRenderer>();
        currentConstructionPlan = currentBuilding.GetComponent<ConstructionPlan>();
        
        currentSR.sortingLayerName = preBuildSortingLayer;
    }

    List<Vector2Int> AssignPosition(List<Vector2Int> buildingTiles)
    {
        List<Vector2Int> currentBuildingTiles = new List<Vector2Int>();
        foreach(var position in buildingTiles)
        {
            currentBuildingTiles.Add(position + Vector2Int.RoundToInt(playerInput.MousePos2D));
        }
        return currentBuildingTiles;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Builder : MonoBehaviour
{
    //config
    [SerializeField] private float buildPeriod = 1f;
    [SerializeField] private int autoBuildRange = 10;

    //state
    public bool InfrastructureToBuild { get; set; }
    public GameObject currentBuilding { get; set; }

    //dependencies
    private BuilderMovement movement;
    [Inject] InfrastructureBuildManager infrastructureBM;
    [Inject] private ChangeTile changeTile;


    public void Awake()
    {
        movement = GetComponent<BuilderMovement>();
    }

    public void BuildInfrastructure()
    {
        StartCoroutine(BuildInfrastructureCoroutine());
    }

    public void BuildOrRepair()
    { 
        if(currentBuilding == null) { return; }

        var buildingInfo = currentBuilding.GetComponent<BuildingInfo>();

        if (!buildingInfo.IsConstructed) //build
        {
            StartCoroutine(BuildCoroutine());
        }
        else if(buildingInfo.CurrentHealth < buildingInfo.MaxHealth) //repair
        {
            //do repair stuff
        }
        else //nothing to do
        {
            currentBuilding = null;
        }
    }

    private IEnumerator BuildCoroutine()
    {
        var constructionPlan = currentBuilding.GetComponent<ConstructionPlan>();
        var buildingInfo = currentBuilding.GetComponent<BuildingInfo>();

        GetComponent<FlashingObject>().StartFlashing();
        while (!buildingInfo.IsConstructed)
        {
            constructionPlan.Build();
            yield return new WaitForSeconds(buildPeriod);
        }
        GetComponent<FlashingObject>().StopFlashing();

        currentBuilding = null;
    }

    private IEnumerator BuildInfrastructureCoroutine()
    {
        if (infrastructureBM.TilesWithInfrastructureToBuild.Count <= 0 || movement.CurrentTile == null)
        {
            yield break;
        }

        var currentTileToBuild = movement.CurrentTile;

        if (currentTileToBuild.InfrastructureToBuild != InfrastructureType.empty)
        {
            GetComponent<FlashingObject>().StartFlashing();
            while(currentTileToBuild.InfrastructureToBuild != InfrastructureType.empty)
            {
                currentTileToBuild.BuildingInfrastructure();
                if(currentTileToBuild.InfrastructureBuildProgress >= infrastructureBM.infrastructureBuildTime)
                {
                    changeTile.TileWithInfrastructure(currentTileToBuild);
                }
                yield return new WaitForSeconds(buildPeriod);
            }
            GetComponent<FlashingObject>().StopFlashing();

            if (infrastructureBM.TilesWithInfrastructureToBuild.Count <= 0 || movement.CurrentTile == null)
            {
                yield break;
            }

            Tile newTileToBuild = Utilities.FindClosestTile(transform.position, infrastructureBM.TilesWithInfrastructureToBuild);
            if (Utilities.GetDistance(movement.CurrentTile, newTileToBuild) <= autoBuildRange)
            {
                movement.MoveToPosition(newTileToBuild);
            }
        }
    }

    public void StopBuild()
    {
        currentBuilding = null;
        InfrastructureToBuild = false;
        GetComponent<FlashingObject>().StopFlashing();
        StopAllCoroutines();
    }

}

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
    public bool Build { get; set; }

    //dependencies
    private BuilderMovement movement;
    [Inject] InfrastructureBuildManager infrastructureBM;
    [Inject] private GridData gridData;


    public void Awake()
    {
        movement = GetComponent<BuilderMovement>();
    }

    public void StartBuild()
    {
        StartCoroutine(BuildCoroutine());
    }

    private IEnumerator BuildCoroutine()
    {
        if (infrastructureBM.TilesWithInfrastructureToBuild.Count <= 0 || movement.CurrentTile == null)
        {
            yield break;
        }

        var currentTileToBuild = movement.CurrentTile;

        if (currentTileToBuild.InfrastructureToBuild)
        {
            GetComponent<FlashingObject>().StartFlashing();
            while(currentTileToBuild.InfrastructureToBuild == true)
            {
                currentTileToBuild.BuildingInfrastructure();
                if(currentTileToBuild.buildProgress >= infrastructureBM.infrastructureBuildTime)
                {
                    gridData.InfrastructureBuilt(currentTileToBuild);
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
        Build = false;
        GetComponent<FlashingObject>().StopFlashing();
        StopAllCoroutines();
    }

}

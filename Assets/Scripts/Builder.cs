using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
    //config
    [SerializeField] private float buildPeriod = 1f;
    [SerializeField] private int autoBuildRange = 10;

    //state
    public bool Build { get; set; }

    //cached
    private BuilderMovement movement;

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
        if (InfrastructureBuildManager.Instance.TilesWithInfrastructureToBuild.Count <= 0 || movement.CurrentTile == null)
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
                if(currentTileToBuild.buildProgress >= InfrastructureBuildManager.Instance.infrastructureBuildTime)
                {
                    GridData.Instance.InfrastructureBuilt(currentTileToBuild);
                }
                yield return new WaitForSeconds(buildPeriod);
            }
            GetComponent<FlashingObject>().StopFlashing();

            if (InfrastructureBuildManager.Instance.TilesWithInfrastructureToBuild.Count <= 0 || movement.CurrentTile == null)
            {
                yield break;
            }

            Tile newTileToBuild = Utilities.FindClosestTile(transform.position, InfrastructureBuildManager.Instance.TilesWithInfrastructureToBuild);
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

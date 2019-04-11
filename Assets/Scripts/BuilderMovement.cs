using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderMovement : Movement
{
    //cached
    Builder builder;

    protected override void Awake()
    {
        base.Awake();
        builder = GetComponent<Builder>();
    }

    public override void MoveToPosition(Tile destinationTile)
    {
        builder.StopBuild();
        base.MoveToPosition(destinationTile); //move to specific tile or...
        if (destinationTile.InfrastructureToBuild != InfrastructureType.empty) // ... move and build infrastructure or ...
        {
            builder.InfrastructureToBuild = true;
        }
        else if(destinationTile.BuildingOnTile) // move and build buildings
        {
            builder.currentBuilding = destinationTile.BuildingOnTile;
        }
    }

    protected override IEnumerator FollowPath(List<Tile> path)
    {
        yield return StartCoroutine(base.FollowPath(path));
        if (builder.InfrastructureToBuild)
        {
            builder.BuildInfrastructure();
        }
        else if (builder.currentBuilding)
        {
            builder.BuildOrRepair();
        }
    }

}

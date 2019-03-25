using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfrastructureBuildManager : MonoBehaviour
{
    public List<Tile> TilesWithInfrastructureToBuild { get; private set; }

    //config
    public int infrastructureBuildTime = 3; //TODO public field

    private void Awake()
    {
        TilesWithInfrastructureToBuild = new List<Tile>();
    }

    public void MarkTileToBuild(Tile tileToBuild)
    {
        if (!TilesWithInfrastructureToBuild.Contains(tileToBuild))
        {
            TilesWithInfrastructureToBuild.Add(tileToBuild);
        }
    }

    public void EraseTileToBuild(Tile tileToDelete)
    {
        if (TilesWithInfrastructureToBuild.Contains(tileToDelete))
        {
            TilesWithInfrastructureToBuild.Remove(tileToDelete);
        }
    }

}

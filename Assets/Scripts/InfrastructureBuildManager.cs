using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfrastructureBuildManager : MonoBehaviour
{
    public List<Tile> TilesWithInfrastructureToBuild { get; private set; }

    //config
    public int infrastructureBuildTime = 3; //TODO public field

    //singleton
    public static InfrastructureBuildManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            TilesWithInfrastructureToBuild = new List<Tile>();
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
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

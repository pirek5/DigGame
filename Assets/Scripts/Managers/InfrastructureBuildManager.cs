using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class InfrastructureBuildManager : MonoBehaviour
{
    //config
    public int infrastructureBuildTime = 3; //TODO public field
    
    //state
    public InfrastructureType typeOfTileToBuild { get; private set; }
    public List<Tile> TilesWithInfrastructureToBuild { get; private set; }

    //dependencies
    [Inject] PlayerInput playerInput;

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

    public void InfrastructureTileSelected(InfrastructureType type)
    {
        typeOfTileToBuild = type;
        playerInput.CurrentState = State.infrastructure;
    }

    public void InfrastuctureSelectCancel()
    {
        typeOfTileToBuild = InfrastructureType.empty;
    }

}

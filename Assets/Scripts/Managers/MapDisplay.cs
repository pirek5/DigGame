using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

public class MapDisplay : MonoBehaviour
{
    //config
    #pragma warning disable 0649
    [Header("Tiles")]
    [SerializeField] private TileBase fullTile;
    [SerializeField] private TileBase backgroundTile;
    [SerializeField] private TileBase digSelectionTile;
    [SerializeField] private TileBase infrastructureTile;
    [SerializeField] private TileBase infrastructureSelectionTile;
    [SerializeField] private InfrastructureTile[] infrastructureTiles;

    [System.Serializable]
    class InfrastructureTile
    {
        public InfrastructureType type;
        public TileBase tileSelection;
        public TileBase tile;
    }

    //references set in editor
    [Header("Tilemaps")]
    [SerializeField] private Tilemap foreground;
    [SerializeField] private Tilemap background;
    [SerializeField] private Tilemap selection;
    [SerializeField] private Tilemap infrastructure;

    //dependencies
    [Inject] private PlayerInput playerInput;
    [Inject] private InfrastructureBuildManager infrastructureBuildManager;
    #pragma warning restore 0649

    private void Update()
    {
        TemporaryTileDisplay(playerInput.tileUnderneathCursor, playerInput.previousTileUnderneathCursor, playerInput.CurrentState);
    }


    public void DisplayMap(Dictionary<Vector2Int, Tile> gridDictionary)
    {
        foreach(KeyValuePair<Vector2Int, Tile> tile in gridDictionary)
        {
            if(tile.Value.TileType == TileType.full)
            {
                foreground.SetTile((Vector3Int)tile.Key, fullTile);
                var ttile = background.GetTile((Vector3Int)tile.Key);
            }
            else if (tile.Value.TileType == TileType.empty)
            {
                background.SetTile((Vector3Int)tile.Key, backgroundTile);
                foreground.SetTile((Vector3Int)tile.Key, null);
            }
        }
    }

    public void DisplayTile(Tile tile)
    {
        if(tile.DigIt)
        {
            selection.SetTile(Vector3Int.FloorToInt(tile.Position), digSelectionTile);
        }
        else if (tile.InfrastructureToBuild != InfrastructureType.empty)
        {
            selection.SetTile(Vector3Int.FloorToInt(tile.Position), GetInfrastructureTile(tile.InfrastructureToBuild, true));
        }
        else
        {
            selection.SetTile(Vector3Int.FloorToInt(tile.Position), null);
        }

        if (tile.InfrastructureType != InfrastructureType.empty)
        {
            infrastructure.SetTile(Vector3Int.FloorToInt(tile.Position), GetInfrastructureTile(tile.InfrastructureType, false));
        }
        else
        {
            infrastructure.SetTile(Vector3Int.FloorToInt(tile.Position), null);
        }

        if(tile.TileType == TileType.empty)
        {
            background.SetTile(Vector3Int.FloorToInt(tile.Position), backgroundTile);
            foreground.SetTile(Vector3Int.FloorToInt(tile.Position), null);
        }
    }

    public void TemporaryTileDisplay(Tile currentTile, Tile previousTile, State currentState)
    {
        if (currentState == State.dig && currentTile.TileType == TileType.full)
        {
            selection.SetTile(Vector3Int.FloorToInt(currentTile.Position), digSelectionTile);
            DisplayTile(previousTile);
        }
        else if(currentState == State.erase)
        {
            selection.SetTile(Vector3Int.FloorToInt(currentTile.Position), null);
            DisplayTile(previousTile);
        }
        else if(currentState == State.infrastructure && currentTile.TileType == TileType.empty)
        {
            SetTempInfrastructureTile(currentTile);
            DisplayTile(previousTile);
        }
        else
        {
            DisplayTile(previousTile);
            DisplayTile(currentTile);
        }
    }

    private void SetTempInfrastructureTile(Tile tile)
    {
        if(infrastructureBuildManager.typeOfTileToBuild == InfrastructureType.substructure)
        {
            Vector2Int lowerTilePos = Vector2Int.FloorToInt(tile.Position) + Vector2Int.down;
            if (!GridData.GridDictionary.ContainsKey(lowerTilePos)) { return; }
            var lowerTile = GridData.GridDictionary[lowerTilePos];
            if (lowerTile.TileType == TileType.full)
            {
                selection.SetTile(Vector3Int.FloorToInt(tile.Position), GetInfrastructureTile(InfrastructureType.substructure, true));
            }
        }
        else if(infrastructureBuildManager.typeOfTileToBuild == InfrastructureType.pipe)
        {
            selection.SetTile(Vector3Int.FloorToInt(tile.Position), GetInfrastructureTile(InfrastructureType.pipe, true));
        }
    }

    private TileBase GetInfrastructureTile(InfrastructureType type, bool SelectionTile)
    {
        foreach (var tile in infrastructureTiles)
        {
            if (tile.type == type)
            {
                return SelectionTile ?  tile.tileSelection : tile.tile;
            }
        }

        Debug.LogError("Cant find tile!");
        return null;
    }
}

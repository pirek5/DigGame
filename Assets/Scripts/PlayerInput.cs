using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public enum State {normal, dig, build, erase, unitSelected, buildingSelected, infrastructure }

public class PlayerInput : MonoBehaviour
{
    //references set in editor
    #pragma warning disable 0649
    [SerializeField] public LayerMask selectables;
    #pragma warning restore 0649

    //state
    public State CurrentState { get; set; }
    public Vector2 MousePos2D { get; private set; }
    public Vector2Int MouseGridPos { get; private set; }
    public Tile tileUnderneathCursor { get; private set; }
    public Tile previousTileUnderneathCursor { get; private set; }

    public bool LMB { get; private set; }
    public bool RMB { get; private set; }
    public bool LMBdown { get; private set; }
    public bool RMBdown { get; private set; }
    public bool CursorOverUI { get; private set; }



    //dependencies
    [Inject] private MapDisplay mapDisplay;
    [Inject] private Grid grid;

    private void Start()
    {
        CurrentState = State.normal;
        tileUnderneathCursor = GridData.DefaultTile;
        previousTileUnderneathCursor = GridData.DefaultTile;
    }

    void Update()
    {
        CollectInputData();       
    }

    private void CollectInputData()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        MousePos2D = new Vector2(mousePos.x, mousePos.y);
        MouseGridPos = (Vector2Int)grid.WorldToCell(mousePos);
        LMB = Input.GetMouseButton(0);
        RMB = Input.GetMouseButton(1);
        LMBdown = Input.GetMouseButtonDown(0);
        RMBdown = Input.GetMouseButtonDown(1);
        CursorOverUI = EventSystem.current.IsPointerOverGameObject();

        Tile tempTile;
        if (GridData.GridDictionary.ContainsKey(MouseGridPos))
        {
            tempTile = GridData.GridDictionary[MouseGridPos];
            
        }
        else
        {
            tempTile = GridData.DefaultTile;
        } 

        if (tempTile != tileUnderneathCursor)
        {
            previousTileUnderneathCursor = tileUnderneathCursor;
            tileUnderneathCursor = tempTile;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public enum State {normal, dig, erase, unitSelected, infrastructure }

public class PlayerInput : MonoBehaviour
{
    //references set in editor
    #pragma warning disable 0649
    [SerializeField] private LayerMask selectables;
    #pragma warning restore 0649

    //state
    public State CurrentState { get; set; }
    public GameObject SelectedObject { get; private set; }
    public Vector2 MousePos2D { get; private set; }
    private Vector2Int gridPos;
    Tile mouseOverTile, previousMouseOverTile;
    
    //dependencies
    [Inject] private GridData gridData;
    [Inject] private MapDisplay mapDisplay;
    [Inject] private Grid grid;
    [Inject] private UIPanelManager uiPanelManager;

    private void Start()
    {
        CurrentState = State.normal;
        mouseOverTile = GridData.DefaultTile;
        previousMouseOverTile = GridData.DefaultTile;
    }

    void Update()
    {
        CollectData();       
        switch (CurrentState)
        {
            case State.normal:
            case State.unitSelected:
                MouseActionsNormal();
                break;
            case State.dig:
            case State.erase:
            case State.infrastructure:
                MouseActionsDigOrErase();
                break;
        }
        DebugLog();
    }

    private void CollectData()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        MousePos2D = new Vector2(mousePos.x, mousePos.y);
        gridPos = (Vector2Int)grid.WorldToCell(mousePos);
        if (GridData.GridDictionary.ContainsKey(gridPos))
        {
            mouseOverTile = GridData.GridDictionary[gridPos];
        }
        else
        {
            mouseOverTile = GridData.DefaultTile;
        }

        if(mouseOverTile != previousMouseOverTile)
        {
            mapDisplay.TemporaryTileDisplay(mouseOverTile, previousMouseOverTile, CurrentState);
            previousMouseOverTile = mouseOverTile;
        }
    }

    private void MouseActionsNormal()
    {
        if (Input.GetMouseButtonDown(0)) // LMB (clicked)
        {
            RaycastHit2D hit = Physics2D.Raycast(MousePos2D, Vector2.zero, Mathf.Infinity, selectables);
            if (hit.collider != null)
            {
                SelectObject(hit.collider.gameObject);
            }
            else if(!EventSystem.current.IsPointerOverGameObject())
            {
                uiPanelManager.CloseAll();
                SelectedObject = null;
                CurrentState = State.normal;
            }
        }

        if (CurrentState == State.unitSelected && Input.GetMouseButtonDown(1)) //RMB (clicked)
        {
            if (SelectedObject.GetComponentInParent<Movement>())
            {
                if (GridData.GridDictionary.ContainsKey(gridPos))
                {
                    SelectedObject.GetComponentInParent<Movement>().MoveToPosition(GridData.GridDictionary[gridPos]);
                }
            }
        }
    }

    private void MouseActionsDigOrErase()
    {
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject()) //LMB (click and hold)
        {
            if (CurrentState == State.dig)
            {
                gridData.MarkTileToDig(gridPos);
            }
            else if (CurrentState == State.erase)
            {
                gridData.EraseMark(gridPos);
            }
            else if (CurrentState == State.infrastructure)
            {
                gridData.MarkTileAsInfrastructureToBuild(gridPos);
            }
        }

        RaycastHit2D hit = Physics2D.Raycast(MousePos2D, Vector2.zero, Mathf.Infinity, selectables); //LMB Click
        if (Input.GetMouseButtonDown(0) && hit.collider != null)
        {
            SelectObject(hit.collider.gameObject);
        }

        if (Input.GetMouseButton(1) && !EventSystem.current.IsPointerOverGameObject()) //RMB (click and hold)
        {
            CurrentState = State.normal;
            mapDisplay.TemporaryTileDisplay(mouseOverTile, previousMouseOverTile, CurrentState);
        }
    }

    private void SelectObject(GameObject obj)
    {
        SelectedObject = obj;
        if (obj.GetComponentInParent<Unit>())
        {
            CurrentState = State.unitSelected;
            uiPanelManager.OpenUnitPanel();
        }
        // else if building....
    }

    private void DebugLog()
    {
        if (Input.GetMouseButtonDown(2))
        {
            if (GridData.GridDictionary.ContainsKey(gridPos))
            {
                var tile = GridData.GridDictionary[gridPos];
                print("State: " + tile.TileType);
                print("DigIt: " + tile.DigIt);
            }
        }
    }
}

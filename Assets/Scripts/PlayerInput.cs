using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum State {normal, dig, erase, unitSelected }

public class PlayerInput : MonoBehaviour
{
    //references set in editor
    #pragma warning disable 0649
    [SerializeField] private Grid grid;
    [SerializeField] private LayerMask selectables;
    #pragma warning restore 0649

    //state
    public State CurrentState { get; set; }
    private GameObject selectedObject;

    //singleton
    public static PlayerInput Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            CurrentState = State.normal;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    void Update()
    {
        switch (CurrentState)
        {
            case State.normal:
            case State.unitSelected:
                MouseActionsNormal();
                break;
            case State.dig:
            case State.erase:
                MouseActionsDigOrErase();
                break;
        }
        DebugLog();
    }

    private void MouseActionsNormal()
    {
        if (Input.GetMouseButtonDown(0)) // LMB (clicked)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero, Mathf.Infinity, selectables);
            if (hit.collider != null)
            {
                selectedObject = hit.transform.gameObject;
                CurrentState = State.unitSelected;
                DiggerPanel.Open();
            }
            else if(!EventSystem.current.IsPointerOverGameObject())
            {
                UIPanelManager.Instance.CloseAll();
                selectedObject = null;
                CurrentState = State.normal;
            }
        }

        if (CurrentState == State.unitSelected && Input.GetMouseButtonDown(1)) //RMB (clicked)
        {
            if (selectedObject.GetComponentInParent<Movement>())
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2Int gridPos = (Vector2Int)grid.WorldToCell(mousePos);
                if (GridData.gridDictionary.ContainsKey(gridPos))
                {
                    selectedObject.GetComponentInParent<Movement>().MoveToPosition(GridData.gridDictionary[gridPos]);
                }
            }
        }
    }

    private void MouseActionsDigOrErase()
    {
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject()) //LMB (click and hold)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int gridPos = (Vector2Int)grid.WorldToCell(mousePos);
            if (CurrentState == State.dig)
            {
                GridData.Instance.MarkTileToDig(gridPos);
            }
            else if (CurrentState == State.erase)
            {
                GridData.Instance.EraseTileToDig(gridPos);
            }
        }

        if (Input.GetMouseButton(1) && !EventSystem.current.IsPointerOverGameObject()) //RMB (click and hold)
        {
             CurrentState = State.normal;
        }
    }

    private void DebugLog()
    {
        if (Input.GetMouseButtonDown(2))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int gridPos = (Vector2Int)grid.WorldToCell(mousePos);
            if (GridData.gridDictionary.ContainsKey(gridPos))
            {
                var tile = GridData.gridDictionary[gridPos];
                print("State: " + tile.TileType);
                print("DigIt: " + tile.DigIt);
            }
        }

        if(Input.GetKeyDown(KeyCode.Backspace) && selectedObject != null)
        {
            if (selectedObject.GetComponentInParent<Digger>())
            {
                if (selectedObject.GetComponentInParent<Digger>().CurrentExcavation != null)
                {
                    print("current excavation tiles: " + selectedObject.GetComponent<Digger>().CurrentExcavation.TilesInExcavation.Count);
                }
                else
                {
                    print("current excavation is null");
                }
                print("digging: " + selectedObject.GetComponentInParent<Digger>().Digging);
            }
            else
            {
                print("something wrong");
            }
            
        }
    }
}

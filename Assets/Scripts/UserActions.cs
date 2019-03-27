using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UserActions : MonoBehaviour
{
    //dependencies
    [Inject] private GridData gridData;
    [Inject] private UIPanelManager uiPanelManager;
    [Inject] private BuildManager buildManager;
    private PlayerInput playerInput;

    //State
    public GameObject SelectedObject { get; private set; }

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        switch (playerInput.CurrentState)
        {
            case State.normal:
            case State.unitSelected:
            case State.buildingSelected:
                MouseActionsNormal();
                break;
            case State.dig:
            case State.erase:
            case State.infrastructure:
                MouseActionsEnvironment();
                break;
            case State.build:
                MouseActionsBuild();
                break;
        }
    }

    private void MouseActionsNormal()
    {
        if (playerInput.LMBdown) // LMB (clicked) - select unit or bulding
        {
            TrySelectObject();
        }

        if (playerInput.CurrentState == State.unitSelected && playerInput.RMBdown) //RMB (clicked) - move unit
        {
            MoveUnit();
        }
    }

    private void MouseActionsEnvironment()
    {
        if (playerInput.LMB && !playerInput.CursorOverUI) // LMB (hold) - confirm selected action
        {
            ModifyEnvironment();
        }

        RaycastHit2D hit = Physics2D.Raycast(playerInput.MousePos2D, Vector2.zero, Mathf.Infinity, playerInput.selectables); //select object if clicked
        if (Input.GetMouseButtonDown(0) && hit.collider != null)
        {
            SelectObject(hit.collider.gameObject);
        }

        if (playerInput.RMBdown && !playerInput.CursorOverUI) //RMB click - cancel
        {
            playerInput.CurrentState = State.normal;
        }
    }

    private void MouseActionsBuild()
    {
        if (playerInput.LMBdown) // LMB (clicked) - place buildig
        {
            buildManager.PlaceBuilding();
        }

        if (playerInput.RMBdown) //RMB (clicked) - cancel
        {
            buildManager.CancelBuild();
        }
    }

    private void SelectObject(GameObject obj)
    {
        SelectedObject = obj;
        if (obj.GetComponentInParent<Unit>())
        {
            playerInput.CurrentState = State.unitSelected;
            uiPanelManager.OpenUnitPanel();
        }
        // else if building....
    }

    private void TrySelectObject()
    {
        RaycastHit2D hit = Physics2D.Raycast(playerInput.MousePos2D, Vector2.zero, Mathf.Infinity, playerInput.selectables);
        if (hit.collider != null)
        {
            SelectObject(hit.collider.gameObject);
        }
        else if (!playerInput.CursorOverUI)
        {
            uiPanelManager.CloseAll();
            SelectedObject = null;
            playerInput.CurrentState = State.normal;
        }
    }

    private void MoveUnit()
    {
        if (SelectedObject.GetComponentInParent<Movement>())
        {
            if (GridData.GridDictionary.ContainsKey(playerInput.MouseGridPos))
            {
                SelectedObject.GetComponentInParent<Movement>().MoveToPosition(GridData.GridDictionary[playerInput.MouseGridPos]);
            }
        }
    }

    private void ModifyEnvironment()
    {
        if (playerInput.CurrentState == State.dig)
        {
            gridData.MarkTileToDig(playerInput.MouseGridPos);
        }
        else if (playerInput.CurrentState == State.erase)
        {
            gridData.EraseMark(playerInput.MouseGridPos);
        }
        else if (playerInput.CurrentState == State.infrastructure)
        {
            gridData.MarkTileAsInfrastructureToBuild(playerInput.MouseGridPos);
        }
    }


}

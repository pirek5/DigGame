using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum State {normal, dig, erase, unitSelected }

public class PlayerInput : MonoBehaviour
{
    //references set in editor
    [SerializeField] private Grid grid;
    [SerializeField] private GridData gridData;
    [SerializeField] private LayerMask selectables;

    //state
    public State currentState = State.normal;
    public GameObject selectedObject;

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
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
                currentState = State.unitSelected;
                DiggerPanel.Open();
                //hit.collider.GetComponent<UnitControlPanel>().Init();
            }
            else if(!EventSystem.current.IsPointerOverGameObject())
            {
                UIPanelManager.Instance.CloseAll();
                selectedObject = null;
                currentState = State.normal;
            }
        }

        if (currentState == State.unitSelected && Input.GetMouseButtonDown(1)) //RMB (clicked)
        {
            if (selectedObject.GetComponentInParent<Movement>())
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2Int gridPos = (Vector2Int)grid.WorldToCell(mousePos);
                if (GridData.gridDictionary.ContainsKey(gridPos))
                {
                    selectedObject.GetComponentInParent<Movement>().MoveToPosition(gridPos);
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
            if (currentState == State.dig)
            {
                gridData.DigTile(gridPos, true);
                //gridData.DeleteTile(gridPos);
            }
            else if (currentState == State.erase)
            {
                gridData.DigTile(gridPos, false);
            }
        }

        if (Input.GetMouseButton(1) && !EventSystem.current.IsPointerOverGameObject()) //RMB (click and hold)
        {
             currentState = State.normal;
        }
    }
}

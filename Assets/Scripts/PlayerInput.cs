using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            }
            else
            {
                selectedObject = null;
                currentState = State.normal;
            }
        }

        if (currentState == State.unitSelected && Input.GetMouseButtonDown(1)) //RMB (clicked)
        {
            if (selectedObject.GetComponentInParent<Digger>())
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2Int gridPos = (Vector2Int)grid.WorldToCell(mousePos);
                selectedObject.GetComponentInParent<Digger>().MoveOrDig(gridPos);
            }
        }
    }

    private void MouseActionsDigOrErase()
    {
        if (Input.GetMouseButton(0)) //LMB (click and hold)
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

        if (Input.GetMouseButton(1)) //RMB (click and hold)
        {
             currentState = State.normal;
        }
    }
}

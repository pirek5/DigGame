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
        if (Input.GetMouseButton(0))
        {
            if (currentState == State.normal || currentState == State.unitSelected)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, selectables))
                {
                    //highlightedObject = hit.transform.gameObject.name;
                    if (Input.GetMouseButtonDown(0))
                    {
                        selectedObject = hit.transform.gameObject;
                        currentState = State.unitSelected;
                    }
                }
                else
                {
                    selectedObject = null;
                    currentState = State.normal;
                }
            }
            else if(currentState == State.unitSelected)
            {
                if (selectedObject.GetComponent<Digger>())
                {

                }
            }
            else
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2Int gridPos = (Vector2Int)grid.WorldToCell(mousePos);
                if (currentState == State.dig)
                {
                    //gridData.DigTile(gridPos, true);
                    gridData.DeleteTile(gridPos);
                }
                else if (currentState == State.erase)
                {
                    gridData.DigTile(gridPos, false);
                }
            }
        }

        //cancel
        if(currentState != State.normal && Input.GetMouseButton(1))
        {
            currentState = State.normal;
            selectedObject = null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum state {normal, dig, erase }

public class PlayerInput : MonoBehaviour
{
    //references set in editor
    [SerializeField] private Grid grid;
    [SerializeField] private GridData gridData;

    //state
    public state currentState = state.normal;
    public GameObject selectedObject;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (currentState == state.normal)
            {
                // TODO zaznaczanie jednostek
            }
            else
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2Int gridPos = (Vector2Int)grid.WorldToCell(mousePos);
                if (currentState == state.dig)
                {
                    gridData.DigTile(gridPos, true);
                }
                else if (currentState == state.erase)
                {
                    gridData.DigTile(gridPos, false);
                }
            }
        }

        if(currentState != state.normal && Input.GetMouseButton(1))
        {
            currentState = state.normal;
        }
    }
}

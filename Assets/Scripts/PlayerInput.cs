using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    //references set in editor
    [SerializeField] private Grid grid;
    [SerializeField] private GridData gridData;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPos = grid.WorldToCell(mousePos);
            gridData.ChangeTile(gridPos);
        }
    }
}

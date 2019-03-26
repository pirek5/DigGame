using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingSize { B1x1, B2x1, B2x2};

public class ConstructionPlan : MonoBehaviour
{
    [SerializeField] private BuildingSize size;

    void Start()
    {
        List<Vector2Int> buildingTiles = GetBuildingTiles(size);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    List<Vector2Int> GetBuildingTiles(BuildingSize size)
    {
        switch (size)
        {
            case BuildingSize.B1x1: return new List<Vector2Int>();
            case BuildingSize.B2x1: return new List<Vector2Int>() { Vector2Int.right};
            case BuildingSize.B2x2: return new List<Vector2Int>() { Vector2Int.right, Vector2Int.up, new Vector2Int(1, 1) };
        }
        return null;
    }

}

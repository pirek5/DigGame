using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingSize { B1x1, B2x1, B2x2};

public class ConstructionPlan : MonoBehaviour
{
    //set in editor
    [SerializeField] private BuildingSize size;
    [SerializeField] private int constructionTime = 0;
    public BuildingName BuildingName { get; set; }

    //state
    private int constructionProgress; //when > constructionTime, building is finished
    public List<Vector2Int> BuildingTiles { get; private set; }
    public Vector2 offset { get; private set; }

    //dependecies
    BuildingInfo buildingInfo;
    BuildingDisplay buildingDisplay;

    void Awake()
    {
        (BuildingTiles, offset) = GetBuildingTiles(size);
        buildingInfo = GetComponent<BuildingInfo>();
        buildingDisplay = GetComponent<BuildingDisplay>();
    }

    (List<Vector2Int>, Vector2) GetBuildingTiles(BuildingSize size)
    {
        switch (size)
        {
            case BuildingSize.B1x1: return (new List<Vector2Int>() { Vector2Int.zero }, Vector2.zero);
            case BuildingSize.B2x1: return (new List<Vector2Int>() { Vector2Int.zero, Vector2Int.right}, new Vector2(0f,0f));
            case BuildingSize.B2x2: return (new List<Vector2Int>() { Vector2Int.zero, Vector2Int.right, Vector2Int.up, new Vector2Int(1, 1) }, new Vector2(0.5f, 0.5f));
        }
        return (null, Vector2.zero);
    }

    public void Build()
    {
        constructionProgress++;
        if(constructionProgress >= constructionTime)
        {
            buildingInfo.IsConstructed = true;
            buildingDisplay.BuildingFinished();
        }
    }

}

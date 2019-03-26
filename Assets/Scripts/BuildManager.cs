using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BuildManager : MonoBehaviour
{
    //cached
    private GameObject currentBuilding;

    //dependencies
    [Inject] private GridData gridData;
    [Inject] private PlayerInput playerInput;



    // Update is called once per frame
    void Update()
    {
        if(currentBuilding != null)
        {
            currentBuilding.transform.position = playerInput.MousePos2D;
        }
    }

    public void CheckIfPossibleToBuild(GameObject building)
    {
        print("elo");
        if (!building.GetComponent<ConstructionPlan>()) { return; }
        print("elo2");
        currentBuilding = Instantiate(building);  
    }
}

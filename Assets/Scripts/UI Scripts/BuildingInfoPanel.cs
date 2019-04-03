using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingInfoPanel : UIPanel
{
    #pragma warning disable 0649
    [SerializeField] private  Text buildingName;
    #pragma warning restore 0649

    public BuildingInfo selectedBuilding;
    private UserActions userActions;

    private void Update()
    {
        if(selectedBuilding == null) { return; }
        
        UpdatePanelInformations();
    }

    public override void Init()
    {
        selectedBuilding = userActions.SelectedObject.GetComponentInParent<BuildingInfo>();
    }

    public override void SetDependency(UserActions userActions, BuildManager buildManager)
    {
        this.userActions = userActions;
    }

    private void UpdatePanelInformations()
    {
        buildingName.text = selectedBuilding.buildingName;
    }
}

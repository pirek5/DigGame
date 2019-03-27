using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ChooseBuildingPanel : UIPanel
{
    //dependencies
    [Inject]
    private BuildingTemplatesContainer buildingTemplates;

    private BuildManager buildManager;

    private void Start()
    {
        buildingTemplates = FindObjectOfType<BuildingTemplatesContainer>(); //TODO
    }

    public override void SetDependency(UserActions userActions, BuildManager buildManager)
    {
        this.buildManager = buildManager;
    }

    public void OnHeadquatersPressed()
    {
        var hedquatersPrefab = buildingTemplates.SetActiveTemplate(BuildingName.Headquaters);
        buildManager.TryBuild(hedquatersPrefab);
    }

}

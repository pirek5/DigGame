using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseBuildingPanel : UIPanel
{
    [SerializeField] private GameObject HedquatersPrefab;

    private BuildManager buildManager;

    public override void SetDependency(UserActions userActions, BuildManager buildManager)
    {
        this.buildManager = buildManager;
    }

    public void OnHeadquatersPressed()
    {
        buildManager.TryBuild(HedquatersPrefab);
    }

}

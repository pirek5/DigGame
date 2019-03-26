using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseBuildingPanel : UIPanel
{
    [SerializeField] private GameObject HedquatersPrefab;

    private PlayerInput playerInput;
    private BuildManager buildManager;

    public override void SetDependency(PlayerInput playerInput, BuildManager buildManager)
    {
        this.playerInput = playerInput;
        this.buildManager = buildManager;
    }

    public void OnHeadquatersPressed()
    {
        buildManager.CheckIfPossibleToBuild(HedquatersPrefab);
    }

}

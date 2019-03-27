using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitPanel : UIPanel
{
    #pragma warning disable 0649
    [SerializeField] private  Text unitName;
    [SerializeField] private Slider energySlider;
    #pragma warning restore 0649

    public Unit selectedUnit;
    private UserActions userActions;

    private void Update()
    {
        if(selectedUnit == null) { return; }

        UpdatePanelInformations();
    }

    public override void Init()
    {
        selectedUnit = userActions.SelectedObject.GetComponentInParent<Unit>();
    }

    public override void SetDependency(UserActions userActions, BuildManager buildManager)
    {
        this.userActions = userActions;
    }

    private void UpdatePanelInformations()
    {
        unitName.text = selectedUnit.unitName;
        energySlider.maxValue = selectedUnit.maxEnergy;
        energySlider.value = selectedUnit.currentEnergy;
    }
}

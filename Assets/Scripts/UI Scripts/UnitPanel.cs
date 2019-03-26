using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitPanel : UIPanel<UnitPanel>
{
    #pragma warning disable 0649
    [SerializeField] private  Text unitName;
    [SerializeField] private Slider energySlider;
    #pragma warning restore 0649

    public Unit selectedUnit;
    private PlayerInput playerInput;

    private void Update()
    {
        if(selectedUnit == null) { return; }

        UpdatePanelInformations();
    }

    public override void Init()
    {
        selectedUnit = playerInput.SelectedObject.GetComponentInParent<Unit>();
    }

    public override void SetDependency(PlayerInput playerInput, BuildManager buildManager)
    {
        this.playerInput = playerInput;
    }

    private void UpdatePanelInformations()
    {
        unitName.text = selectedUnit.unitName;
        energySlider.maxValue = selectedUnit.maxEnergy;
        energySlider.value = selectedUnit.currentEnergy;
    }
}

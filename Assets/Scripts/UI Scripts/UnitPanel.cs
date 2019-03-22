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

    private void Update()
    {
        if(selectedUnit == null) { return; }

        UpdatePanelInformations();
    }

    public override void Init()
    {
        base.Init();
        if(PlayerInput.Instance.SelectedObject != null)
        {
            selectedUnit = PlayerInput.Instance.SelectedObject.GetComponentInParent<Unit>();
        }
        FindRenderCamera();
    }

    private void UpdatePanelInformations()
    {
        unitName.text = selectedUnit.unitName;
        energySlider.maxValue = selectedUnit.maxEnergy;
        energySlider.value = selectedUnit.currentEnergy;
    }

    private void FindRenderCamera()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }


}

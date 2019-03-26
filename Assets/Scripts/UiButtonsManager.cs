using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UiButtonsManager : MonoBehaviour
{
    [Inject] PlayerInput playerInput;
    [Inject] UIPanelManager uiPanelManager;

    public void OnDigPressesd()
    {
        uiPanelManager.CloseAll();
        playerInput.CurrentState = State.dig;
    }

    public void OnErasePressesd()
    {
        uiPanelManager.CloseAll();
        playerInput.CurrentState = State.erase;
    }

    public void OnInfrastructurePressed()
    {
        uiPanelManager.CloseAll();
        playerInput.CurrentState = State.infrastructure;
    }

    public void OnBuildPressed()
    {
        uiPanelManager.OpenBuildingPanel();
    }
}

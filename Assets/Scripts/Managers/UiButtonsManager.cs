﻿using System.Collections;
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
        uiPanelManager.OpenPanelAndClosePanel<ChooseInfrastructurePanel, ChooseBuildingPanel>();
    }

    public void OnBuildPressed()
    {
        uiPanelManager.OpenPanelAndClosePanel<ChooseBuildingPanel, ChooseInfrastructurePanel>();
    }
}

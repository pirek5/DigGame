using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UiButtonsManager : MonoBehaviour
{
    [Inject] PlayerInput playerInput;

    public void OnDigPressesd()
    {
        UIPanelManager.Instance.CloseAll();
        playerInput.CurrentState = State.dig;
    }

    public void OnErasePressesd()
    {
        UIPanelManager.Instance.CloseAll();
        playerInput.CurrentState = State.erase;
    }

    public void OnInfrastructurePressed()
    {
        UIPanelManager.Instance.CloseAll();
        playerInput.CurrentState = State.infrastructure;
    }

    public void OnBuildPressed()
    {
        ChooseBuildingPanel.Open();
    }
}

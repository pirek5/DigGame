using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiButtonsManager : MonoBehaviour
{
    public void OnDigPressesd()
    {
        UIPanelManager.Instance.CloseAll();
        PlayerInput.Instance.CurrentState = State.dig;
    }

    public void OnErasePressesd()
    {
        UIPanelManager.Instance.CloseAll();
        PlayerInput.Instance.CurrentState = State.erase;
    }

    public void OnInfrastructurePressed()
    {
        UIPanelManager.Instance.CloseAll();
        PlayerInput.Instance.CurrentState = State.infrastructure;
    }
}

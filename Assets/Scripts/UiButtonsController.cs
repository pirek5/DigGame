using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiButtonsController : MonoBehaviour
{
    //cached
    PlayerInput playerInput;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    public void OnDigPressesd()
    {
        playerInput.currentState = state.dig;
    }

    public void OnErasePressesd()
    {
        playerInput.currentState = state.erase;
    }

}

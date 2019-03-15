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
        playerInput.currentState = State.dig;
    }

    public void OnErasePressesd()
    {
        playerInput.currentState = State.erase;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    //config
    [SerializeField] private float sizeFactor = 1f;

    //references set in editor
    #pragma warning disable 0649
    [SerializeField] private Animator primaryAnimator;
    [SerializeField] private Animator secendaryAnimator;
    [SerializeField] private Transform cursor;
    [SerializeField] private Camera mainCamera;
    #pragma warning restore 0649

    //cached
    State cursorState;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        cursor.localScale = mainCamera.orthographicSize * new Vector3(sizeFactor, sizeFactor);
        Vector2 cursorPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        cursor.position = cursorPos;
        cursorState = PlayerInput.Instance.CurrentState;
        UpdateCursorIcon();
    }

    public void UpdateCursorIcon()
    {
        switch (cursorState)
        {
            case State.normal:
            case State.unitSelected:
                CursorNormal();
                break;
            case State.dig:
                CursorDig();
                break;
            case State.erase:
                CursorErase();
                break;
        }
    }

    public void OnMouseClick()
    {
        primaryAnimator.SetTrigger("click");
    }

    public void CursorErase()
    {
        secendaryAnimator.SetTrigger("Erase");
    }

    public void CursorNormal()
    {
        secendaryAnimator.SetTrigger("Normal");
    }

    public void CursorDig()
    {
        secendaryAnimator.SetTrigger("Dig");
    }

    public void CursorDigging()
    {

    }
}

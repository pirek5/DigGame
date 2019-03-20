using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    //reference set in editor
    #pragma warning disable 0649
    [SerializeField] private Camera mainCamera;
    #pragma warning restore 0649

    //config
    [SerializeField] private float mouseScrollSensitivity = 1f;
    [SerializeField] private float keyboardScrollSensitivity = 1f;
    [SerializeField] private float mouseZoomSensitivity = 1f;
    [SerializeField] private float keyboardZoomSensitivity = 1f;

    //cached
    private Vector3 mousePos = Vector3.zero;

    void Update()
    {
        MouseInput();
        KeyboardInput();
    }

    private void MouseInput()
    {
        //convert mouse position range to (-1, +1)
        mousePos.x = (Input.mousePosition.x - (Screen.width * (1 - Input.mousePosition.x / Screen.width))) / Screen.width;
        mousePos.y = (Input.mousePosition.y - (Screen.height * (1 - Input.mousePosition.y / Screen.height))) / Screen.height;
        if (Mathf.Abs(mousePos.x) >= 1f)
        {
            MoveCameraHorizontal(mousePos.x, mouseScrollSensitivity);
        }

        if (Mathf.Abs(mousePos.y) >= 1f)
        {
            MoveCameraVertical(mousePos.y, mouseScrollSensitivity);
        }

        CameraZoom(Input.mouseScrollDelta.y, mouseZoomSensitivity);
    }

    private void KeyboardInput()
    {
        MoveCameraHorizontal(Input.GetAxis("Horizontal"), keyboardScrollSensitivity);
        MoveCameraVertical(Input.GetAxis("Vertical"), keyboardScrollSensitivity);
        if (Input.GetKey(KeyCode.KeypadPlus))
        {
            CameraZoom(1f, keyboardZoomSensitivity);
        }
        else if (Input.GetKey(KeyCode.KeypadMinus))
        {
            CameraZoom(-1f, keyboardZoomSensitivity);
        }
    }

    private void MoveCameraHorizontal(float translateX, float sensitivity)
    {
        mainCamera.transform.position = Camera.main.transform.position + new Vector3(translateX,0f,0f) * Time.deltaTime * sensitivity * Camera.main.orthographicSize;
    }

    private void MoveCameraVertical(float translateY, float sensitivity)
    {
        mainCamera.transform.position = Camera.main.transform.position + new Vector3(0f, translateY, 0f) * Time.deltaTime * sensitivity * Camera.main.orthographicSize;
    }

    private void CameraZoom(float zoom, float sensitivity)
    {
        mainCamera.orthographicSize = Camera.main.orthographicSize - zoom * Time.deltaTime * sensitivity;
    }
}

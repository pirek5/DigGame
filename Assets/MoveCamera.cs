using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    //config
    [SerializeField] private float scrollSensivity = 1f;

    //cached
    private Vector3 mousePos = Vector3.zero;


    // Update is called once per frame
    void Update()
    {
        MouseInput();
        KeyboardInput();
    }

    private void MouseInput()
    {
        //converts to range from -1 to +1
        mousePos.x = (Input.mousePosition.x - (Screen.width * (1 - Input.mousePosition.x / Screen.width))) / Screen.width;
        mousePos.y = (Input.mousePosition.y - (Screen.height * (1 - Input.mousePosition.y / Screen.height))) / Screen.height;
        if (Mathf.Abs(mousePos.x) >= 1f)
        {
            MoveCameraHorizontal(mousePos.x);
        }

        if (Mathf.Abs(mousePos.y) >= 1f)
        {
            MoveCameraVertical(mousePos.y);
        }
    }

    private void KeyboardInput()
    {

    }

    private void MoveCameraHorizontal(float transalteX)
    {
        Camera.main.transform.position = Camera.main.transform.position + new Vector3(transalteX,0f,0f) * Time.deltaTime * scrollSensivity;
    }

    private void MoveCameraVertical(float transalteY)
    {
        Camera.main.transform.position = Camera.main.transform.position + new Vector3(0f, transalteY, 0f) * Time.deltaTime * scrollSensivity;
    }
}

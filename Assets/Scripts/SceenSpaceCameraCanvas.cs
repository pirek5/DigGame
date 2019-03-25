using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SceenSpaceCameraCanvas : MonoBehaviour
{
    private string sortingLayer = "UI";

    private void OnEnable()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
        GetComponent<Canvas>().sortingLayerName = sortingLayer;
    }
}

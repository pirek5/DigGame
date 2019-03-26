using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SnapToGrid : MonoBehaviour
{
    private const float grid = 1f;
    public float offset = 0.5f;

    void LateUpdate()
    {
        float xPos =  Mathf.RoundToInt(transform.position.x);
        float yPos = Mathf.RoundToInt(transform.position.y);
        transform.position = new Vector3(xPos, yPos, 0f);
    }
}


﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashingObject : MonoBehaviour {

    //config
    [SerializeField] protected Color highlightedColor;
    [Range(0f, 1f)] [SerializeField] private float flashingSpeed = 0.1f;

    //cached
    private Color defaultColor;
    private SpriteRenderer spriteRenderer;

    void Awake () {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        defaultColor = spriteRenderer.color;
    }

    public void StartFlashing()
    {
        StartCoroutine(FlashCoroutine());
    }

    public void StopFlashing()
    {
        StopAllCoroutines();
        spriteRenderer.color = defaultColor;
    }


    private IEnumerator FlashCoroutine()
    {
        Color currentColor;
        float t = 1;
        float derivative = flashingSpeed;
        while (true)
        {
            currentColor = Vector4.Lerp(defaultColor, highlightedColor, t);
            spriteRenderer.color = currentColor;

            t = t + derivative;
            if (t > 1)
            {
                t = 1;
                derivative = -derivative;
            }
            else if (t < 0)
            {
                t = 0;
                derivative = -derivative;
            }
            yield return null;
        }
    }
}

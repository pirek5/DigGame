using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDisplay : MonoBehaviour
{
    
    private string preBuildSortingLayer = "PreConstructedBuildings";
    private string deffaultSortingLayer = "Buildings";

    //config
    [SerializeField] private Color preBuildColorPositive;
    [SerializeField] private Color preBuildColorNegative;
    [SerializeField] private Color preBuildPlaced;
    [SerializeField] private Color normalColor;

    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite preBuildSprite;

    //cached
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void Init()
    {
        _spriteRenderer.sortingLayerName = preBuildSortingLayer;
        _spriteRenderer.sprite = preBuildSprite;
    }

    public void BuildingCanBePlaced(bool canIt)
    {
        _spriteRenderer.color = canIt ? preBuildColorPositive : preBuildColorNegative;
    }

    public void BuildingPlaced()
    {
        _spriteRenderer.color = preBuildPlaced;
        _spriteRenderer.sortingLayerName = deffaultSortingLayer;
    }

    public void BuildingFinished()
    {
        _spriteRenderer.color = normalColor;
        _spriteRenderer.sprite = normalSprite;
    }

}

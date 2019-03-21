﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class UIPanelManager : MonoBehaviour
{
    [SerializeField] private DiggerPanel diggerPanelPrefab;
    [SerializeField] private Transform menuParent;

    private static UIPanelManager instance;
    public static UIPanelManager Instance { get { return instance; } }

    //cached
    List<UIPanel> panels = new List<UIPanel>();

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            InitializeMenus();
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    private void InitializeMenus()
    {
        if (menuParent == null)
        {
            GameObject menuParentObject = new GameObject("UI Panel");
            menuParent = menuParentObject.transform;
        }

        System.Type myType = this.GetType();
        BindingFlags myFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
        FieldInfo[] fields = myType.GetFields(myFlags);

        foreach (FieldInfo field in fields)
        {
            UIPanel prefab = field.GetValue(this) as UIPanel;
            if (prefab != null)
            {
                UIPanel panelInstance = Instantiate(prefab, menuParent);
                panels.Add(panelInstance);
                panelInstance.gameObject.SetActive(false);
            }
        }
    }

    public void OpenPanel(UIPanel uiPanelInstance)
    {
        if (uiPanelInstance == null)
        {
            Debug.LogWarning("MENUMANAGER OpenMenu Error: invalid menu");
        }
        else
        {
            foreach(var panel in panels)
            {
                panel.gameObject.SetActive(false);
            }
            uiPanelInstance.gameObject.SetActive(true);
        }
    }

    public void CloseAll()
    {
        foreach (var panel in panels)
        {
            panel.gameObject.SetActive(false);
        }
    }
}
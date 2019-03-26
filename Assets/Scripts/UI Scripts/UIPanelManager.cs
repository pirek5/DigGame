using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using Zenject;

public class UIPanelManager : MonoBehaviour
{
    [SerializeField] private UIPanel unitPanelPrefab;
    [SerializeField] private UIPanel buildingPanelPreafab;

    [SerializeField] private Transform menuParent;

    //cached
    [SerializeField] private List<UIPanel> panels;

    //dependencies to pass
    [Inject] PlayerInput playerInput;
    [Inject] BuildManager buildManager;

    private void Awake()
    {
        //FillList();
        InitializeMenus();
    }

    private void FillList()
    {
        panels = new List<UIPanel>() { unitPanelPrefab, buildingPanelPreafab };
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
                var panelInstance = Instantiate(prefab, menuParent);
                panels.Add(panelInstance);
                panelInstance.SetDependency(playerInput, buildManager);
                panelInstance.gameObject.SetActive(false);
            }
        }



        //foreach (var prefab in panels)
        //{

        //}
    }

    public void OpenUnitPanel()
    {
        foreach (var panel in panels)
        {
            if (panel.GetComponent<UnitPanel>())
            {
                panel.gameObject.SetActive(true);
                panel.Init();
            }
        }
    }

    public void OpenBuildingPanel()
    {
        foreach (var panel in panels)
        {
            if (panel.GetComponent<ChooseBuildingPanel>())
            {
                panel.gameObject.SetActive(true);
                panel.Init();
            }
        }
    }

    private void OpenMenu(UIPanel menu)
    {
        foreach(var panel in panels)
        {
            if(panel.Equals(menu))
            {
                panel.gameObject.SetActive(true);
            }
        }
    }

    public void CloseAll()
    {
        foreach (var panel in panels)
        {
            panel.gameObject.SetActive(false);
        }
    }



    //private void Awake()
    //{
    //    if (instance != null)
    //    {
    //        Destroy(gameObject);
    //    }
    //    else
    //    {
    //        instance = this;
    //        InitializeMenus();
    //    }
    //}

    //private void OnDestroy()
    //{
    //    if (instance == this)
    //    {
    //        instance = null;
    //    }
    //}

    //private void InitializeMenus()
    //{
    //    if (menuParent == null)
    //    {
    //        GameObject menuParentObject = new GameObject("UI Panel");
    //        menuParent = menuParentObject.transform;
    //    }

    //    System.Type myType = this.GetType();
    //    BindingFlags myFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
    //    FieldInfo[] fields = myType.GetFields(myFlags);

    //    foreach (FieldInfo field in fields)
    //    {
    //        UIPanel prefab = field.GetValue(this) as UIPanel;
    //        if (prefab != null)
    //        {
    //            UIPanel panelInstance = Instantiate(prefab, menuParent);
    //            panels.Add(panelInstance);
    //            panelInstance.gameObject.SetActive(false);
    //            print(panelInstance.gameObject.name);
    //            panelInstance.SetDependency(playerInput, buildManager);
    //        }
    //    }
    //}

    //public void OpenPanel(UIPanel uiPanelInstance)
    //{
    //    if (uiPanelInstance == null)
    //    {
    //        Debug.LogWarning("MENUMANAGER OpenMenu Error: invalid menu");
    //    }
    //    else
    //    {
    //        foreach(var panel in panels)
    //        {
    //            panel.gameObject.SetActive(false);
    //        }
    //        uiPanelInstance.gameObject.SetActive(true);
    //        uiPanelInstance.Init();
    //    }
    //}

    //public void CloseAll()
    //{
    //    foreach (var panel in panels)
    //    {
    //        panel.gameObject.SetActive(false);
    //    }
    //}
}

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
    [Inject] UserActions userActions;
    [Inject] BuildManager buildManager;

    private void Awake()
    {
        InitializeMenus();
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
                panelInstance.SetDependency(userActions, buildManager);
                panelInstance.gameObject.SetActive(false);
            }
        }
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

}

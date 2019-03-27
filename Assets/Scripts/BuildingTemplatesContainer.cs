using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public enum BuildingName { Headquaters, Magazine}

public class BuildingTemplatesContainer : MonoBehaviour
{
    [SerializeField] private GameObject HedquatersPrefab;

    [SerializeField] private Transform templatesParent;

    private Dictionary<BuildingName, GameObject> templates = new Dictionary<BuildingName, GameObject>();

    public GameObject ActiveTemplate { get; set; }

    private void Start()
    {
        InitializeTemplates();
    }

    private void InitializeTemplates()
    {
        if (templatesParent == null)
        {
            GameObject templatesParentObject = new GameObject("Building Templates");
            templatesParent = templatesParentObject.transform;
        }

        System.Type myType = this.GetType();
        BindingFlags myFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
        FieldInfo[] fields = myType.GetFields(myFlags);

        foreach (FieldInfo field in fields)
        {
            GameObject prefab = field.GetValue(this) as GameObject;
            if (prefab != null)
            {
                if (!prefab.GetComponent<ConstructionPlan>()) { continue; }
                var templateInstance = Instantiate(prefab, templatesParent);
                templates.Add(prefab.GetComponent<ConstructionPlan>().BuildingName, templateInstance);
                templateInstance.gameObject.SetActive(false);
            }
        }
    }

    public GameObject SetActiveTemplate(BuildingName name)
    {
        if(!templates.ContainsKey(name)) { return null; }

        ActiveTemplate = templates[name];
        ActiveTemplate.SetActive(true);
        return ActiveTemplate;
    }
}

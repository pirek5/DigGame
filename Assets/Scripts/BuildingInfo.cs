using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingInfo : MonoBehaviour
{

    public string buildingName;

    public bool IsConstructed { get; set; }
    public int CurrentHealth { get; private set; }
    public int MaxHealth { get; private set; }

}

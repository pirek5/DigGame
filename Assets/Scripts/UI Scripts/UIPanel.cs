using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class UIPanel : MonoBehaviour //TODO change to interface
{
    public virtual void Init() { }
    public virtual void SetDependency(UserActions userActions, BuildManager buildManager, InfrastructureBuildManager infrastructureBuildManager) { }
}

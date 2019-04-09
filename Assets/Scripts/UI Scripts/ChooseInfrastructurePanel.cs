using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ChooseInfrastructurePanel : UIPanel
{
    private InfrastructureBuildManager infrastructureBuildManager;

    public override void SetDependency(UserActions userActions, BuildManager buildManager, InfrastructureBuildManager infrastructureBuildManager)
    {
        this.infrastructureBuildManager = infrastructureBuildManager;
    }
    
    
    public void OnInfrastrusturePressed(int type)
    {
        infrastructureBuildManager.InfrastructureTileSelected((InfrastructureType)type);
    }

}

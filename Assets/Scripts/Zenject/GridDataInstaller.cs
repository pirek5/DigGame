using UnityEngine;
using Zenject;

public class GridDataInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind(typeof(GridData), typeof(DigManager), typeof(InfrastructureBuildManager), typeof(BuildManager), typeof(CheckTile), typeof(ChangeTile)).FromComponentInHierarchy().AsSingle();
    }
}
using UnityEngine;
using Zenject;

public class StandardInstaller : MonoInstaller<StandardInstaller>
{
    public GameObject gridDataPrefab;
    public GameObject gridPrefab;

    public override void InstallBindings()
    {
        Container.Bind(typeof(GridData), typeof(DigManager), typeof(InfrastructureBuildManager)).FromComponentInNewPrefab(gridDataPrefab).AsSingle();
        Container.Bind(typeof(MapDisplay), typeof(MapGenerator), typeof(Grid)).FromComponentInNewPrefab(gridPrefab).AsSingle();
    }
}
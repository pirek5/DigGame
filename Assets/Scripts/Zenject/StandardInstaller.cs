using UnityEngine;
using Zenject;

public class StandardInstaller : MonoInstaller<StandardInstaller>
{
    public GameObject gridDataPrefab;
    public GameObject gridPrefab;
    public GameObject playerInputPrefab;
    public GameObject templatesContainer;

    public override void InstallBindings()
    {
        Container.Bind(typeof(GridData), typeof(DigManager), typeof(InfrastructureBuildManager), typeof(BuildManager)).FromComponentInNewPrefab(gridDataPrefab).AsSingle();
        Container.Bind(typeof(MapDisplay), typeof(MapGenerator), typeof(Grid)).FromComponentInNewPrefab(gridPrefab).AsSingle();
        Container.Bind(typeof(PlayerInput), typeof(UIPanelManager), typeof(UserActions)).FromComponentInNewPrefab(playerInputPrefab).AsSingle();
        Container.Bind(typeof(BuildingTemplatesContainer)).FromComponentInNewPrefab(templatesContainer).AsSingle().NonLazy();
    }

}
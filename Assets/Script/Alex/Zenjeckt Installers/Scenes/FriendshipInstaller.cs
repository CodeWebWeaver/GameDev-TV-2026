using UnityEngine;
using Zenject;

public class UntitledInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<FriendService>().FromComponentInHierarchy().AsSingle().NonLazy();
    }
}
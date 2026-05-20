using DG.Tweening.Core.Easing;
using UnityEditor;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller {

    [SerializeField] GameController gameControllerPrefab;
    [SerializeField] UIManager uiManagerPrefab;
    public override void InstallBindings() {
        Container.Bind<GameController>().FromComponentInNewPrefab(gameControllerPrefab).AsSingle().NonLazy();
        Container.Bind<UIManager>().FromComponentInNewPrefab(uiManagerPrefab).AsSingle().NonLazy();
    }
    
}

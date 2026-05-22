using DG.Tweening.Core.Easing;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller {

    [SerializeField] GameController gameControllerPrefab;
    [SerializeField] UIManager uiManagerPrefab;
    [SerializeField] SceneDataService _sceneDataService;
    [SerializeField] GameManager _gameBootstrapper;
    [SerializeField] DialogueManager dialogueManager;

    public override void InstallBindings() {
        Container.BindInterfacesAndSelfTo<GameManager>()
        .FromComponentInNewPrefab(_gameBootstrapper)
        .AsSingle()
        .NonLazy();

        Container.Bind<IStateFactory<GameManager>>()
           .To<StateFactory<GameManager>>()
           .AsSingle();

        Container.Bind<GameController>().FromComponentInNewPrefab(gameControllerPrefab).AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<UIManager>().FromComponentInNewPrefab(uiManagerPrefab).AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<DialogueManager>().FromComponentInNewPrefab(dialogueManager).AsSingle().NonLazy();

        Container.Bind<ISceneDataService>().To<SceneDataService>().FromComponentInNewPrefab(_sceneDataService).AsSingle().NonLazy();

        Container.BindInterfacesAndSelfTo<InputManager>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<PlayerInputService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<DialogInputService>().AsSingle().NonLazy();

        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<FriendAddedSignal>();
    }

    private void StateMachineInstall() {

        Container.Bind(typeof(IStateFactory<>))
            .To(typeof(StateFactory<>))
            .AsTransient();

        Container.Bind<IStateFactory<GameManager>>()
            .To<StateFactory<GameManager>>()
            .AsSingle();

        Container.Bind<BootstrapState>().AsSingle();
        Container.Bind<MainMenuState>().AsSingle();
        Container.Bind<GameLoopState>().AsSingle();
        Container.Bind<PauseState>().AsSingle();
        Container.Bind<ExitState>().AsSingle();
    }
}

public class FriendAddedSignal {
    public Human Friend;

    public FriendAddedSignal(Human friend) {
        Friend = friend;
    }
}
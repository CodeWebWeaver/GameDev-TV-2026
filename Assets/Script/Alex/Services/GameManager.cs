using System;
using System.Collections.Generic;
using Tymski;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameManager : MonoBehaviour, IGameManager {
    [Inject] IStateFactory<GameManager> stateFactory;
    private ContextStateMachine<GameManager> _stateMachine;
    public ContextStateMachine<GameManager> StateMachine => _stateMachine;

    [Inject] ISceneDataService sceneDataService;
    [Inject] IUiService uiService;

    private void Awake() {
        _stateMachine = new ContextStateMachine<GameManager>(this, stateFactory);
        _stateMachine.ChangeState<BootstrapState>();
    }

    public void StartNewGame() {
        Debug.Log("Starting new game...");
        SceneReference gameSceneRef = sceneDataService.GetSceneReference(SceneTypes.Game);
        LoadScene(gameSceneRef);
        _stateMachine.ChangeState<GameLoopState>();
    }

    private void LoadScene(string name) {
        uiService.HideAll();
        SceneManager.LoadScene(name);
    }

    public void ExitGame() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void OnApplicationQuit() {
        _stateMachine?.ChangeState<ExitState>();
    }

    internal void EnterMenu() {
        SceneReference menuSceneRef = sceneDataService.GetSceneReference(SceneTypes.MainMenu);
        LoadScene(menuSceneRef);
        _stateMachine?.ChangeState<MainMenuState>();
    }
}

public class ExitState : State<GameManager> {
    private readonly ISaveLoadService _saveLoadService;

    public ExitState(ISaveLoadService saveLoad) {
        _saveLoadService = saveLoad;
    }

    public override void Enter() {
        _saveLoadService.SaveAll();
        Context.ExitGame();
    }

    public override void Update() {
        // No update logic needed for exit state
    }
    public override void Exit() {
        // No exit logic needed for exit state
    }
}

public class BootstrapState : State<GameManager> {
    private readonly ISaveLoadService _saveLoadService;
    private readonly ISceneDataService _sceneData;

    public BootstrapState(ISaveLoadService saveLoad, ISceneDataService data) {
        _sceneData = data;
        _saveLoadService = saveLoad;
    }

    public override void Enter() {
        Application.targetFrameRate = 60;
        _saveLoadService?.LoadAll();


        if (_sceneData.IsMainMenu()) {
            Context.StateMachine.ChangeState<MainMenuState>();
            return;
        }

        Context.StateMachine.ChangeState<GameLoopState>();
    }

    public override void Exit() {
    }
}

public class MainMenuState : State<GameManager> {
    private readonly IAudioService _audioService;

    public MainMenuState(IAudioService audioService) {
        _audioService = audioService;
    }

    public override void Enter() {
        _audioService?.StartMusicPlaylist(MusicPlaylist.MainMenu);
    }

    public override void Exit() {
        _audioService?.StopCurrentMusic();
    }
}

public class GameLoopState : State<GameManager> {
    private readonly IAudioService _audioService;

    public GameLoopState([InjectOptional] IAudioService audioService) {
        _audioService = audioService;
    }

    public override void Enter() {
        _audioService?.StartMusicPlaylist(MusicPlaylist.GameLoop);
    }


    public override void Exit() {
        _audioService?.StopCurrentMusic();
    }
}
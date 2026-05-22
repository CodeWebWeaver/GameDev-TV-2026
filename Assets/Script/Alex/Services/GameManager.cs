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
    [Inject] private InputManager inputManager;

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

    public void HandleCancel() {
        if (sceneDataService.IsMainMenu()) {
            uiService.ToggleSettings();
            return;
        } else {
            // toggle pause state
            bool isPaused = _stateMachine.CurrentState is PauseState pause;
            if (isPaused) {
                _stateMachine.ChangeState<GameLoopState>();
            } else {
                _stateMachine.ChangeState<PauseState>();
            }
        }
    }

    public void ResetActiveState() {
        bool inActiveLoop = _stateMachine.CurrentState is MainMenuState or GameLoopState;
        if (inActiveLoop) return;
        if (sceneDataService.IsMainMenu()) {
            _stateMachine.ChangeState<MainMenuState>();
        } else {
            _stateMachine.ChangeState<GameLoopState>();
        }
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
    private readonly InputManager _inputManager;

    public MainMenuState(IAudioService audioService, InputManager inputManager) {
        _audioService = audioService;
        _inputManager = inputManager;
    }

    public override void Enter() {
        _audioService?.ChangeMusicPlaylist(MusicPlaylist.MainMenu);
        _inputManager.SwitchToUIMap();
    }

    public override void Exit() {

    }
}

public class GameLoopState : State<GameManager> {
    private readonly IAudioService _audioService;
    private readonly InputManager _inputManager;

    public GameLoopState(IAudioService audioService, InputManager inputManager) {
        _audioService = audioService;
        _inputManager = inputManager;
    }

    public override void Enter() {
        _audioService?.ChangeMusicPlaylist(MusicPlaylist.GameLoop);
        _inputManager.SwitchToPlayerMap();
        Cursor.visible = false;
    }


    public override void Exit() {
        Cursor.visible = true;
    }
}

public class PauseState : State<GameManager> {
    private readonly IUiService _uiService;
    private readonly InputManager _inputManager;

    public PauseState(IUiService uiService, InputManager inputManager) {
        _uiService = uiService;
        _inputManager = inputManager;
    }

    public override void Enter() {
        _uiService.ShowPause();
        _inputManager.SwitchToUIMap();
        Cursor.visible = true;
    }

    public override void Exit() {
        _uiService.HidePause();
    }
}

public class DialogueState : State<GameManager> {
    private readonly IUiService _uiService;
    private readonly InputManager _inputManager;
    private readonly DialogueManager _dialogueService;

    public DialogueState(IUiService ui, InputManager input, DialogueManager dialogue) {
        _uiService = ui;
        _inputManager = input;
        _dialogueService = dialogue;
    }

    public override void Enter() {
        _inputManager.SwitchToDialogMap();
        Cursor.visible = true;
    }

    public override void Exit() {
        _inputManager.SwitchToPlayerMap();
    }
}
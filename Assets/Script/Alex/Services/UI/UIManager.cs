using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Zenject;


public interface IUiService {
    void HideAll();
    void ShowSettings();
    void TogglePanel(UIPanel panel);
}

public class UIManager : MonoBehaviour, IUiService {

    [SerializeField] UIPanel pauseUI;

    [SerializeField] UIPanel settingsPanel;

    [InjectOptional] ISceneDataService sceneDataService;
    [SerializeField] EventSystem eventSysPrefab;

    private void Awake() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1) {
        TryAddEventSystem();
    }

    private void OnDestroy() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void TryAddEventSystem() {
        EventSystem eventSystem = FindAnyObjectByType<EventSystem>();
        if (eventSystem != null) return;
        Instantiate(eventSysPrefab);
    }

    public void TogglePanel(UIPanel panel) {
        if (panel.IsOpen) {
            panel.Hide();
        } else {
            panel.Show();
        }
    }

    public void HandleCancel() {
        if (sceneDataService != null) {
            if (sceneDataService.IsMainMenu()) {
                TogglePanel(settingsPanel);
            } else {
                TogglePanel(pauseUI);
            }
        } else {
            TogglePanel(pauseUI);
        }
    }

    public void HideAll() {
        pauseUI.Hide();
        settingsPanel.Hide();
    }

    public void ShowSettings() {
        HandleCancel();
    }
}

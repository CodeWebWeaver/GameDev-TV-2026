using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Zenject;

public interface IUiService {
    void HideAll();
    void ShowSettings();
    void TogglePanel(UIPanel panel);
    void ShowPanel(UIPanel panel);
    void HidePanel(UIPanel panel);
    bool IsPanelOpen(UIPanel panel);
    GameObject InstantiateUIElement(GameObject uiPrefab, Transform parent = null);
    T InstantiateUIElement<T>(T uiPrefab, Transform parent = null) where T : Component;
}

public class UIManager : MonoBehaviour, IUiService {
    [Header("UI Panels")]
    [SerializeField] private UIPanel _pauseUI;
    [SerializeField] private UIPanel _settingsPanel;

    [Header("References")]
    [SerializeField] private EventSystem _eventSystemPrefab;

    [Header("Settings")]
    [SerializeField] private bool _autoCreateEventSystem = true;
    [SerializeField] private bool _logWarnings = true;

    [InjectOptional] private ISceneDataService _sceneDataService;
    [Inject] private DiContainer _diContainer;

    [SerializeField] private Canvas _canvas;
    private EventSystem _currentEventSystem;
    private readonly Dictionary<Type, UIPanel> _panelCache = new Dictionary<Type, UIPanel>();

    private void Awake() {
        InitializeCanvas();
        CachePanels();
        ValidateReferences();
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void InitializeCanvas() {
        if (_canvas == null) {
            _canvas = GetComponentInChildren<Canvas>();
        }

        if (_canvas == null) {
            _canvas = FindAnyObjectByType<Canvas>();

            if (_canvas == null && _logWarnings) {
                Debug.LogWarning($"[UIManager] No Canvas found on {gameObject.name} or in scene. UI elements will be instantiated without a parent.");
            }
        }
    }

    private void CachePanels() {
        CachePanel(_pauseUI);
        CachePanel(_settingsPanel);
    }

    private void CachePanel(UIPanel panel) {
        if (panel != null) {
            var type = panel.GetType();
            if (!_panelCache.ContainsKey(type)) {
                _panelCache[type] = panel;
            }
        }
    }

    private void ValidateReferences() {
        if (_pauseUI == null && _logWarnings)
            Debug.LogWarning($"[UIManager] Pause UI panel is not assigned on {gameObject.name}");

        if (_settingsPanel == null && _logWarnings)
            Debug.LogWarning($"[UIManager] Settings panel is not assigned on {gameObject.name}");

        if (_eventSystemPrefab == null && _autoCreateEventSystem && _logWarnings)
            Debug.LogWarning("[UIManager] EventSystem prefab is not assigned. EventSystem won't be auto-created.");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (_autoCreateEventSystem)
            TryAddEventSystem();
    }

    private void TryAddEventSystem() {
        if (_currentEventSystem != null && _currentEventSystem.gameObject != null)
            return;

        _currentEventSystem = FindAnyObjectByType<EventSystem>();

        if (_currentEventSystem != null)
            return;

        if (_eventSystemPrefab != null) {
            _currentEventSystem = Instantiate(_eventSystemPrefab);
            _currentEventSystem.name = "EventSystem (Auto-created)";
            DontDestroyOnLoad(_currentEventSystem.gameObject);

            if (_logWarnings)
                Debug.Log("[UIManager] EventSystem was auto-created.");
        } else if (_logWarnings) {
            Debug.LogWarning("[UIManager] Cannot create EventSystem: prefab is missing.");
        }
    }

    public void TogglePanel(UIPanel panel) {
        if (!ValidatePanel(panel, "toggle")) return;

        if (panel.IsOpen)
            panel.Hide();
        else
            panel.Show();
    }

    public void ShowPanel(UIPanel panel) {
        if (!ValidatePanel(panel, "show")) return;

        if (!panel.IsOpen)
            panel.Show();
    }

    public void HidePanel(UIPanel panel) {
        if (!ValidatePanel(panel, "hide")) return;

        if (panel.IsOpen)
            panel.Hide();
    }

    public bool IsPanelOpen(UIPanel panel) => panel != null && panel.IsOpen;

    public T GetPanel<T>() where T : UIPanel {
        var type = typeof(T);
        return _panelCache.TryGetValue(type, out var panel) ? panel as T : null;
    }

    public void HandleCancel() {
        UIPanel targetPanel = DetermineTargetPanel();

        if (targetPanel != null)
            TogglePanel(targetPanel);
        else if (_logWarnings)
            Debug.LogWarning("[UIManager] No target panel available for cancel action.");
    }

    private UIPanel DetermineTargetPanel() {
        if (_sceneDataService != null && _sceneDataService.IsMainMenu())
            return _settingsPanel;

        return _pauseUI;
    }

    public void HideAll() {
        HidePanel(_pauseUI);
        HidePanel(_settingsPanel);
    }

    public void ShowSettings() => HandleCancel();

    public GameObject InstantiateUIElement(GameObject uiPrefab, Transform parent = null) {
        if (uiPrefab == null) {
            LogError("Cannot instantiate null UI prefab");
            return null;
        }

        Transform targetParent = parent ?? GetValidCanvasTransform();

        if (targetParent == null) {
            if (_logWarnings)
                Debug.LogWarning("[UIManager] No valid parent or canvas found. Instantiating without parent.");

            return _diContainer != null ? _diContainer.InstantiatePrefab(uiPrefab) : Instantiate(uiPrefab);
        }

        try {
            if (_diContainer != null) {
                return _diContainer.InstantiatePrefab(uiPrefab, targetParent);
            } else {
                Debug.LogWarning("[UIManager] DiContainer is null, using standard Instantiate.");
                return Instantiate(uiPrefab, targetParent);
            }
        } catch (Exception ex) {
            LogError($"Failed to instantiate UI element through DI container: {ex.Message}");
            return Instantiate(uiPrefab, targetParent);
        }
    }

    public T InstantiateUIElement<T>(T uiPrefab, Transform parent = null) where T : Component {
        if (uiPrefab == null) {
            LogError($"Cannot instantiate null UI prefab of type {typeof(T).Name}");
            return null;
        }

        GameObject instance = InstantiateUIElement(uiPrefab.gameObject, parent);

        if (instance == null)
            return null;

        T component = instance.GetComponent<T>();

        if (component == null) {
            LogError($"Instantiated prefab doesn't have component of type {typeof(T).Name}");
            Destroy(instance);
            return null;
        }

        return component;
    }

    private Transform GetValidCanvasTransform() {
        if (_canvas != null && _canvas.gameObject.activeInHierarchy)
            return _canvas.transform;

        Canvas anyCanvas = FindAnyObjectByType<Canvas>();

        if (anyCanvas != null)
            return anyCanvas.transform;

        if (_logWarnings)
            LogError("No canvas found in scene!");

        return null;
    }

    private bool ValidatePanel(UIPanel panel, string action) {
        if (panel == null) {
            LogError($"Cannot {action} null panel");
            return false;
        }
        return true;
    }

    private void LogError(string message) {
        if (_logWarnings)
            Debug.LogError($"[UIManager] {message}");
    }
}
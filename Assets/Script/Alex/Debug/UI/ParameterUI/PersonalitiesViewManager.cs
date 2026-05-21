using FMODUnity;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;
using static UnityEngine.Audio.GeneratorInstance;

public class PersonalitiesViewManager : MonoBehaviour {
    [SerializeField] private PersonalityParamUI personalityPrefab;
    [SerializeField] private Transform container;
    [SerializeField] private bool usePooling = true;
    [SerializeField] private int maxVisibleParams = 20;

    private ObjectPool<PersonalityParamUI> _pool;
    private readonly Dictionary<string, PersonalityParamUI> _activeUI = new();
    private PlayerPersonality _personality;

    [Header ("Audio")]
    [InjectOptional] FmodAudioService _audioService;
    [SerializeField] private EventReference aquiredEventReference;

    private void Awake() {
        if (usePooling) {
            InitializePool();
        }
    }

    private void InitializePool() {
        _pool = new ObjectPool<PersonalityParamUI>(
            CreateItem,
            OnTakeFromPool,
            OnReturnedToPool,
            OnDestroyPoolObject,
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 50
        );
    }

    public void Observe(PlayerPersonality personality) {
        _personality = personality;

        // Підписуємось на події
        personality.OnParamAdded += HandleParamAdded;
        personality.OnParamChanged += HandleParamChanged;

        // Відображаємо всі існуючі параметри
        foreach (var param in personality.GetAll()) {
            HandleParamAdded(param);
        }
    }

    private void HandleParamAdded(PersonalityParam param) {
        if (_activeUI.ContainsKey(param.Name)) return;

        // Обмежуємо кількість відображуваних параметрів
        if (_activeUI.Count >= maxVisibleParams) {
            Debug.LogWarning($"Max visible params reached ({maxVisibleParams})");
            return;
        }

        var ui = usePooling ? _pool.Get() : InstantiateUI();
        _activeUI[param.Name] = ui;

        // Пряма передача PersonalityParam
        ui.Setup(param);
    }

    private PersonalityParamUI InstantiateUI() {
        return Instantiate(personalityPrefab, container);
    }

    private void HandleParamChanged(PersonalityParam param) {
        if (_activeUI.TryGetValue(param.Name, out var ui)) {
            // UI оновиться автоматично через підписку всередині ui.Setup()
            // Тому тут нічого не потрібно робити
        }

        _audioService?.PlayOneShot(aquiredEventReference);
    }

    private void OnTakeFromPool(PersonalityParamUI ui) {
        ui.gameObject.SetActive(true);
    }

    private void OnReturnedToPool(PersonalityParamUI ui) {
        ui.gameObject.SetActive(false);
    }

    private void OnDestroyPoolObject(PersonalityParamUI ui) {
        if (ui != null) Destroy(ui.gameObject);
    }

    private PersonalityParamUI CreateItem() {
        return Instantiate(personalityPrefab, container);
    }

    private void OnDestroy() {
        if (_personality != null) {
            _personality.OnParamAdded -= HandleParamAdded;
            _personality.OnParamChanged -= HandleParamChanged;
        }
    }
}

public class PlayerPersonality {
    private readonly Dictionary<string, PersonalityParam> _params = new();
    private readonly Dictionary<string, PersonalityParamSO> _availableParams = new();

    public event System.Action<PersonalityParam> OnParamChanged;
    public event System.Action<PersonalityParam> OnParamAdded;
    public void Initialize(List<PersonalityParamSO> allPossibleParams) {
        foreach (var paramData in allPossibleParams) {
            string name = paramData.Name.ToLowerInvariant();
            _availableParams[name] = paramData;

            // Створюємо всі параметри зі значенням за замовчуванням
            var param = new PersonalityParam(paramData, paramData.DefaultValue);
            _params[name] = param;
            param.OnValueChanged += OnParamValueChanged;
            OnParamAdded?.Invoke(param);
        }
    }

    public void ChangeParam(string name, int delta) {
        string loweredName = name.ToLowerInvariant();
        if (!_params.TryGetValue(loweredName, out var param)) {
            // Якщо параметра немає - створюємо новий (тільки якщо є SO)
            if (_availableParams.TryGetValue(loweredName, out var paramData)) {
                param = new PersonalityParam(paramData, 0);
                _params[loweredName] = param;
                param.OnValueChanged += OnParamValueChanged;
                OnParamAdded?.Invoke(param);
            } else {
                Debug.LogWarning($"Unknown personality param: {loweredName}. Create a PersonalityParamSO for it.");
                return;
            }
        }

        param.ChangeValue(delta);
    }

    private void OnParamValueChanged(PersonalityParam param) {
        OnParamChanged?.Invoke(param);
    }

    public PersonalityParam GetParam(string name) {
        _params.TryGetValue(name, out var param);
        return param;
    }

    public int GetParamValue(string name) {
        return _params.TryGetValue(name, out var param) ? param.CurrentValue : 0;
    }

    public List<PersonalityParam> GetAll() {
        return _params.Values.ToList();
    }
}

public class PersonalityParam {
    public PersonalityParamSO data;
    private int _currentValue;

    public int CurrentValue {
        get => _currentValue;
        private set {
            int newValue = Mathf.Clamp(value, data.MinValue, data.MaxValue);
            if (_currentValue != newValue) {
                _currentValue = newValue;
                OnValueChanged?.Invoke(this);
            }
        }
    }

    public string Name => data?.Name ?? "Unknown";

    public event System.Action<PersonalityParam> OnValueChanged;

    public PersonalityParam(PersonalityParamSO data, int initialValue = 0) {
        this.data = data;
        _currentValue = Mathf.Clamp(initialValue, data.MinValue, data.MaxValue);
    }

    public void ChangeValue(int delta) {
        CurrentValue += delta;
    }

    public void SetValue(int newValue) {
        CurrentValue = newValue;
    }
}
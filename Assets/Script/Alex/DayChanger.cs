using System;
using UnityEngine;
using Zenject;
using static DayNightController;

public class DayNightCycle : MonoBehaviour {
    [Header("Settings")]
    [SerializeField] private DayChangerSettingsSO settings;

    [Header("References")]
    [SerializeField] private Light sunLight;

    private float currentTimeOfDay;
    private DayPhase _currentPhase;
    private bool isRunning;

    public float CurrentTimeOfDay => currentTimeOfDay;
    public bool IsRunning => isRunning;

    [Inject] SignalBus signalBus;
    public event Action<float> OnTimeChanged;

    private void Awake() {
        if (settings == null) {
            Debug.LogError($"{nameof(DayNightCycle)}: Settings asset is missing.");
            enabled = false;
            return;
        }

        sunLight ??= FindSunLight();

        if (sunLight == null) {
            Debug.LogError($"{nameof(DayNightCycle)}: Directional Light not found.");
            enabled = false;
        }
    }

    private void Start() {
        currentTimeOfDay = settings.startTimeOfDay;

        _currentPhase = GetDayPhase();

        ApplyTime();

        if (settings.autoStart) {
            StartCycle();
        }
    }

    private void Update() {
        if (!isRunning)
            return;

        currentTimeOfDay += Time.deltaTime / settings.dayDurationInSeconds;

        if (currentTimeOfDay >= 1f) {
            currentTimeOfDay %= 1f;
        }

        ApplyTime();

        CheckPhaseChange();

        OnTimeChanged?.Invoke(currentTimeOfDay);
    }

    private void CheckPhaseChange() {
        DayPhase newPhase = GetDayPhase();

        if (newPhase == _currentPhase)
            return;

        _currentPhase = newPhase;

        signalBus.Fire(new DayCycleChangedSignal(newPhase));
    }

    private void ApplyTime() {
        UpdateSunRotation();
        UpdateEnvironment();
    }

    private void UpdateSunRotation() {
        float sunAngle = currentTimeOfDay * 360f - 90f;

        sunLight.transform.rotation =
            Quaternion.Euler(sunAngle, 170f, 0f);
    }

    private void UpdateEnvironment() {
        sunLight.color =
            settings.sunColorGradient.Evaluate(currentTimeOfDay);

        float intensity =
            settings.lightIntensityCurve.Evaluate(currentTimeOfDay);

        sunLight.intensity =
            Mathf.Lerp(settings.minIntensity,
                       settings.maxIntensity,
                       intensity);

        RenderSettings.ambientLight =
            settings.ambientColorGradient.Evaluate(currentTimeOfDay);

        if (RenderSettings.fog) {
            RenderSettings.fogColor =
                settings.fogColorGradient.Evaluate(currentTimeOfDay);
        }
    }

    private static Light FindSunLight() {
        foreach (var light in FindObjectsByType<Light>()) {
            if (light.type == LightType.Directional) {
                return light;
            }
        }

        return null;
    }

    public void StartCycle() => isRunning = true;

    public void StopCycle() => isRunning = false;

    public void ToggleCycle() => isRunning = !isRunning;

    public void SetTimeOfDay(float normalizedTime) {
        currentTimeOfDay = Mathf.Repeat(normalizedTime, 1f);

        ApplyTime();
        OnTimeChanged?.Invoke(currentTimeOfDay);
    }

    public string GetTimeString() {
        int totalMinutes =
            Mathf.FloorToInt(currentTimeOfDay * 24f * 60f);

        int hours = totalMinutes / 60;
        int minutes = totalMinutes % 60;

        return $"{hours:00}:{minutes:00}";
    }

    public DayPhase GetDayPhase() {
        return currentTimeOfDay switch {
            < 0.25f => DayPhase.Night,
            < 0.30f => DayPhase.Dawn,
            < 0.70f => DayPhase.Day,
            < 0.75f => DayPhase.Dusk,
            _ => DayPhase.Night
        };
    }
}

public class DayCycleChangedSignal {
    public DayPhase DayPhase;

    public DayCycleChangedSignal(DayPhase dayPhase) {
        this.DayPhase = dayPhase;
    }
}
using System;
using UnityEngine;
using Zenject;

public class NightLamp : MonoBehaviour
{
    [Inject] SignalBus signalBus;
    [SerializeField] Light spotLight;

    private void Awake() {
        TurnOff();
    }

    private void OnEnable() {
        signalBus?.Subscribe<DayCycleChangedSignal>(HandleCycleChanged);
    }

    private void OnDisable() {
        signalBus?.Unsubscribe<DayCycleChangedSignal>(HandleCycleChanged);
    }

    private void HandleCycleChanged(DayCycleChangedSignal signal) {
        bool shouldBeOn =
            signal.DayPhase == DayNightController.DayPhase.Dusk ||
            signal.DayPhase == DayNightController.DayPhase.Night;

        spotLight.enabled = shouldBeOn;
    }

    public void TurnOn() {
        spotLight.enabled = true;
    }

    public void TurnOff() {
        spotLight.enabled = false;
    }
}

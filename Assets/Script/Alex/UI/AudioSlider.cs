using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class AudioSlider : MonoBehaviour {
    [Header("References")]
    [SerializeField] private SliderValue sliderValue;
    [SerializeField] private Toggle muteToggle;
    [Header("Audio Settings")]
    [SerializeField] private AudioChannelType channel = AudioChannelType.Master;
    [Inject] IAudioService audioService;
    public AudioChannelType Channel => channel;

    public UnityEvent<AudioChannelType, float> OnVolumeChanged;

    [Inject]
    public void Construct(IAudioService service) {
        this.audioService = service;
    }

    private void Start() {
        sliderValue.OnValueChanged.AddListener(HandleSliderValueChanged);
        sliderValue.SetValue(audioService.GetVolume(channel), false);
        muteToggle.onValueChanged.AddListener(HandleMuteToggleChanged);

    }

    private void HandleMuteToggleChanged(bool isEnabled) {
        audioService.SetBusMute(Channel, !isEnabled);
    }

    private void HandleSliderValueChanged(float normalizedValue) {
        OnVolumeChanged?.Invoke(channel, normalizedValue);
        audioService.SetVolume(channel, normalizedValue);
    }

    public void SetVolume(float normalizedValue, bool triggerEvent = false) {
        sliderValue.SetValue(normalizedValue, triggerEvent);
    }

    public float GetVolume() {
        return sliderValue.GetValue();
    }

    private void OnDestroy() {
        sliderValue.OnValueChanged.RemoveListener(HandleSliderValueChanged);
    }
}
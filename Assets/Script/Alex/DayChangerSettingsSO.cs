using System;
using UnityEngine;

[CreateAssetMenu(
    fileName = "DayChangerSettings",
    menuName = "Game/Day Night Settings")]
public class DayChangerSettingsSO : ScriptableObject {
    [Header("Cycle")]
    [Min(1f)]
    public float dayDurationInSeconds = 120f;

    public bool autoStart = true;

    [Range(0f, 1f)]
    public float startTimeOfDay = 0.3f;

    [Header("Lighting")]
    public Gradient sunColorGradient;
    public Gradient ambientColorGradient;
    public Gradient fogColorGradient;

    [Header("Intensity")]
    public AnimationCurve lightIntensityCurve =
        AnimationCurve.Linear(0f, 0f, 1f, 1f);

    [Min(0f)]
    public float minIntensity = 0.1f;

    [Min(0f)]
    public float maxIntensity = 1.5f;

    private void OnValidate() {
        maxIntensity = Mathf.Max(maxIntensity, minIntensity);
    }
}
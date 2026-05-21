// =============================================================
// DayNightController
// =============================================================
// Controls day/night transitions by animating the sun light's
// X rotation, ambient color, and light intensity.
//
// HOW TO TRIGGER A TRANSITION FROM ANOTHER SCRIPT:
//
//   DayNightController.Instance.TransitionToPhase(DayNightController.DayPhase.Morning);
//   DayNightController.Instance.TransitionToPhase(DayNightController.DayPhase.Afternoon);
//   DayNightController.Instance.TransitionToPhase(DayNightController.DayPhase.Night);
//
// If a transition is already running, it will be interrupted
// and the new one will start from the current state.
// =============================================================

using System.Collections;
using UnityEngine;

public class DayNightController : MonoBehaviour
{
    public static DayNightController Instance { get; private set; }

    [Header("Sun Light")]
    [SerializeField] private Light sunLight;
    [SerializeField] private float transitionDuration = 3f;

    [Header("Angles per phase (X axis)")]
    [SerializeField] private float morningAngle  = 30f;
    [SerializeField] private float afternoonAngle = 60f;
    [SerializeField] private float nightAngle     = 150f;

    [Header("Ambient colors per phase")]
    [SerializeField] private Color morningAmbient   = new Color(1f, 0.9f, 0.7f);
    [SerializeField] private Color afternoonAmbient = new Color(1f, 0.95f, 0.8f);
    [SerializeField] private Color nightAmbient     = new Color(0.05f, 0.05f, 0.1f);

    [Header("Light intensity per phase")]
    [SerializeField] private float morningIntensity   = 0.8f;
    [SerializeField] private float afternoonIntensity = 1.2f;
    [SerializeField] private float nightIntensity     = 0f;

    private Coroutine _currentTransition;

    public enum DayPhase { Morning, Afternoon, Night }

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void TransitionToPhase(DayPhase phase)
    {
        if (_currentTransition != null)
            StopCoroutine(_currentTransition);

        _currentTransition = StartCoroutine(DoTransition(phase));
    }

    private IEnumerator DoTransition(DayPhase phase)
    {
        float startAngle     = sunLight.transform.eulerAngles.x;
        Color startAmbient   = RenderSettings.ambientLight;
        float startIntensity = sunLight.intensity;

        float targetAngle;
        Color targetAmbient;
        float targetIntensity;

        switch (phase)
        {
            case DayPhase.Morning:
                targetAngle     = morningAngle;
                targetAmbient   = morningAmbient;
                targetIntensity = morningIntensity;
                break;
            case DayPhase.Afternoon:
                targetAngle     = afternoonAngle;
                targetAmbient   = afternoonAmbient;
                targetIntensity = afternoonIntensity;
                break;
            default: // Night
                targetAngle     = nightAngle;
                targetAmbient   = nightAmbient;
                targetIntensity = nightIntensity;
                break;
        }

        float elapsed = 0f;
        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / transitionDuration; 

            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            // Aplica los tres cambios simultáneamente
            sunLight.transform.rotation = Quaternion.Euler(
                Mathf.LerpAngle(startAngle, targetAngle, smoothT), 0f, 0f);

            RenderSettings.ambientLight = Color.Lerp(startAmbient, targetAmbient, smoothT);
            sunLight.intensity          = Mathf.Lerp(startIntensity, targetIntensity, smoothT);

            yield return null; 
        }

        _currentTransition = null;
    }
}

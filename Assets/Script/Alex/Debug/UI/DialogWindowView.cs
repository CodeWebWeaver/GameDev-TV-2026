using FMODUnity;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DialogWindowView : MonoBehaviour {
    [Header("General")]
    [SerializeField] private GameObject dialoguePanel;

    [Header("Name")]
    [SerializeField] private GameObject nameObject;
    [SerializeField] private TextMeshProUGUI speakerNameText;

    [Header("Portrait")]
    [SerializeField] private GameObject portraitObject;
    [SerializeField] private Image speakerPortraitImage;

    [Header("Dialog")]
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Typewriter")]
    [SerializeField] private bool useTypewriter = true;
    [SerializeField][Range(10f, 120f)] private float charsPerSecond = 40f;

    // ── Events ───────────────────────────────────────────────────────────
    /// <summary>
    /// Fires when the typewriter finishes naturally or via TrySkipTypewriter().
    /// Does NOT fire when StopTypewriter() is called internally (e.g. on new text).
    /// </summary>
    public event Action OnTypewriterFinished;

    // ── State ────────────────────────────────────────────────────────────
    public bool IsTyping { get; private set; }

    private Coroutine _typewriterCoroutine;
    private string _currentFullText = string.Empty;

    [InjectOptional] FmodAudioService audioService;
    [SerializeField] EventReference typingEvent;
    private const string InstanceKey = "typing"; 

    // ── Lifecycle ─────────────────────────────────────────────────────────
    private void OnDestroy() {
        StopTypewriter(fireEvent: false);
    }

    // ── Dialogue ─────────────────────────────────────────────────────────

    public void SetDialogueText(string text) {
        StopTypewriter(fireEvent: false);

        _currentFullText = text ?? string.Empty;

        if (useTypewriter && !string.IsNullOrEmpty(_currentFullText)) {
            _typewriterCoroutine = StartCoroutine(TypewriterRoutine(_currentFullText));
        } else {
            dialogueText.text = _currentFullText;
            dialogueText.maxVisibleCharacters = int.MaxValue;
            IsTyping = false;
            OnTypewriterFinished?.Invoke();
        }
    }

    public bool TrySkipTypewriter() {
        if (!IsTyping) return false;
        Debug.Log("[DialogWindowView] Typewriter skipped by user input.");

        StopTypewriter(fireEvent: false);

        dialogueText.text = _currentFullText;
        dialogueText.maxVisibleCharacters = int.MaxValue;
        IsTyping = false;
        OnTypewriterFinished?.Invoke();
        return true;
    }

    public void ClearDialogueText() {
        StopTypewriter(fireEvent: false);
        _currentFullText = string.Empty;
        dialogueText.text = string.Empty;
    }

    public void SetSpeakerName(string speakerName) => speakerNameText.text = speakerName;
    public void ShowSpeakerName() => nameObject.SetActive(true);
    public void HideSpeakerName() => nameObject.SetActive(false);

    // ── Portrait ──────────────────────────────────────────────────────────
    public void SetPortrait(Sprite portrait) {
        if (speakerPortraitImage != null)
        speakerPortraitImage.sprite = portrait;
    }
    public void ShowPortrait() => portraitObject.SetActive(true);
    public void HidePortrait() => portraitObject.SetActive(false);

    // ── Panel ─────────────────────────────────────────────────────────────
    public void ShowDialoguePanel() => dialoguePanel.SetActive(true);

    public void HideDialoguePanel() {
        StopTypewriter(fireEvent: false);
        dialoguePanel.SetActive(false);
    }

    // ── Typewriter internals ──────────────────────────────────────────────
    private IEnumerator TypewriterRoutine(string fullText) {
        IsTyping = true;
        dialogueText.text = fullText;
        dialogueText.maxVisibleCharacters = 0;

        audioService?.PlayLooped(InstanceKey, typingEvent);

        // Кэшируем delay вне цикла
        float delay = 1f / charsPerSecond;
        int total = fullText.Length;

        for (int i = 1; i <= total; i++) {
            dialogueText.maxVisibleCharacters = i;
            yield return new WaitForSeconds(delay);
        }

        audioService?.StopLooped(InstanceKey);
        IsTyping = false;
        _typewriterCoroutine = null;
        OnTypewriterFinished?.Invoke();
    }
    private void StopTypewriter(bool fireEvent) {
        if (_typewriterCoroutine != null) {
            StopCoroutine(_typewriterCoroutine);
            _typewriterCoroutine = null;
        }

        audioService?.StopLooped(InstanceKey);
        IsTyping = false;

        if (fireEvent) {
            OnTypewriterFinished?.Invoke();
        }
    }
}
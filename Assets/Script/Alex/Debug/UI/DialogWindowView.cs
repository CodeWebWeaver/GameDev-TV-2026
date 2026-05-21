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
    /// <summary>Fires when the typewriter finishes (or is skipped).</summary>
    public event Action OnTypewriterFinished;

    // ── State ────────────────────────────────────────────────────────────
    public bool IsTyping { get; private set; }

    private Coroutine _typewriterCoroutine;

    [InjectOptional] FmodAudioService audioService;
    [SerializeField] EventReference typingEvent;
    private string instance_key = "typing";

    // ── Dialogue ─────────────────────────────────────────────────────────

    /// <summary>
    /// Displays <paramref name="text"/>.
    /// If the typewriter flag is on, reveals it character-by-character;
    /// otherwise shows the full string immediately.
    /// </summary>
    public void SetDialogueText(string text) {
        StopTypewriter();

        if (useTypewriter && !string.IsNullOrEmpty(text)) {
            _typewriterCoroutine = StartCoroutine(TypewriterRoutine(text));
        } else {
            dialogueText.text = text;
            IsTyping = false;
            OnTypewriterFinished?.Invoke();
        }
    }

    /// <summary>
    /// If typing is still in progress — reveals the full text immediately.
    /// Returns <c>true</c> when the text was completed this call (i.e. was
    /// still typing), so the caller knows NOT to advance the dialogue yet.
    /// Returns <c>false</c> when text was already fully shown.
    /// </summary>
    public bool TrySkipTypewriter() {
        if (!IsTyping) return false;

        StopTypewriter();
        // dialogueText.text already contains the full string because the
        // coroutine sets it character-by-character via maxVisibleCharacters,
        // so we just reveal everything.
        dialogueText.maxVisibleCharacters = dialogueText.text.Length;
        IsTyping = false;
        OnTypewriterFinished?.Invoke();
        return true;
    }

    public void ClearDialogueText() {
        StopTypewriter();
        dialogueText.text = string.Empty;
    }

    // ── Speaker Name ──────────────────────────────────────────────────────
    public void SetSpeakerName(string speakerName) => speakerNameText.text = speakerName;
    public void ShowSpeakerName() => nameObject.SetActive(true);
    public void HideSpeakerName() => nameObject.SetActive(false);

    // ── Portrait ──────────────────────────────────────────────────────────
    public void SetPortrait(Sprite portrait) => speakerPortraitImage.sprite = portrait;
    public void ShowPortrait() => portraitObject.SetActive(true);
    public void HidePortrait() => portraitObject.SetActive(false);

    // ── Panel ─────────────────────────────────────────────────────────────
    public void ShowDialoguePanel() => dialoguePanel.SetActive(true);

    public void HideDialoguePanel() {
        StopTypewriter();
        dialoguePanel.SetActive(false);
    }

    // ── Typewriter internals ──────────────────────────────────────────────
    private IEnumerator TypewriterRoutine(string fullText) {
        IsTyping = true;
        dialogueText.text = fullText;              // load full text into TMP
        dialogueText.maxVisibleCharacters = 0;     // hide all characters
        if (audioService != null) {
            audioService.PlayLooped(instance_key, typingEvent);
        }

        float delay = 1f / charsPerSecond;
        int total = fullText.Length;

        for (int i = 0; i <= total; i++) {
            dialogueText.maxVisibleCharacters = i;
            if (i < total) yield return new WaitForSeconds(delay);
        }

        if (audioService != null) {
            audioService.StopLooped(instance_key);
        }
        IsTyping = false;
        _typewriterCoroutine = null;
        OnTypewriterFinished?.Invoke();
    }

    private void StopTypewriter() {
        if (_typewriterCoroutine != null) {
            StopCoroutine(_typewriterCoroutine);
            _typewriterCoroutine = null;
        }
        if (audioService != null) {
            audioService.StopLooped(instance_key);
        }
        // Reset TMP visibility so the full string is always shown after stop
        dialogueText.maxVisibleCharacters = int.MaxValue;
        IsTyping = false;
    }
}
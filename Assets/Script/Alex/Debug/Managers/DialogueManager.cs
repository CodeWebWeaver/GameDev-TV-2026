using DG.Tweening.Core.Easing;
using Ink.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;


public interface IDialogInputManager {
    public event Action OnContinueRequest;
    public event Action OnSubmitRequest;
    public event Action<Vector2> OnNavigate;
}

public class DialogInputService : IDialogInputManager, IDisposable {
    private readonly InputSystem_Actions.DialogActions _uiActions;

    public event Action OnSubmitRequest;
    public event Action OnContinueRequest;

    public event Action<Vector2> OnNavigate;

    public DialogInputService(InputManager inputManager) {
        _uiActions = inputManager.InputActions.Dialog;
        Subscribe();
    }

    private void Subscribe() {
        _uiActions.Submit.performed += HandleSubmit;

        _uiActions.Click.canceled += HandleSkip;
        _uiActions.Continue.canceled += HandleSkip;

        _uiActions.Navigate.performed += HandleNavigation;
    }

    private void Unsubscribe() {
        _uiActions.Submit.performed -= HandleSubmit;

        _uiActions.Click.canceled -= HandleSkip;
        _uiActions.Continue.canceled -= HandleSkip;

        _uiActions.Navigate.performed -= HandleNavigation;
    }

    private void HandleSkip(InputAction.CallbackContext context) {
        OnContinueRequest?.Invoke();
    }

    private void HandleSubmit(InputAction.CallbackContext context) {
        OnSubmitRequest?.Invoke();
    }

    private void HandleNavigation(InputAction.CallbackContext context) {
        OnNavigate?.Invoke(context.ReadValue<Vector2>());
    }


    public void Dispose() {
        Unsubscribe();
    }
}

/// <summary>
/// Drives an Ink story and routes input.
/// All choice UI is delegated to <see cref="ChoiceSelector"/>.
/// Supports an optional typewriter effect in <see cref="DialogWindowView"/>:
///   • First Submit while text is typing  → reveals full text immediately.
///   • Second Submit (text fully shown)   → advances the dialogue.
/// </summary>
public class DialogueManager : MonoBehaviour {
    // ── Dependencies ──────────────────────────────────────────────────────
    [Inject] private UIManager uIManager;
    [Inject] private GameManager gameManager;
    [Inject] IDialogInputManager dialogInputs;
    [Inject] SignalBus signalBus;

    [SerializeField] private GameObject dialogWindowPrefab;
    [SerializeField] private TextAsset inkJsonAsset;

    // ── Runtime references ────────────────────────────────────────────────
    private ChoiceSelector _choiceSelector;
    private DialogWindowView _dialogWindow;

    private Story _story;
    private bool _waitingForInput;

    private Player _player;
    private DialogueNPC _npc;

    private readonly Dictionary<string, Human> _speakers = new();

    // ── Public state ──────────────────────────────────────────────────────
    public bool IsDialoguePlaying { get; private set; }

    // ── Events ────────────────────────────────────────────────────────────
    public event Action OnDialogueBegin;
    public event Action OnDialogueEnd;

    private const string PlayerNameVar = "player_name";
    private const string PlayerFriendsCountVar = "player_friends_count";
    private const string addFriendFunc = "add_friend";

    // ── Unity lifecycle ───────────────────────────────────────────────────
    protected void Awake() {
        InitUI();
        InitStory();
    }

    private void Start() {
        ExitDialogueMode();

        _choiceSelector.OnChoiceSelected += OnChoiceSelected;
    }

    private void Subscribe() {
        dialogInputs.OnSubmitRequest += HandleSubmit;
        dialogInputs.OnContinueRequest += HandleSkip;
        dialogInputs.OnNavigate += HandleNavigation;
    }

    private void Unsubscribe() {
        dialogInputs.OnSubmitRequest -= HandleSubmit;
        dialogInputs.OnContinueRequest -= HandleSkip;
        dialogInputs.OnNavigate -= HandleNavigation;
    }

    private void OnEnable() {
        Subscribe();
    }

    private void OnDisable() {
        Unsubscribe();
    }

    protected void OnDestroy() {
        if (_choiceSelector != null)
            _choiceSelector.OnChoiceSelected -= OnChoiceSelected;
    }

    // ── Initialisation helpers ────────────────────────────────────────────
    private void InitUI() {
        GameObject go = uIManager.InstantiateUIElement(dialogWindowPrefab);
        if (go == null) {
            Debug.LogError("[DialogueManager] Failed to instantiate dialogWindowPrefab.");
            return;
        }

        _choiceSelector = go.GetComponent<ChoiceSelector>();
        _dialogWindow = go.GetComponent<DialogWindowView>();
    }

    private void InitStory() {
        if (inkJsonAsset == null) {
            Debug.LogWarning("[DialogueManager] inkJsonAsset is null — aborting story init.");
            return;
        }

        _story = new Story(inkJsonAsset.text);

        _story.BindExternalFunction(addFriendFunc, (string name) => {
            if (_npc == null || _player == null) return;
            if (!EqualStringsInvariant(name, _npc.Name)) return;
            
            _npc.FriendSystem.AddFriend(_player);
            _player.FriendSystem.AddFriend(_npc);
        });

        _story.variablesState.variableChangedEvent += OnInkVariableChanged;
    }

    // ── Ink variable listener ─────────────────────────────────────────────
    private void OnInkVariableChanged(string varName, Ink.Runtime.Object newValue) {
        if (newValue is not Ink.Runtime.IntValue intVal) return;

        PersonalityParam param = _player?.Personality.GetParam(varName);
        if (param == null) return;

        param.SetValue(intVal.value);
        Debug.Log($"[INK_VAR] {varName} → {intVal.value}");
    }

    // ── Public entry point ────────────────────────────────────────────────
    public void EnterDialogueMode(string dialogueKnot, Player player, DialogueNPC npc) {
        if (IsDialoguePlaying) {
            Debug.LogWarning("[DialogueManager] Dialogue already playing — ignoring.");
            return;
        }
        if (string.IsNullOrEmpty(dialogueKnot)) {
            Debug.LogWarning("[DialogueManager] dialogueKnot is null or empty — aborting.");
            return;
        }

        gameManager?.StateMachine.ChangeState<DialogueState>();
        signalBus.Fire(new DialogStartedSignal(player, npc));

        _player = player;
        _npc = npc;

        _story.ChoosePathString(dialogueKnot);
        _story.variablesState[PlayerNameVar] = player.Name;
        _story.variablesState[PlayerFriendsCountVar] = player.FriendSystem.Friends.Count;

        CacheSpeaker(player);
        CacheSpeaker(npc);

        IsDialoguePlaying = true;
        _dialogWindow.ShowDialoguePanel();
        OnDialogueBegin?.Invoke();

        ContinueStory();
    }

    private void HandleSkip() {
        if (_dialogWindow.TrySkipTypewriter()) return;

        // Text is fully visible; advance on next Skip.
        if (_waitingForInput) {
            _waitingForInput = false;
            ContinueStory();
        }
    }


    // ── Input handlers ────────────────────────────────────────────────────
    private void HandleSubmit() {
        if (!IsDialoguePlaying) return;

        // Choices take priority; let ChoiceSelector handle confirmation.
        if (_choiceSelector.IsShowingChoices) {
            _choiceSelector.ConfirmSelection();
            return;
        }
    }

    private void HandleNavigation(Vector2 navigation) {
        if (!IsDialoguePlaying || !_choiceSelector.IsShowingChoices) return;

        float x = navigation.x;
        if (x > 0.5f) _choiceSelector.Navigate(+1);
        else if (x < -0.5f) _choiceSelector.Navigate(-1);
    }

    // ── Story progression ─────────────────────────────────────────────────
    private void ContinueStory() {
        Debug.Log("[DialogueManager] Continuing story...");
        // Advance past empty, tag-only lines that have no visible text.
        while (_story.canContinue) {
            string rawLine = _story.Continue();

            bool hasText = !string.IsNullOrWhiteSpace(rawLine);
            bool hasChoices = _story.currentChoices.Count > 0;
            bool hasTags = _story.currentTags.Count > 0;

            if (!hasText && !hasChoices && !hasTags) continue;

            ProcessEventTags(_story.currentTags);

            string speakerName = GetSpeakerName(_story.currentTags);
            if (speakerName.Equals("Player")) {
                Human human = _speakers.Values.Where(speaker => speaker is Player).First();
                if (human != null) {
                    speakerName = human.Name;
                } 
            }
            Sprite speakerPortrait = GetSpeakerPortrait(speakerName);

            ShowLine(rawLine.Trim(), speakerName, speakerPortrait);

            if (hasChoices)
                _choiceSelector.Show(_story.currentChoices);
            else
                _waitingForInput = true;

            return;
        }

        // Choices without preceding text.
        if (_story.currentChoices.Count > 0) {
            ShowLine(string.Empty, string.Empty, null);
            _choiceSelector.Show(_story.currentChoices);
            return;
        }

        ExitDialogueMode();
    }

    private void ShowLine(string line, string speakerName, Sprite speakerPortrait) {
        _dialogWindow.SetDialogueText(line);

        bool hasName = !string.IsNullOrEmpty(speakerName);

        if (hasName) {
            _dialogWindow.ShowSpeakerName();
            _dialogWindow.SetSpeakerName(speakerName);
        } else {
            _dialogWindow.HideSpeakerName();
        }

        if (hasName && speakerPortrait != null) {
            _dialogWindow.SetPortrait(speakerPortrait);
            _dialogWindow.ShowPortrait();
        } else {
            _dialogWindow.HidePortrait();
        }
    }

    // ── Tag parsing ───────────────────────────────────────────────────────
    private string GetSpeakerName(List<string> tags) {
        foreach (string tag in tags)
            if (TryParseTag(tag, out string key, out string value) &&
                key.Equals("speaker", StringComparison.OrdinalIgnoreCase))
                return value;

        return string.Empty;
    }

    private Sprite GetSpeakerPortrait(string speakerName) {
        if (string.IsNullOrWhiteSpace(speakerName)) return null;
        string normalized = NormalizeName(speakerName);
        if (_speakers.TryGetValue(normalized, out Human human))
            return human.Portrait;

        return null;
    }

    private void ProcessEventTags(List<string> tags) {
        foreach (string tag in tags) {
            if (!TryParseTag(tag, out string key, out string value)) continue;

            switch (key.ToLowerInvariant()) {
                case "anim": DialogueEvents.FireAnimationTag(value); break;
                case "sfx": DialogueEvents.FireSfxTag(value); break;
            }
        }
    }

    private static bool TryParseTag(string tag, out string key, out string value) {
        key = string.Empty;
        value = string.Empty;

        if (string.IsNullOrWhiteSpace(tag)) return false;

        int colon = tag.IndexOf(':');
        if (colon <= 0 || colon >= tag.Length - 1) return false;

        key = tag[..colon].Trim();
        value = tag[(colon + 1)..].Trim();
        return true;
    }

    // ── Choice callback ───────────────────────────────────────────────────
    private void OnChoiceSelected(Choice choice) {
        _story.ChooseChoiceIndex(choice.index);
        ContinueStory();
    }

    // ── Exit ──────────────────────────────────────────────────────────────
    private void ExitDialogueMode() {
        IsDialoguePlaying = false;
        _waitingForInput = false;

        _dialogWindow.HideDialoguePanel();
        _choiceSelector.Hide();
        OnDialogueEnd?.Invoke();
        signalBus.Fire(new DialogEndSignal(_player, _npc));
        gameManager.ResetActiveState();
    }

    // ── Helpers ───────────────────────────────────────────────────────────
    private void CacheSpeaker(Human human) {
        if (human == null) return;
        string normalizedName = NormalizeName(human.Name);
        _speakers[normalizedName] = human;
    }

    private void ChangeHappiness(string name, int delta) {
        if (_player.Name.Equals(name)) _player.ChangeHappiness(delta);
        else if (_npc.Name.Equals(name)) _npc.ChangeHappiness(delta);
    }

    private static string NormalizeName(string name) {
        if (string.IsNullOrWhiteSpace(name))
            return string.Empty;

        return string.Join(
            " ",
            name.Trim()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
        );
    }

    private static bool EqualStringsInvariant(string value1, string value2) {
        return string.Equals(
            NormalizeName(value1),
            NormalizeName(value2),
            StringComparison.OrdinalIgnoreCase);
    }
}


// ── Static event bus ──────────────────────────────────────────────────────────
public static class DialogueEvents {
    public static event Action<string> OnAnimationTag;
    public static event Action<string> OnSfxTag;
    public static event Action<string> OnSpeakerChanged;

    public static void FireAnimationTag(string tag) => OnAnimationTag?.Invoke(tag);
    public static void FireSfxTag(string tag) => OnSfxTag?.Invoke(tag);
    public static void FireSpeakerChanged(string speaker) => OnSpeakerChanged?.Invoke(speaker);
}


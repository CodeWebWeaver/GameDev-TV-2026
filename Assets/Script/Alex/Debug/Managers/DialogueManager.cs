using Ink.Runtime;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

/// <summary>
/// Drives an Ink story and routes input.
/// All choice UI is delegated to <see cref="ChoiceSelector"/>.
/// </summary>
public class DialogueManager : Singleton<DialogueManager> {
    [Header("UI")]
    
    [SerializeField] private ChoiceSelector choiceSelector;
    [SerializeField] private DialogWindowView dialogWindow;
    

    public bool IsDialoguePlaying { get; private set; }

    private Story _story;
    private bool _waitingForInput;
    private InputSystem_Actions.UIActions _uiMap;

    public event Action OnDialogueBegin;
    public event Action OnDialogueEnd;

    [SerializeField] private TextAsset inkJsonAsset;

    private Dictionary<string, Human> speakers = new();
    [InjectOptional] InputManager inputManager;
    protected override void Awake() {
        base.Awake();

        if (inkJsonAsset == null) {
            Debug.LogWarning("[DialogueManager] inkJsonAsset is null — aborting.");
            return;
        }
        _story = new Story(inkJsonAsset.text);

        _story.BindExternalFunction("addFriend", (string name) => {
            if (npc == null || player == null) return;
            if (name == npc.Name) {
                npc.AddFriend(player);
                player.AddFriend(npc);
            }
        });

        // Підписуємося на ВСІ зміни змінних в Ink
        _story.variablesState.variableChangedEvent += (string varName, Ink.Runtime.Object newValue) => {

            if (newValue is Ink.Runtime.IntValue intVal) {
                int actualValue = intVal.value;

                PersonalityParam personalityParam = player?.Personality.GetParam(varName);

                if (personalityParam != null) {
                    personalityParam.SetValue(actualValue);
                    Debug.Log($"[INK_VAR] {varName} changed. New value: {actualValue}");
                }
            }
        };
    }

    private void Start() {
        ExitDialogueMode();
        choiceSelector.OnChoiceSelected += OnChoiceSelected;

        if (inputManager != null) {
            _uiMap = inputManager.InputActions.UI;
            _uiMap.Submit.performed += HandleSubmit;
            _uiMap.Navigate.performed += HandleNavigation;
        }
        
        
    }

    protected override void OnDestroy() {
        base.OnDestroy();
        if (inputManager != null) {
            _uiMap.Submit.performed -= HandleSubmit;
            _uiMap.Navigate.performed -= HandleNavigation;
            choiceSelector.OnChoiceSelected -= OnChoiceSelected;
        }
        choiceSelector.OnChoiceSelected -= OnChoiceSelected;
    }

    private Player player;
    private DialogueNPC npc;
    private string currentSpeaker;

    // ── Public entry point ────────────────────────────────────────────────
    public void EnterDialogueMode(DialogueNodeSO dialogueNodeSO, Player player, DialogueNPC npc) {
        this.player = player;
        this.npc = npc;

        if (IsDialoguePlaying) {
            Debug.LogWarning("[DialogueManager] Dialogue already playing — ignoring.");
            return;
        }
        if (dialogueNodeSO == null) {
            Debug.LogWarning("[DialogueManager] dialogueNodeSO is null — aborting.");
            return;
        }

        _story.ChoosePathString(dialogueNodeSO.InkKnotName);
        // Inject values directly into Ink variables
        _story.variablesState["player_name"] = player.Name;
        _story.variablesState["player_friends_count"] = player.GetFriendCount();

        CacheSpeakerSprite(player);
        CacheSpeakerSprite(npc);

        IsDialoguePlaying = true;
        dialogWindow.ShowDialoguePanel();
        ContinueStory();
        OnDialogueBegin?.Invoke();
    }

    private void CacheSpeakerSprite(Human human) {
        if (human != null && human.personDataSO != null) {
            speakers[human.Name] = human;
        }
    }
    private void ChangeHappiness(string name, int delta) {
        Human toChange = null;
        if (player.Name.Equals(name)) toChange = player;
        if (npc.Name.Equals(name)) toChange = npc;
        if (toChange != null) toChange.ChangeHappiness(delta);
    }

    // ── Input handlers ────────────────────────────────────────────────────
    private void HandleSubmit(InputAction.CallbackContext ctx) {
        if (!IsDialoguePlaying) return;

        if (choiceSelector.IsShowingChoices)
            choiceSelector.ConfirmSelection();
        else if (_waitingForInput) {
            _waitingForInput = false;
            ContinueStory();
        }
    }

    private void HandleNavigation(InputAction.CallbackContext ctx) {
        if (!IsDialoguePlaying || !choiceSelector.IsShowingChoices) return;

        float x = ctx.ReadValue<Vector2>().x;
        if (x > 0.5f) choiceSelector.Navigate(+1);
        else if (x < -0.5f) choiceSelector.Navigate(-1);
    }

    private void ContinueStory() {
        while (_story.canContinue) {
            string rawLine = _story.Continue();

            bool hasText = !string.IsNullOrWhiteSpace(rawLine);
            bool hasChoices = _story.currentChoices.Count > 0;
            bool hasTags = _story.currentTags.Count > 0;

            if (!hasText && !hasChoices && !hasTags) continue;

            string line = rawLine.Trim();
            ProcessEventTags(_story.currentTags);

            string speakerName = GetSpeakerName(_story.currentTags);
            Sprite speakerPortrait = GetSpeakerPortrait(speakerName);

            ShowLine(line, speakerName, speakerPortrait);

            if (hasChoices)
                choiceSelector.Show(_story.currentChoices);
            else
                _waitingForInput = true;

            return;
        }

        // ── Choise without text──────────────────────
        if (_story.currentChoices.Count > 0) {
            ShowLine(string.Empty, string.Empty, null);
            choiceSelector.Show(_story.currentChoices);
            return;
        }

        ExitDialogueMode();
    }

    private void ShowLine(string line, string speakerName = "???", Sprite speakerPortrait = null) {
        dialogWindow.SetDialogueText(line);
        bool isEmptyName = string.IsNullOrEmpty(speakerName);

        if (isEmptyName) {
            dialogWindow.HideSpeakerName();
        } else {
            dialogWindow.ShowSpeakerName();
            dialogWindow.SetSpeakerName(speakerName);
        }

        

        // Portrait
        if (isEmptyName || speakerPortrait == null) {
            dialogWindow.HidePortrait();
        } else {
            dialogWindow.SetPortrait(speakerPortrait);
            dialogWindow.ShowPortrait();
        }
    }

    private string GetSpeakerName(List<string> tags) {
        foreach (string tag in tags) {
            if (!TryParseTag(tag, out string key, out string value))
                continue;

            if (key.Equals("speaker", StringComparison.OrdinalIgnoreCase))
                return value;
        }

        return string.Empty;
    }
    private Sprite GetSpeakerPortrait(string speakerName) {
        if (string.IsNullOrWhiteSpace(speakerName))
            return null;

        if (speakers.TryGetValue(speakerName, out Human human)) {
            if (human != null && human.personDataSO != null)
                return human.personDataSO.Portrait;
        }

        return null;
    }


    private void ProcessEventTags(List<string> tags) {
        foreach (string tag in tags) {
            if (!TryParseTag(tag, out string key, out string value))
                continue;

            switch (key.ToLower()) {
                case "anim":
                    DialogueEvents.FireAnimationTag(value);
                    break;

                case "sfx":
                    DialogueEvents.FireSfxTag(value);
                    break;
            }
        }
    }


    private bool TryParseTag(string tag, out string key, out string value) {
        key = string.Empty;
        value = string.Empty;

        if (string.IsNullOrWhiteSpace(tag))
            return false;

        int colon = tag.IndexOf(':');

        if (colon <= 0 || colon >= tag.Length - 1)
            return false;

        key = tag[..colon].Trim();
        value = tag[(colon + 1)..].Trim();

        return true;
    }

    private void OnChoiceSelected(Choice choice) {
        _story.ChooseChoiceIndex(choice.index);
        ContinueStory();
    }

    private void ExitDialogueMode() {
        IsDialoguePlaying = false;

        _waitingForInput = false;
        dialogWindow.HideDialoguePanel();
        choiceSelector.Hide();
        OnDialogueEnd?.Invoke();
    }
}


public static class DialogueEvents {
    public static event Action<string> OnDialogueBegin;
    public static event Action<string> OnDialogueEnd;

    public static event Action<string> OnAnimationTag;
    public static event Action<string> OnSfxTag;
    public static event Action<string> OnSpeakerChanged;
    
    public static void FireAnimationTag(string tag) => OnAnimationTag?.Invoke(tag);
    public static void FireSfxTag(string tag) => OnSfxTag?.Invoke(tag);
    public static void FireSpeakerChanged(string speaker) => OnSpeakerChanged?.Invoke(speaker);
}

using System;
using UnityEngine;
using Zenject;

public class DialogueNPC : Human {
    [SerializeField] private BodyView bodyView;
    [SerializeField] private Color lonelyColor, happyColor, questionColor;
    [SerializeField] private bool CanSpeakAgain = true;
    private bool isSpoken = false;
    public bool CanSpeak => CanSpeakAgain ? true : !isSpoken;
    [SerializeField] bool forcedFirstDialogue = false; // if true, will trigger dialogue immediately on start (for testing)

    private DialogueTrigger _dTrigger;
    [Inject] DialogueManager dialogueManager;

    protected override void Awake() {
        base.Awake();
        _dTrigger = GetComponentInChildren<DialogueTrigger>();
        bodyView?.ToggleVisualCue(false);
    }

    private void OnEnable() {
        if (_dTrigger) _dTrigger.OnDialoguePossible += HandlePossibleDialog;

        signalBus?.Subscribe<DialogStartedSignal>(HandleDialogueBegin);
    }

    private void OnDisable() {
        if (_dTrigger) _dTrigger.OnDialoguePossible -= HandlePossibleDialog;

        signalBus?.TryUnsubscribe<DialogStartedSignal>(HandleDialogueBegin);
        signalBus?.TryUnsubscribe<DialogEndSignal>(HandleDialogueEnd);
    }

    private void HandlePossibleDialog(bool isPossible, Player player) {
        if (forcedFirstDialogue && isPossible && !isSpoken) {
            dialogueManager.EnterDialogueMode(GetDialogKnot(), player, this);
            return;
        }
        if (CanSpeakAgain && !isSpoken) {
            bodyView.ToggleVisualCue(true);
        } else {
            bodyView.ToggleVisualCue(false);
        }
    }

    // Called by DialogueManager subscriber (set up in Player.HandleInteraction)
    public void HandleDialogueBegin(DialogStartedSignal signal) {
        bodyView?.ToggleVisualCue(false);
        if (signal.NPC != this) return;

        isSpoken = true;
        // Subscribe to events only for the duration of THIS dialogue
        DialogueEvents.OnAnimationTag += HandleAnimationTag;

        signalBus.Subscribe<DialogEndSignal>(HandleDialogueEnd);
    }

    private void HandleDialogueEnd() {
        isSpoken = true;
        DialogueEvents.OnAnimationTag -= HandleAnimationTag;

        signalBus.Unsubscribe<DialogEndSignal>(HandleDialogueEnd);
    }

    // Handles only tags that start with this NPC's name
    private void HandleAnimationTag(string tag) {
        int colon = tag.IndexOf(':');
        if (colon < 0) return;

        string owner = tag[..colon];
        string action = tag[(colon + 1)..];

        if (bodyView == null) return;

        Color target = action switch {
            "question" => questionColor,
            "color_fade" => lonelyColor,
            "color_green" => happyColor,
            _ => bodyView.CurrentColor
        };

        bodyView?.SetColor(target);
    }

    public string GetDialogKnot() {
        return personDataSO != null ? personDataSO.InkKnotName : string.Empty;
    }
}
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Zenject;

public class DialogueNPC : Human {
    [SerializeField] private DialogueNodeSO dialogueData;
    [SerializeField] private BodyView bodyView;
    [SerializeField] private Color lonelyColor, happyColor, questionColor;
    [SerializeField] private bool CanSpeakAgain = true;
    private bool isSpoken = false;
    public bool CanSpeak => CanSpeakAgain ? true : !isSpoken;

    private DialogueTrigger _dTrigger;
    [Inject] DialogueManager dialogueManager;

    protected void Awake() {
        _dTrigger = GetComponentInChildren<DialogueTrigger>();
        if (_dTrigger) _dTrigger.OnDialoguePossible += HandlePossibleDialog;
        bodyView?.ToggleVisualCue(false);
    }

    private void HandlePossibleDialog(bool isPossible) {
        if (CanSpeakAgain && !isSpoken) {
            bodyView.ToggleVisualCue(true);
        } else {
            bodyView.ToggleVisualCue(false);
        }
    }

    public DialogueNodeSO BeginDialogue() => dialogueData;

    // Called by DialogueManager subscriber (set up in Player.HandleInteraction)
    public void OnDialogueBegin() {
        bodyView?.ToggleVisualCue(false);
        isSpoken = true;
        // Subscribe to events only for the duration of THIS dialogue
        DialogueEvents.OnAnimationTag += HandleAnimationTag;
        dialogueManager.OnDialogueEnd += OnDialogueEnd;
    }

    private void OnDialogueEnd() {
        isSpoken = true; // stays false after talking — change if you want repeat dialogue
        DialogueEvents.OnAnimationTag -= HandleAnimationTag;
        dialogueManager.OnDialogueEnd -= OnDialogueEnd;
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

    
}
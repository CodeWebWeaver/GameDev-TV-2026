using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Human {

    private InputSystem_Actions.PlayerActions player;
    private InputAction _interactAction;
    [SerializeField] PlayerDialogSystem dialogSystem;
    [Header ("Personality Settings")]
    public PlayerPersonality Personality { get; } = new();
    [SerializeField] List<PersonalityParamSO> startingEmotions;
    [SerializeField] PersonalitiesUIManager personalitiesUIManager;

    private void Start() {
        personalitiesUIManager.Observe(Personality);
        Personality.Initialize(startingEmotions);

        player = InputManager.Instance.InputActions.Player;

        _interactAction = player.Interact;
        _interactAction.performed += OnInteract;
    }

    private void OnDestroy() {
        if (_interactAction != null)
        _interactAction.performed -= OnInteract;
    }

    private void OnInteract(InputAction.CallbackContext ctx) {
        HandleInteraction();
    }

    private void HandleInteraction() {
        if (DialogueManager.Instance.IsDialoguePlaying) return;

        DialogueNPC[] npcs = dialogSystem.FindDialogueNPC();
        if (npcs.Length == 0) return;

        DialogueNPC npc = npcs[0];
        npc.OnDialogueBegin(); // NPC підписується на свої події
        DialogueManager.Instance.EnterDialogueMode(npc.BeginDialogue(), this, npc);
    }
}


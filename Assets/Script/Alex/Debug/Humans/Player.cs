using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class Player : Human {
    private const string Personalities_Path = "PersonalityParams";
    private InputSystem_Actions.PlayerActions player;
    private InputAction _interactAction;
    [SerializeField] PlayerDialogSystem dialogSystem;
    [Header ("Personality Settings")]
    public PlayerPersonality Personality { get; } = new();
    [SerializeField] PersonalitiesViewManager personalitiesUIManager;
    [Inject] private InputManager InputManager;
    [Inject] private DialogueManager dialogueManager;

    private void Start() {
        personalitiesUIManager.Observe(Personality);
        List<PersonalityParamSO> personalityParamSOs = Resources.LoadAll<PersonalityParamSO>(Personalities_Path).ToList();
        Personality.Initialize(personalityParamSOs);

        player = InputManager.InputActions.Player;

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
        if (dialogueManager == null) return;
        if (dialogueManager.IsDialoguePlaying) return;

        DialogueNPC[] npcs = dialogSystem.FindDialogueNPC();
        if (npcs.Length == 0) return;

        DialogueNPC npc = npcs[0];
        npc.OnDialogueBegin(); // NPC підписується на свої події
        dialogueManager.EnterDialogueMode(npc.BeginDialogue(), this, npc);
    }
}


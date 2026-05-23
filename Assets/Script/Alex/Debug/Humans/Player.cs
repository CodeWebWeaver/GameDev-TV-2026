using DG.Tweening.Core.Easing;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class Player : Human {
    private const string Personalities_Path = "PersonalityParams";
    [SerializeField] PlayerDialogSystem dialogSystem;

    [Header ("Personality Settings")]
    public PlayerPersonality Personality { get; private set; }

    [Inject] private PlayerInputService playerInput;
    [Inject] private DialogueManager dialogueManager;
    [Inject] private UIManager uIManager;

    protected void Awake() {
        Personality = new PlayerPersonality(signalBus);
    }

    private void Start() {
        List<PersonalityParamSO> personalityParamSOs = Resources.LoadAll<PersonalityParamSO>(Personalities_Path).ToList();
        Personality.Initialize(personalityParamSOs);
    }

    private void OnEnable() {
        playerInput.OnInteractEnded += OnInteract;
    }

    private void OnDisable() {
        playerInput.OnInteractEnded -= OnInteract;
    }

    private void OnInteract() {
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


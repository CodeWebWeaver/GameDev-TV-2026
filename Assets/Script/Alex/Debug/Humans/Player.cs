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
    public PlayerPersonality Personality { get; } = new();
    [SerializeField] private PersonalitiesViewManager personalitiesUIManagerPrefab;
    private PersonalitiesViewManager personalitiesUIManager;

    [Inject] private PlayerInputService playerInput;
    [Inject] private DialogueManager dialogueManager;
    [Inject] private UIManager uIManager;

    private void Awake() {
        GameObject gameObject1 = uIManager.InstantiateUIElement(personalitiesUIManagerPrefab.gameObject);
        if (gameObject1 != null) {
            personalitiesUIManager = gameObject1.GetComponent<PersonalitiesViewManager>();
        }
    }
    private void Start() {
        personalitiesUIManager?.Observe(Personality);
        List<PersonalityParamSO> personalityParamSOs = Resources.LoadAll<PersonalityParamSO>(Personalities_Path).ToList();
        Personality.Initialize(personalityParamSOs);

        playerInput.OnInteractEnded += OnInteract;
    }

    private void OnDestroy() {
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


using DG.Tweening.Core.Easing;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class Player : Human {
    private const string Personalities_Path = "PersonalityParams";
    [SerializeField] PlayerDialogSystem dialogSystem;

    [Header("Personality Settings")]
    public PlayerPersonality Personality { get; private set; }

    [Inject] private PlayerInputService playerInput;
    [Inject] private DialogueManager dialogueManager;
    [Inject] private UIManager uIManager;

    [SerializeField] CinemachineCamera dialogCamera;
    [SerializeField] CinemachineCamera playerMainCamera;

    [Header("Dialog Camera Settings")]
    [Tooltip("Висота камери відносно позиції NPC (на рівні очей)")]
    [SerializeField] private float cameraHeightOffset = 1.6f;

    [Tooltip("Відстань камери від NPC по горизонталі")]
    [SerializeField] private float cameraDistanceFromNpc = 2.5f;

    [Tooltip("Зміщення камери вбік від осі гравець-NPC (0 = пряма вісь, >0 = збоку)")]
    [SerializeField] private float cameraSideOffset = 0.5f;

    [Tooltip("Висота точки, на яку дивиться камера (рівень голови NPC)")]
    [SerializeField] private float lookAtHeightOffset = 1.6f;

    [Tooltip("Мінімальна відстань камери від гравця")]
    [SerializeField] private float minDistanceFromPlayer = 0.5f;

    protected void Awake() {
        if (playerMainCamera == null)
            Debug.LogWarning("Player main camera is not assigned in the inspector.");
        if (dialogCamera == null)
            Debug.LogWarning("Dialog camera is not assigned in the inspector.");

        Personality = new PlayerPersonality(signalBus);
    }

    private void Start() {
        List<PersonalityParamSO> personalityParamSOs = Resources.LoadAll<PersonalityParamSO>(Personalities_Path).ToList();
        Personality.Initialize(personalityParamSOs);
    }

    private void OnEnable() {
        playerInput.OnInteractEnded += OnInteract;
        signalBus.Subscribe<DialogStartedSignal>(HandleDialogStart);
        signalBus.Subscribe<DialogEndSignal>(HandleDialogEnd);
    }

    private void OnDisable() {
        playerInput.OnInteractEnded -= OnInteract;
        signalBus.Unsubscribe<DialogStartedSignal>(HandleDialogStart);
        signalBus.Unsubscribe<DialogEndSignal>(HandleDialogEnd);
    }

    private void HandleDialogStart(DialogStartedSignal signal) {
        if (signal.NPC == null) return;
        PlaceDialogCameraToNpc(signal.NPC);
        SwitchToDialogCamera();
    }

    private void HandleDialogEnd(DialogEndSignal _) {
        SwitchToMainCamera();
    }

    private void PlaceDialogCameraToNpc(DialogueNPC npc) {
        if (dialogCamera == null || npc == null) return;

        Vector3 playerPos = transform.position;
        Vector3 npcPos = npc.transform.position;

        // Горизонтальний напрямок від NPC до гравця (зворотній — щоб камера була з боку NPC)
        Vector3 horizontalDir = (playerPos - npcPos);
        horizontalDir.y = 0f;
        horizontalDir.Normalize();

        // Перпендикуляр (вбік) для бічного зміщення
        Vector3 sideDir = Vector3.Cross(Vector3.up, horizontalDir).normalized;

        // Фінальна позиція камери: від NPC в бік гравця + бічне зміщення + висота
        Vector3 cameraPos = npcPos
            + horizontalDir * cameraDistanceFromNpc
            + sideDir * cameraSideOffset
            + Vector3.up * cameraHeightOffset;

        // Перевірка що камера не "провалюється" всередину гравця
        float distFromPlayer = Vector3.Distance(
            new Vector3(cameraPos.x, 0, cameraPos.z),
            new Vector3(playerPos.x, 0, playerPos.z)
        );
        if (distFromPlayer < minDistanceFromPlayer) {
            Vector3 pushDir = (cameraPos - playerPos);
            pushDir.y = 0f;
            pushDir.Normalize();
            cameraPos = playerPos + pushDir * minDistanceFromPlayer + Vector3.up * cameraHeightOffset;
        }

        dialogCamera.transform.position = cameraPos;

        // Камера дивиться на голову NPC
        Vector3 lookAtPoint = npcPos + Vector3.up * lookAtHeightOffset;
        dialogCamera.transform.LookAt(lookAtPoint);
    }

    private void SwitchToDialogCamera() {
        dialogCamera.Priority = 10;
        playerMainCamera.Priority = 0;
    }

    private void SwitchToMainCamera() {
        dialogCamera.Priority = 0;
        playerMainCamera.Priority = 10;
    }

    private void OnInteract() => HandleInteraction();

    private void HandleInteraction() {
        if (dialogueManager == null || dialogueManager.IsDialoguePlaying) return;

        DialogueNPC[] npcs = dialogSystem.FindDialogueNPC();
        if (npcs.Length == 0) return;

        DialogueNPC npc = npcs[0];
        npc.OnDialogueBegin();
        dialogueManager.EnterDialogueMode(npc.GetDialogKnot(), this, npc);
    }
}

public class DialogStartedSignal {
    public Player Player { get; }
    public DialogueNPC NPC { get; }
    public DialogStartedSignal(Player player, DialogueNPC npc) {
        Player = player;
        NPC = npc;
    }
}

public class DialogEndSignal {
    public Player Player { get; }
    public DialogueNPC NPC { get; }
    public DialogEndSignal(Player player, DialogueNPC npc) {
        Player = player;
        NPC = npc;
    }
}
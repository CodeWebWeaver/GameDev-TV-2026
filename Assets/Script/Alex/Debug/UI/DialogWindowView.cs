using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


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

    // ── Dialogue ─────────────────────────────────────────

    public void SetDialogueText(string text) {
        dialogueText.text = text;
    }

    public void ClearDialogueText() {
        dialogueText.text = string.Empty;
    }

    // ── Speaker Name ────────────────────────────────────

    public void SetSpeakerName(string speakerName) {
        speakerNameText.text = speakerName;
    }

    public void ShowSpeakerName() {
        nameObject.SetActive(true);
    }

    public void HideSpeakerName() {
        nameObject.SetActive(false);
    }

    // ── Portrait ────────────────────────────────────────

    public void SetPortrait(Sprite portrait) {
        speakerPortraitImage.sprite = portrait;
    }

    public void ShowPortrait() {
        portraitObject.SetActive(true);
    }

    public void HidePortrait() {
        portraitObject.SetActive(false);
    }

    // ── Panel ───────────────────────────────────────────

    public void ShowDialoguePanel() {
        dialoguePanel.SetActive(true);
    }

    public void HideDialoguePanel() {
        dialoguePanel.SetActive(false);
    }
}

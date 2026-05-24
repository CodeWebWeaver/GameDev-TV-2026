using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PausePanel : MonoBehaviour
{
    [SerializeField] Button menuButton;
    [InjectOptional] GameController gameController;
    [SerializeField] private UIPanel pausePanel;

    private void OnEnable() {
        if (pausePanel != null) {
            pausePanel.OnCloseButtonClicked += HandlePausePanelClose;
        }
        menuButton.onClick.AddListener(HandleGoToMenu);
    }

    private void HandlePausePanelClose() {
        gameController.HandlePauseRequest();
    }

    private void HandleGoToMenu() {
        gameController.HandleMenuRequest();
    }

    private void OnDisable() {
        if (pausePanel != null) {
            pausePanel.OnCloseButtonClicked -= HandlePausePanelClose;
        }
        menuButton.onClick.RemoveListener(HandleGoToMenu);
    }
}

using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PausePanel : MonoBehaviour
{
    [SerializeField] Button menuButton;
    [InjectOptional] GameController gameController;

    private void Awake() {
        menuButton.onClick.AddListener(HandleGoToMenu);
    }

    private void HandleGoToMenu() {
        gameController.HandleMenuRequest();
    }

    private void OnDestroy() {
        menuButton.onClick.RemoveListener(HandleGoToMenu);
    }
}

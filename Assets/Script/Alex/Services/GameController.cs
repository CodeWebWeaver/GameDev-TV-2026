using System;
using UnityEngine;
using Zenject;

public class GameController : MonoBehaviour
{
    [InjectOptional] private UIManager uiManager;
    private InputSystem_Actions.UIActions uIMap;
    [InjectOptional] GameManager gameManager;

    private void OnEnable() {
        if (InputManager.Instance == null) return;

        uIMap = InputManager.Instance.InputActions.UI;
        uIMap.Cancel.performed += TogglePause;
    }

    private void TogglePause(UnityEngine.InputSystem.InputAction.CallbackContext context) {
        uiManager?.HandleCancel();
    }

    public void HandleMenuRequest() {
        gameManager.EnterMenu();
    }

    
    private void OnDisable() {
        uIMap.Cancel.performed -= TogglePause;
    }
}

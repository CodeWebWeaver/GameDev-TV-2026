using System;
using UnityEngine;
using Zenject;

public class GameController : MonoBehaviour
{
    [InjectOptional] private InputManager inputManager;

    private InputSystem_Actions.UIActions uIMap;
    private InputSystem_Actions.PlayerActions playerMap;
    [InjectOptional] GameManager gameManager;

    private void OnEnable() {
        if (inputManager == null) return;

        playerMap = inputManager.InputActions.Player;
        uIMap = inputManager.InputActions.UI;

        playerMap.Cancel.performed += TogglePause;
        uIMap.Cancel.performed += TogglePause;
    }

    private void TogglePause(UnityEngine.InputSystem.InputAction.CallbackContext context) {
        gameManager.HandleCancel();
    }

    public void HandleMenuRequest() {
        gameManager.EnterMenu();
    }

    
    private void OnDisable() {
        playerMap.Cancel.performed -= TogglePause;
        uIMap.Cancel.performed -= TogglePause;
    }
}

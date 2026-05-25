using System;
using UnityEngine;

public class InputManager : IDisposable {
    private InputSystem_Actions _inputActions;
public InputManager() {
        _inputActions = new InputSystem_Actions();
        _inputActions.Enable();
    }

    public InputSystem_Actions InputActions => _inputActions;


    public void SwitchToPlayerMap() {
        Debug.Log("Switching to Player input map...");
        _inputActions.UI.Disable();
        _inputActions.Player.Enable();
        _inputActions.Dialog.Disable();
    }

    public void SwitchToUIMap() {
        Debug.Log("Switching to UI input map...");
        _inputActions.Player.Disable();
        _inputActions.UI.Enable();
        _inputActions.Dialog.Disable();
    }

    public void SwitchToDialogMap() {
        Debug.Log("Switching to Dialog input map...");
        _inputActions.Player.Disable();
        _inputActions.UI.Disable();
        _inputActions.Dialog.Enable();
    }

    private void OnDestroy() => Dispose();
    public void Dispose() => _inputActions?.Dispose();

    
}
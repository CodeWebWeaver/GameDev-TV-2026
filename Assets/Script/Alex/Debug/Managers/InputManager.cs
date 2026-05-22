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
        _inputActions.UI.Disable();
        _inputActions.Player.Enable();
        _inputActions.Dialog.Disable();
    }

    public void SwitchToUIMap() {
        _inputActions.Player.Disable();
        _inputActions.UI.Enable();
        _inputActions.Dialog.Disable();
    }

    public void SwitchToDialogMap() {
        _inputActions.Player.Disable();
        _inputActions.UI.Disable();
        _inputActions.Dialog.Enable();
    }

    private void OnDestroy() => Dispose();
    public void Dispose() => _inputActions?.Dispose();

    
}
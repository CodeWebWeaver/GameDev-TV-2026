using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputService : IDisposable {
    private readonly InputSystem_Actions.PlayerActions _playerActions;
    private bool _isSubscribed;

    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool IsJumping { get; private set; }
    public bool IsInteractHold { get; private set; }

    public event Action OnInteractStarted;
    public event Action OnInteractPerformed;
    public event Action OnInteractEnded;

    public PlayerInputService(InputManager inputManager) {
        _playerActions = inputManager.InputActions.Player;
        Subscribe();
    }

    public void Enable() {
        if (!_isSubscribed) {
            Subscribe();
        }
    }

    public void Disable() {
        if (_isSubscribed) {
            Unsubscribe();
        }
    }

    private void Subscribe() {
        _playerActions.Move.performed += HandleMove;
        _playerActions.Move.canceled += HandleMove;
        _playerActions.Look.performed += HandleLook;
        _playerActions.Look.canceled += HandleLook;
        _playerActions.Jump.performed += HandleJump;
        _playerActions.Jump.canceled += HandleJump;
        _playerActions.Interact.started += HandleInteractStarted;
        _playerActions.Interact.performed += HandleInteractPerformed;
        _playerActions.Interact.canceled += HandleInteractCanceled;

        _isSubscribed = true;
    }

    private void Unsubscribe() {
        _playerActions.Move.performed -= HandleMove;
        _playerActions.Move.canceled -= HandleMove;
        _playerActions.Look.performed -= HandleLook;
        _playerActions.Look.canceled -= HandleLook;
        _playerActions.Jump.performed -= HandleJump;
        _playerActions.Jump.canceled -= HandleJump;
        _playerActions.Interact.started -= HandleInteractStarted;
        _playerActions.Interact.performed -= HandleInteractPerformed;
        _playerActions.Interact.canceled -= HandleInteractCanceled;

        _isSubscribed = false;
    }

    private void HandleMove(InputAction.CallbackContext ctx) {
        MoveInput = ctx.ReadValue<Vector2>();
    }

    private void HandleLook(InputAction.CallbackContext ctx) {
        LookInput = ctx.ReadValue<Vector2>();
    }

    private void HandleJump(InputAction.CallbackContext ctx) {
        IsJumping = ctx.performed;
    }

    private void HandleInteractStarted(InputAction.CallbackContext ctx) => OnInteractStarted?.Invoke();
    private void HandleInteractPerformed(InputAction.CallbackContext ctx) {
        IsInteractHold = true;
        OnInteractPerformed?.Invoke();
    }
    private void HandleInteractCanceled(InputAction.CallbackContext ctx) {
        IsInteractHold = false;
        OnInteractEnded?.Invoke();
    }

    public void Dispose() {
        Unsubscribe();
    }
}

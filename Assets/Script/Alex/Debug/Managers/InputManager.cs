public class InputManager {
    public InputSystem_Actions InputActions => inputActions;

    private InputSystem_Actions inputActions = new InputSystem_Actions();

    public InputManager() {
        inputActions.Enable();
    }
}

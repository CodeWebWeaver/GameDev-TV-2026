using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private InputManagement inputManagement;

    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        inputManagement = InputManagement.Instance;
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        Vector2 movement = inputManagement.GetPlayerMovement();
        Vector3 localMoveInput = new Vector3(movement.x, 0f, movement.y);

        controller.Move(localMoveInput * Time.deltaTime * playerSpeed);

        if (localMoveInput != Vector3.zero)
        {
            gameObject.transform.forward = localMoveInput;
        }

        if (inputManagement.PlayerJumpedThisFrame() && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}

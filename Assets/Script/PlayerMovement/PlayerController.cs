using KinematicCharacterController;
using UnityEngine;
using Zenject;

public class KinematicPlayerMovement : MonoBehaviour, ICharacterController {
    [Inject] PlayerInputService playerInput;
    [SerializeField] KinematicCharacterMotor motor;

    [Header("Camera Settings")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform head;

    [SerializeField] private float mouseSensitivity = 2.0f;
    [SerializeField] private float cameraSmoothing = 10f;

    [Header("Movement Settings")]
    [SerializeField] private float stableMovementSharpness = 15f;
    [SerializeField] private float maxStableMoveSpeed = 10f;
    [SerializeField] private float airAccelerationSpeed = 15f;
    [SerializeField] private float drag = 0.1f;

    [Header("Jump Settings")]
    [SerializeField] private bool allowJumpingWhenSliding = false;
    [SerializeField] private float jumpUpSpeed = 10f;
    [SerializeField] private float jumpScalableForwardSpeed = 10f;
    [SerializeField] private float jumpBufferTime = 0.15f; // Замість grace time
    [SerializeField] private float coyoteTime = 0.2f; // Час для стрибка після зіткнення з землею
    [SerializeField] private bool preserveVelocityOnJump = false;

    [Header("Rotation Settings")]
    [SerializeField] private float rotationSharpness = 25f;

    [Range(0, 90)]
    [SerializeField] private float cameraClampUp = 85f;

    [Range(0, 90)]
    [SerializeField] private float cameraClampDown = 85f;

    // Камера
    private float cameraPitch = 0f;
    private float targetCameraPitch = 0f;
    private float characterYaw = 0f;

    // Рух
    [SerializeField] private Vector3 cachedCurrentVelocity;
    private Vector3 cachedWorldMoveInput;

    // Стрибки - спрощені змінні
    private float jumpBufferTimer = 0f;
    private float coyoteTimer = 0f;
    private bool wasGroundedLastFrame = false;

    private void Awake() {
        motor.CharacterController = this;
        cameraPitch = 0f;
        targetCameraPitch = 0f;
        characterYaw = transform.eulerAngles.y;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update() {
        // Оновлюємо таймери
        UpdateTimers();

        // Обробка вводу стрибка
        if (playerInput.IsJumping) {
            jumpBufferTimer = jumpBufferTime;
        }
    }

    private void UpdateTimers() {
        if (jumpBufferTimer > 0) {
            jumpBufferTimer -= Time.deltaTime;
        }

        if (motor.GroundingStatus.IsStableOnGround) {
            coyoteTimer = coyoteTime;
            wasGroundedLastFrame = true;
        } else {
            if (wasGroundedLastFrame && coyoteTimer > 0) {
                coyoteTimer -= Time.deltaTime;
            }
            wasGroundedLastFrame = false;
        }
    }

    public void UpdateRotation(ref Quaternion currentRotation, float deltaTime) {
        Vector2 lookInput = playerInput.LookInput;

        characterYaw += lookInput.x * mouseSensitivity;
        targetCameraPitch -= lookInput.y * mouseSensitivity;
        targetCameraPitch = Mathf.Clamp(targetCameraPitch, -cameraClampDown, cameraClampUp);

        cameraPitch = Mathf.Lerp(cameraPitch, targetCameraPitch, 1f - Mathf.Exp(-cameraSmoothing * deltaTime));


        // Apply Y to character body
        Quaternion targetRotation = Quaternion.Euler(0f, characterYaw, 0f);
        currentRotation = Quaternion.Slerp(
            currentRotation,
            targetRotation,
            1f - Mathf.Exp(-rotationSharpness * deltaTime)
        );


        // Apply X to camera and head
        Vector3 resultEulers = new Vector3(cameraPitch, 0f, 0f);

        if (cameraTransform != null) {
            cameraTransform.localEulerAngles = resultEulers;
        }

        if (head) {
            head.localEulerAngles = resultEulers;
        }
    }

    public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime) {
        // Обчислюємо вхідні дані руху
        CalculateMovementVectors();

        // Розділена логіка залежно від стану
        if (motor.GroundingStatus.IsStableOnGround) {
            HandleGroundMovement(ref currentVelocity, deltaTime);
        } else {
            HandleAirMovement(ref currentVelocity, deltaTime);
        }

        // Обробка стрибка
        HandleJump(ref currentVelocity, deltaTime);

        cachedCurrentVelocity = currentVelocity;
    }

    private void CalculateMovementVectors() {
        Vector2 moveInput = playerInput.MoveInput;

        Vector3 forward = motor.CharacterForward;
        Vector3 right = motor.CharacterRight;

        // Проектуємо на площину землі якщо стоїмо
        if (motor.GroundingStatus.IsStableOnGround) {
            Vector3 groundNormal = motor.GroundingStatus.GroundNormal;
            forward = Vector3.ProjectOnPlane(forward, groundNormal).normalized;
            right = Vector3.ProjectOnPlane(right, groundNormal).normalized;
        }

        cachedWorldMoveInput = forward * moveInput.y + right * moveInput.x;

        if (cachedWorldMoveInput.magnitude > 1f) {
            cachedWorldMoveInput.Normalize();
        }
    }

    private void HandleGroundMovement(ref Vector3 currentVelocity, float deltaTime) {
        Vector3 effectiveGroundNormal = motor.GroundingStatus.GroundNormal;
        Vector3 targetMovementVelocity = cachedWorldMoveInput * maxStableMoveSpeed;

        // Проектуємо поточну швидкість на поверхню
        currentVelocity = motor.GetDirectionTangentToSurface(currentVelocity, effectiveGroundNormal) * currentVelocity.magnitude;

        // Обмежуємо швидкість при русі вгору по схилу
        if (Vector3.Dot(targetMovementVelocity, effectiveGroundNormal) > 0f) {
            targetMovementVelocity = Vector3.ProjectOnPlane(targetMovementVelocity, effectiveGroundNormal);
        }

        // Плавна інтерполяція до цільової швидкості
        currentVelocity = Vector3.Lerp(
            currentVelocity,
            targetMovementVelocity,
            1f - Mathf.Exp(-stableMovementSharpness * deltaTime)
        );
    }

    private void HandleAirMovement(ref Vector3 currentVelocity, float deltaTime) {
        // Додаємо прискорення в повітрі тільки якщо є input
        if (cachedWorldMoveInput.sqrMagnitude > 0f) {
            Vector3 addedVelocity = cachedWorldMoveInput * airAccelerationSpeed * deltaTime;
            Vector3 currentVelocityOnInputsPlane = Vector3.ProjectOnPlane(currentVelocity, motor.CharacterUp);

            // Обмежуємо швидкість максимальною
            if (currentVelocityOnInputsPlane.magnitude < maxStableMoveSpeed) {
                Vector3 newVelocity = currentVelocityOnInputsPlane + addedVelocity;
                addedVelocity = Vector3.ClampMagnitude(newVelocity, maxStableMoveSpeed) - currentVelocityOnInputsPlane;
                currentVelocity += addedVelocity;
            }
        }

        // Застосовуємо гравітацію та опір повітря
        currentVelocity += Physics.gravity * deltaTime;
        currentVelocity *= (1f / (1f + (drag * deltaTime)));
    }

    private void HandleJump(ref Vector3 currentVelocity, float deltaTime) {
        // Перевірка можливості стрибка
        if (jumpBufferTimer <= 0) return;

        bool canJumpFromSlope = !motor.GroundingStatus.SnappingPrevented || allowJumpingWhenSliding;
        bool isGrounded = motor.GroundingStatus.IsStableOnGround && canJumpFromSlope;
        bool hasCoyoteTime = coyoteTimer > 0 && canJumpFromSlope;

        if (!isGrounded && !hasCoyoteTime) return;

        // Виконуємо стрибок
        ExecuteJump(ref currentVelocity);

        // Скидаємо таймери
        jumpBufferTimer = 0f;
        coyoteTimer = 0f;
    }

    private void ExecuteJump(ref Vector3 currentVelocity) {
        // Визначаємо напрямок стрибка
        Vector3 jumpDirection = motor.CharacterUp;
        if (motor.GroundingStatus.FoundAnyGround && !motor.GroundingStatus.IsStableOnGround) {
            jumpDirection = motor.GroundingStatus.GroundNormal;
        }

        // Обробка збереження швидкості
        if (!preserveVelocityOnJump && cachedWorldMoveInput.sqrMagnitude < 0.01f) {
            Vector3 verticalVelocity = Vector3.Project(currentVelocity, motor.CharacterUp);
            currentVelocity = verticalVelocity;
        }

        // Додаємо вертикальну швидкість стрибка
        currentVelocity += (jumpDirection * jumpUpSpeed) - Vector3.Project(currentVelocity, motor.CharacterUp);

        // Додаємо горизонтальну швидкість тільки якщо є input
        if (cachedWorldMoveInput.sqrMagnitude > 0.01f) {
            currentVelocity += cachedWorldMoveInput * jumpScalableForwardSpeed;
        }
    }

    public void PostGroundingUpdate(float deltaTime) {
        // Якщо стрибнули, від'єднуємо від землі
        if (jumpBufferTimer > 0 && (motor.GroundingStatus.IsStableOnGround || coyoteTimer > 0)) {
            motor.ForceUnground();
        }
    }

    public void BeforeCharacterUpdate(float deltaTime) { }
    public void AfterCharacterUpdate(float deltaTime) { }
    public bool IsColliderValidForCollisions(Collider coll) { return true; }
    public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport) { }
    public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport) { }
    public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport) { }
    public void OnDiscreteCollisionDetected(Collider hitCollider) { }

    void OnDrawGizmos() {
        if (!Application.isPlaying || motor == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, cachedCurrentVelocity);

        if (cameraTransform != null) {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(cameraTransform.position, cameraTransform.forward * 2f);
        }

        Gizmos.color = motor.GroundingStatus.IsStableOnGround ? Color.green : Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.2f);
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    public float moveSpeed = 5f;
    public float lookSensitivity = 1f;
    public Transform cameraTransform;
    public float raycastMaxDistance = 5f;

    private PlayerControls controls;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private CharacterController characterController;
    private Vector3 velocity;
    private float pitch = 0f;

    // Reference to your menu manager
    private MenuManager menuManager;

    private void Awake() {
        controls = new PlayerControls();
        characterController = GetComponent<CharacterController>();
        menuManager = FindFirstObjectByType<MenuManager>();
    }

    private void OnEnable() {
        controls.Player.Enable();
        controls.Camera.Enable();
        controls.Interaction.Enable();

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        controls.Camera.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        controls.Camera.Look.canceled += ctx => lookInput = Vector2.zero;
        controls.Interaction.Interact.performed += _ => Interact();
    }

    private void OnDisable() {
        controls.Player.Disable();
        controls.Camera.Disable();
        controls.Interaction.Disable();
    }

    private void Update() {
        // Check both InputBox and Menu states
        if (!IsInputBlocked()) {
            Move();
            LookAround();
        }
    }

    private bool IsInputBlocked() {
        return InputBoxManager.Instance.IsInputActive ||
               (menuManager != null && menuManager.IsMenuActive);
    }

    private void Move() {
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;

        if (characterController.isGrounded && velocity.y < 0) {
            velocity.y = -2f;
        }
        velocity.y += Physics.gravity.y * Time.deltaTime;

        characterController.Move((move * moveSpeed + velocity) * Time.deltaTime);
    }

    private void LookAround() {
        transform.Rotate(Vector3.up, lookInput.x * lookSensitivity);
        pitch -= lookInput.y * lookSensitivity;
        pitch = Mathf.Clamp(pitch, -90f, 90f);
        cameraTransform.localEulerAngles = new Vector3(pitch, 0f, 0f);
    }

    private void Interact() {
        // Only allow interaction when input isn't blocked
        if (!IsInputBlocked()) {
            Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, raycastMaxDistance)) {
                Debug.Log($"Interacted with: {hit.collider.name}");
            }
        }
    }

    // Optional: Method to manually control input state
    public void SetInputEnabled(bool enabled) {
        if (enabled) {
            controls.Player.Enable();
            controls.Camera.Enable();
            controls.Interaction.Enable();
        } else {
            controls.Player.Disable();
            controls.Camera.Disable();
            controls.Interaction.Disable();

            // Reset inputs
            moveInput = Vector2.zero;
            lookInput = Vector2.zero;
        }
    }
}
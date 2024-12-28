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
    private Vector3 velocity; // For gravity
    private float pitch = 0f; // Tracks vertical camera rotation


    private void Awake() {
        controls = new PlayerControls();
        characterController = GetComponent<CharacterController>();

    }

    private void OnEnable() {
        // Enable input action maps
        controls.Player.Enable();
        controls.Camera.Enable();
        controls.Interaction.Enable();

        // Subscribe to input actions
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        controls.Camera.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        controls.Camera.Look.canceled += ctx => lookInput = Vector2.zero;

        controls.Interaction.Interact.performed += _ => Interact();
    }

    private void OnDisable() {
        // Disable input action maps
        controls.Player.Disable();
        controls.Camera.Disable();
        controls.Interaction.Disable();
    }

    private void Update() {

        if (!InputBoxManager.Instance.IsInputActive) {
            Move();
            LookAround();
        }
    }

    private void Move() {
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;

        // Apply gravity
        if (characterController.isGrounded && velocity.y < 0) {
            velocity.y = -2f; // Reset velocity when grounded
        }
        velocity.y += Physics.gravity.y * Time.deltaTime;


        // Apply movement
        characterController.Move((move * moveSpeed + velocity) * Time.deltaTime);
    }

    private void LookAround() {
        // Horizontal rotation (player)
        transform.Rotate(Vector3.up, lookInput.x * lookSensitivity);

        // Vertical rotation (camera)
        pitch -= lookInput.y * lookSensitivity; // Decrease pitch for upward movement
        pitch = Mathf.Clamp(pitch, -90f, 90f); // Limit pitch to -90 to 90 degrees

        cameraTransform.localEulerAngles = new Vector3(pitch, 0f, 0f); // Apply pitch
    }

    private void Interact() {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, raycastMaxDistance)) // Adjust ray distance as needed
        {
            Debug.Log($"Interacted with: {hit.collider.name}");
            // Add custom interaction logic here
        }
    }
}

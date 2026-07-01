using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
    public float crouchSpeed = 2.5f;
    public float jumpHeight = 1.5f;
    public float gravity = -20f;

[Header("Mouse Look")]
    public float mouseSensitivity = 2f;
    public Transform cameraHolder;

    [Header("Crouch")]
    public float standingHeight = 2f;
    public float crouchingHeight = 1f;
    public float crouchSmoothSpeed = 8f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.3f;
    public LayerMask groundMask;

    private CharacterController controller;

    private Vector3 velocity;
    private bool isGrounded;

    private float xRotation = 0f;

    private bool isCrouching;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
        HandleJump();
        HandleCrouch();
        ApplyGravity();
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleMovement()
    {
        isGrounded = Physics.CheckSphere(
            groundCheck.position,
            groundDistance,
            groundMask
        );

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        float currentSpeed = walkSpeed;

        if (Input.GetKey(KeyCode.LeftShift) && !isCrouching)
        {
            currentSpeed = sprintSpeed;
        }

        if (isCrouching)
        {
            currentSpeed = crouchSpeed;
        }

        Vector3 move =
            transform.right * x +
            transform.forward * z;

        controller.Move(move * currentSpeed * Time.deltaTime);
    }

    void HandleJump()
    {
        if (
            Input.GetButtonDown("Jump") &&
            isGrounded &&
            !isCrouching
        )
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void HandleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouching = !isCrouching;
        }

        float targetHeight =
            isCrouching ? crouchingHeight : standingHeight;

        controller.height = Mathf.Lerp(
            controller.height,
            targetHeight,
            crouchSmoothSpeed * Time.deltaTime
        );
    }

    void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

}

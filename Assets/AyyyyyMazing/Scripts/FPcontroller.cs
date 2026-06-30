using UnityEngine;

public class SimpleFirstPersonController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;

    public float sprintSpeed = 8f;

    public float jumpHeight = 2f;

    public float gravity = -20f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 2f;

    public Transform playerCamera;

    private CharacterController controller;

    private Vector3 velocity;

    private float cameraRotationX = 0f;

    void Start()
    {
        controller =
            GetComponent<CharacterController>();

        Cursor.lockState =
            CursorLockMode.Locked;

        Cursor.visible = false;
    }

    void Update()
    {
        Look();

        Move();
    }

    void Look()
    {
        float mouseX =
            Input.GetAxis("Mouse X") *
            mouseSensitivity;

        float mouseY =
            Input.GetAxis("Mouse Y") *
            mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        cameraRotationX -= mouseY;

        cameraRotationX =
            Mathf.Clamp(cameraRotationX,
                        -90f,
                        90f);

        playerCamera.localRotation =
            Quaternion.Euler(
                cameraRotationX,
                0f,
                0f
            );
    }

    void Move()
    {
        float moveX =
            Input.GetAxis("Horizontal");

        float moveZ =
            Input.GetAxis("Vertical");

        Vector3 move =
            transform.right * moveX +
            transform.forward * moveZ;

        float currentSpeed =
            Input.GetKey(KeyCode.LeftShift)
            ? sprintSpeed
            : walkSpeed;

        controller.Move(
            move * currentSpeed * Time.deltaTime
        );

        // Ground check
        if (controller.isGrounded &&
            velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Jump
        if (Input.GetButtonDown("Jump") &&
            controller.isGrounded)
        {
            velocity.y =
                Mathf.Sqrt(
                    jumpHeight * -2f * gravity
                );
        }

        // Gravity
        velocity.y +=
            gravity * Time.deltaTime;

        controller.Move(
            velocity * Time.deltaTime
        );
    }
}
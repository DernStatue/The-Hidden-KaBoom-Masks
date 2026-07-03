using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 6f;

    public float sprintSpeed = 10f;

    public float jumpHeight = 1.5f;

    public float gravity = -20f;

    [Header("Mouse Look")]
    public Transform cameraHolder;

    public float mouseSensitivity = 2f;

    public float maxLookAngle = 85f;

    [Header("Head Bob")]
    public bool enableHeadBob = true;

    public float bobSpeed = 10f;

    public float bobAmount = 0.05f;

    CharacterController controller;

    Vector3 velocity;

    bool isGrounded;

    float verticalLookRotation;

    Vector3 cameraOriginalPos;

    float bobTimer;

    void Start()
    {
        controller =
            GetComponent<
                CharacterController>();

        Cursor.lockState =
            CursorLockMode.Locked;

        Cursor.visible = false;

        if (cameraHolder != null)
        {
            cameraOriginalPos =
                cameraHolder.localPosition;
        }
    }

    void Update()
    {
        if (PauseMenu.IsPaused)
        {
            return;
        }

        HandleMouseLook();

        HandleMovement();

        HandleHeadBob();
    }

    void HandleMouseLook()
    {
        float mouseX =
            Input.GetAxis(
                "Mouse X"
            ) *
            mouseSensitivity;

        float mouseY =
            Input.GetAxis(
                "Mouse Y"
            ) *
            mouseSensitivity;

        verticalLookRotation -=
            mouseY;

        verticalLookRotation =
            Mathf.Clamp(
                verticalLookRotation,
                -maxLookAngle,
                maxLookAngle
            );

        if (cameraHolder != null)
        {
            cameraHolder.localRotation =
                Quaternion.Euler(
                    verticalLookRotation,
                    0f,
                    0f
                );
        }

        transform.Rotate(
            Vector3.up *
            mouseX
        );
    }

    void HandleMovement()
    {
        isGrounded =
            controller.isGrounded;

        if (
            isGrounded &&
            velocity.y < 0
        )
        {
            velocity.y = -2f;
        }

        float moveX =
            Input.GetAxis(
                "Horizontal"
            );

        float moveZ =
            Input.GetAxis(
                "Vertical"
            );

        Vector3 move =
            transform.right *
            moveX +
            transform.forward *
            moveZ;

        float speed =
            Input.GetKey(
                KeyCode.LeftShift
            )
            ? sprintSpeed
            : walkSpeed;

        controller.Move(
            move *
            speed *
            Time.deltaTime
        );

        if (
            Input.GetButtonDown(
                "Jump"
            ) &&
            isGrounded
        )
        {
            velocity.y =
                Mathf.Sqrt(
                    jumpHeight *
                    -2f *
                    gravity
                );
        }

        velocity.y +=
            gravity *
            Time.deltaTime;

        controller.Move(
            velocity *
            Time.deltaTime
        );
    }

    void HandleHeadBob()
    {
        if (
            !enableHeadBob ||
            cameraHolder == null
        )
        {
            return;
        }

        Vector3 horizontalVelocity =
            controller.velocity;

        horizontalVelocity.y = 0f;

        if (
            horizontalVelocity.magnitude > 0.1f &&
            isGrounded
        )
        {
            bobTimer +=
                Time.deltaTime *
                bobSpeed;

            float bobOffset =
                Mathf.Sin(
                    bobTimer
                ) *
                bobAmount;

            cameraHolder.localPosition =
                Vector3.Lerp(
                    cameraHolder.localPosition,
                    cameraOriginalPos +
                    Vector3.up *
                    bobOffset,
                    Time.deltaTime *
                    10f
                );
        }
        else
        {
            bobTimer = 0f;

            cameraHolder.localPosition =
                Vector3.Lerp(
                    cameraHolder.localPosition,
                    cameraOriginalPos,
                    Time.deltaTime *
                    10f
                );
        }
    }
}
using UnityEngine;

public class FPSController : MonoBehaviour
{
    public CharacterController controller;
    public Transform cameraHolder;

    [Header("Movement")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
    public float crouchSpeed = 2.5f;
    public float jumpHeight = 2f;
    public float gravity = -20f;

    [Header("Mouse")]
    [Tooltip("Nên để từ 1 đến 5 đối với code mới này")]
    public float mouseSensitivity = 2f; 

    [Header("Crouch")]
    public float standingHeight = 2f;
    public float crouchHeight = 1f;

    private Vector3 velocity;
    private float xRotation = 0f;
    private bool isCrouching;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Move();
        Look();
        Jump();
        Crouch();
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        float speed = walkSpeed;

        if (Input.GetKey(KeyCode.LeftShift) && !isCrouching)
        {
            speed = sprintSpeed;
        }

        if (isCrouching)
        {
            speed = crouchSpeed;
        }

        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    // ĐÂY LÀ HÀM ĐÃ ĐƯỢC FIX LỖI XOAY VÒNG VÒNG
    void Look()
    {
        // KHÔNG nhân với Time.deltaTime ở đây vì Input.GetAxis của chuột đã tự tối ưu rồi
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Xoay nhân vật theo trục Y (quay trái/phải)
        transform.Rotate(Vector3.up * mouseX);
    }

    void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouching = !isCrouching;

            if (isCrouching)
            {
                controller.height = crouchHeight;
            }
            else
            {
                controller.height = standingHeight;
            }
        }
    }
}
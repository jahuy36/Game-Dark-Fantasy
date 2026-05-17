using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Cài đặt Di chuyển")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;

    [Header("Cài đặt Chuột (Gắn chặt)")]
    public float mouseSensitivity = 200f;
    public Transform cameraPivot; // Kéo CameraPivot vào đây
    public Transform cameraTransform; // Kéo PlayerCamera vào đây

    [Header("Tham chiếu khác")]
    public Animator anim;
    public string idleAnim = "Idle";
    public string walkAnim = "Walk";

    private CharacterController controller;
    private PlayerInput playerInput;
    private float xRotation = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        // Khóa chuột
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Look();
        MoveAndRotate();
    }

    private void Look()
    {
        if (cameraPivot == null) return;

        // 1. Xoay CameraPivot lên/xuống (Trục X) dựa theo chuột Y
        float mY = playerInput.MouseY * mouseSensitivity * Time.deltaTime;
        xRotation -= mY;
        xRotation = Mathf.Clamp(xRotation, -40f, 60f); // Giới hạn không cho ngửa cổ quá đà
        cameraPivot.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // 2. Xoay toàn bộ nhân vật qua trái/phải (Trục Y) dựa theo chuột X
        float mX = playerInput.MouseX * mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mX);
    }

    private void MoveAndRotate()
    {
        if (cameraTransform == null) return;

        // Lấy hướng tiến/lùi, trái/phải dựa theo góc nhìn của Camera
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        // CHÌA KHÓA FIX LỖI BAY LÊN TRỜI: Triệt tiêu hoàn toàn trục Y (độ cao)
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        // Tính toán hướng đi thực tế trên mặt đất
        Vector3 moveDirection = (camForward * playerInput.Vertical + camRight * playerInput.Horizontal).normalized;

        // Di chuyển nhân vật
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        // Xử lý Animation
        if (playerInput.IsMoving)
        {
            if (anim != null) anim.Play(walkAnim);
        }
        else
        {
            if (anim != null) anim.Play(idleAnim);
        }
    }
}
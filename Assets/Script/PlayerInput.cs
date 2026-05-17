using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    // Các thuộc tính để các script khác truy cập (Read-only từ bên ngoài)
    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }
    public float MouseX { get; private set; }
    public float MouseY { get; private set; }
    public bool IsMoving => Horizontal != 0 || Vertical != 0;

    void Update()
    {
        // Nhận phím WASD / Mũi tên
        Horizontal = Input.GetAxisRaw("Horizontal");
        Vertical = Input.GetAxisRaw("Vertical");

        // Nhận cử động chuột
        MouseX = Input.GetAxis("Mouse X");
        MouseY = Input.GetAxis("Mouse Y");
    }
}
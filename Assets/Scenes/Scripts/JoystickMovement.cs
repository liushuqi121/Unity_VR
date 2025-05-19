using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SimplePlayerMovement : MonoBehaviour
{
    [Header("移动设置")]
    public float moveSpeed = 5f;
    public float gravity = -9.81f;
    
    private CharacterController _controller;
    private Vector3 _velocity;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // 基础移动输入
        float horizontal = Input.GetAxis("Horizontal"); // A/D 或 ←/→
        float vertical = Input.GetAxis("Vertical");     // W/S 或 ↑/↓
        
        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        _controller.Move(move * moveSpeed * Time.deltaTime);

        // 简单重力模拟
        if(_controller.isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }
        _velocity.y += gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
    }
}
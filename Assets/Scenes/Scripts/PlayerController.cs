using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // 移动速度
    public float rotationSpeed = 10f; // 旋转速度
    public float mouseSensitivity = 100f;
    float xRotation = 0f;

    private CharacterController characterController;

    void Start()
    {
        // 获取 CharacterController 组件
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
       MoveControlByTranslateGetAxis();

    }
        void MoveControlByTranslateGetAxis()
    {
        float horizontal = Input.GetAxis("Horizontal"); //A D 左右
        float vertical = Input.GetAxis("Vertical"); //W S 上 下
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;


        transform.Translate(Vector3.forward * vertical * moveSpeed * Time.deltaTime);//WS 前后
        transform.Translate(Vector3.right * horizontal * rotationSpeed * Time.deltaTime);//AD 左右
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // 应用旋转
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}
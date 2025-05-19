using UnityEngine;
using UnityEngine.UI; // 引入 UI 命名空间

public class Sa_1 : MonoBehaviour
{
    public GameObject Arm_1; // 肩关节
    public GameObject Arm_2; // 线性升降
    public GameObject Arm_3; // 肘关节
    public GameObject Arm_4; // 末端执行器

    public float L1 = 0.027f; // Arm_1 到 Arm_3 的长度
    public float L2 = 0.111f; // Arm_3 到 Arm_4 的长度

    public Transform target; // 目标点（机械臂的子对象）

    public float minArm2Y = 10f; // Arm_2 的最小高度
    public float maxArm2Y = 10f; // Arm_2 的最大高度
    public float arm2MoveSpeed = 3f; // Arm_2 的移动速度
    public float arm4MoveSpeed = 10f; // Arm_4 的旋转速度

    public Vector3 moveDirection = Vector3.right; // 目标点的移动方向
    public float moveSpeed = 3f; // 目标点的移动速度
    public float moveDistance = 5f; // 目标点的移动范围

    private Vector3 initialPosition; // 目标点的初始位置
    private bool movingForward = true; // 控制目标点的移动方向
    private Vector3 previousTargetPosition; // 记录目标点的上一帧位置
    public float circleRadius = 2f; // 圆形轨迹的半径
    public float circleSpeed = 0.5f; // 圆形轨迹的运动速度

    private float angle; // 当前角度
    private bool isCircleTrajectory = false; // 是否使用圆形轨迹

    public Button lineButton; // 直线轨迹按钮
    public Button circleButton; // 圆形轨迹按钮

    void Start()
    {
        if (target != null)
        {
            initialPosition = target.localPosition; // 记录目标点的初始位置
            previousTargetPosition = target.localPosition; // 初始化上一帧位置
        }

        if (Arm_4 != null)
        {
            // 初始化 Arm_4 的姿态为默认朝向
            Arm_4.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        // 绑定按钮点击事件
        if (lineButton != null)
        {
            circleButton.onClick.AddListener(SetCircleTrajectory);
        }

        if (circleButton != null)
        {
            
            lineButton.onClick.AddListener(SetLineTrajectory);
        }
    }

    void Update()
    {
        if (target != null)
        {
            // 根据当前轨迹模式执行对应的运动逻辑
            if (isCircleTrajectory)
            {
                MoveTargetInCircle();
            }
            else
            {
                MoveTarget();
            }

            // 使用目标点的局部坐标计算逆运动学
            ComputeIK(target.localPosition);

            // 更新夹爪的旋转
            UpdateArm4RotationWithTrajectory();
        }
    }

    void SetLineTrajectory()
    {
        isCircleTrajectory = false; // 切换为直线轨迹
    }

    void SetCircleTrajectory()
    {
        isCircleTrajectory = true; // 切换为圆形轨迹
    }

    void MoveTarget()
    {
        if (movingForward)
        {
            target.localPosition += moveDirection * moveSpeed * Time.deltaTime;

            // 检查是否到达最大移动距离
            if ((target.localPosition - initialPosition).sqrMagnitude >= moveDistance * moveDistance)
            {
                movingForward = false; // 改变方向
            }
        }
        else
        {
            target.localPosition -= moveDirection * moveSpeed * Time.deltaTime;

            // 检查是否回到初始位置
            if ((target.localPosition - initialPosition).sqrMagnitude <= 0.05f * 0.05f) // 增大阈值
            {
                movingForward = true; // 改变方向
            }
        }
    }

    void MoveTargetInCircle()
    {
        // 增加角度，控制运动速度
        angle += circleSpeed * Time.deltaTime;

        // 计算目标点在圆形轨迹上的位置
        float x = initialPosition.x + circleRadius * Mathf.Cos(angle);
        float z = initialPosition.z + circleRadius * Mathf.Sin(angle);

        // 更新目标点的位置
        target.localPosition = new Vector3(x, target.localPosition.y, z);

        // 更新 Arm_4 的旋转，使其始终面向前方
        if (Arm_4 != null)
        {
            Vector3 directionToTarget = new Vector3(-Mathf.Sin(angle), 0, Mathf.Cos(angle));
            float targetAngleY = Mathf.Atan2(directionToTarget.x, directionToTarget.z) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, targetAngleY, 0);
            Arm_4.transform.localRotation = Quaternion.Lerp(Arm_4.transform.localRotation, targetRotation, arm4MoveSpeed * Time.deltaTime);
        }
    }

    public void ComputeIK(Vector3 localTargetPosition)
    {
        // 计算水平距离 R 和高度 Z
        float R = Mathf.Sqrt(localTargetPosition.x * localTargetPosition.x + localTargetPosition.z * localTargetPosition.z);
        float Z = localTargetPosition.y;

        // 检查目标点是否在可达范围内
        if (R > (L1 + L2) || R < Mathf.Abs(L1 - L2))
        {
            Debug.LogWarning("目标点超出机械臂可达范围！");
            return;
        }

        // 计算 Arm_1 的旋转角度
        float theta1 = Mathf.Atan2(localTargetPosition.x, localTargetPosition.z) * Mathf.Rad2Deg;

        // 计算 Arm_3 的旋转角度
        float cosTheta3 = -(L1 * L1 + L2 * L2 - R * R) / (2 * L1 * L2);
        cosTheta3 = Mathf.Clamp(cosTheta3, -1f, 1f); // 防止超出范围
        float theta3 = Mathf.Acos(cosTheta3) * Mathf.Rad2Deg;

        // 检查计算结果是否合理
        if (float.IsNaN(theta1) || float.IsNaN(theta3))
        {
            Debug.LogError("逆运动学计算失败：角度计算结果无效！");
            return;
        }

        // 计算 Arm_2 的线性位置
        float arm2Position = Z;

        // 应用到机械臂
        UpdateArm1Rotation(theta1);
        UpdateArm2Position(arm2Position);
        UpdateArm3Rotation(theta3);

        Debug.Log($"目标点: {localTargetPosition}, Theta1: {theta1}, Theta3: {theta3}, Arm2 Position: {arm2Position}");
    }

    void UpdateArm1Rotation(float value)
    {
        Arm_1.transform.localRotation = Quaternion.Euler(0, value - 200, 0);
    }

    void UpdateArm2Position(float value)
    {
        // 限制 Arm_2 的高度范围
        value = Mathf.Clamp(value, minArm2Y, maxArm2Y);

        // 使用插值平滑移动 Arm_2
        float currentY = Arm_2.transform.localPosition.y;
        float newY = Mathf.Lerp(currentY, value, arm2MoveSpeed * Time.deltaTime);

        Arm_2.transform.localPosition = new Vector3(Arm_2.transform.localPosition.x, newY, Arm_2.transform.localPosition.z);
    }

    void UpdateArm3Rotation(float value)
    {
        Arm_3.transform.localRotation = Quaternion.Euler(0, value - 15, 0);
    }

    void UpdateArm4RotationWithTrajectory()
    {
        if (Arm_4 != null && Arm_1 != null)
        {
            // 获取机械臂的朝向（Arm_1 的局部旋转）
            float arm1YRotation = Arm_1.transform.localRotation.eulerAngles.y;

            // 获取目标点的运动方向
            Vector3 directionToTarget = -target.localPosition - previousTargetPosition;

            // 如果目标点没有移动，保持当前旋转
            if (directionToTarget.sqrMagnitude < 0.0001f)
            {
                return;
            }

            // 归一化方向向量
            directionToTarget.Normalize();

            // 计算夹爪的目标旋转角度
            float targetAngleY = -Mathf.Atan2(directionToTarget.x, directionToTarget.z) * Mathf.Rad2Deg;

            // 计算最终的旋转角度，使夹爪始终朝向目标点的运动方向
            float finalAngleY = -arm1YRotation + targetAngleY-85;

            // 使用插值平滑旋转
            Quaternion targetRotation = Quaternion.Euler(0, finalAngleY, 0);
            Arm_4.transform.localRotation = Quaternion.Lerp(Arm_4.transform.localRotation, targetRotation, arm4MoveSpeed * Time.deltaTime);

            // 更新上一帧位置
            previousTargetPosition = target.localPosition;
        }
    }
}
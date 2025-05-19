using UnityEngine;

public class Sa_2 : MonoBehaviour
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

    public float circleRadius = 2f; // 圆形轨迹的半径
    public float circleSpeed = 1f; // 圆形轨迹的运动速度

    private Vector3 initialPosition; // 目标点的初始位置
    private float angle; // 当前角度

    void Start()
    {
        if (target != null)
        {
            initialPosition = target.localPosition; // 记录目标点的初始位置
        }

        if (Arm_4 != null)
        {
            // 初始化 Arm_4 的姿态为默认朝向
            Arm_4.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    void Update()
    {
        if (target != null)
        {
            // 控制目标点的圆形轨迹运动
            //MoveTargetInCircle();

            // 使用目标点的局部坐标计算逆运动学
            ComputeIK(target.localPosition);

            // 更新夹爪的旋转
            UpdateArm4RotationWithTrajectory();
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
            Vector3 directionToTarget = target.localPosition - initialPosition;

            // 如果目标点没有移动，保持当前旋转
            if (directionToTarget.sqrMagnitude < 0.0001f)
            {
                return;
            }

            // 归一化方向向量
            directionToTarget.Normalize();

            // 计算夹爪的目标旋转角度
            float targetAngleY = Mathf.Atan2(-directionToTarget.x, -directionToTarget.z) * Mathf.Rad2Deg;

            // 计算最终的旋转角度，使夹爪始终朝向前方
            float finalAngleY = arm1YRotation + targetAngleY;

            // 使用插值平滑旋转
            Quaternion targetRotation = Quaternion.Euler(0, -finalAngleY - 90, 0);
            Arm_4.transform.localRotation = Quaternion.Lerp(Arm_4.transform.localRotation, targetRotation, arm4MoveSpeed * Time.deltaTime);
        }
    }
}
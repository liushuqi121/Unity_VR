using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum ArmState
{
    Idle,
    GoingDown,
    GoingUp,
    RotatingRight,
    GoingDownAgain,
    Done
}

public class a : MonoBehaviour
{
    public GameObject Arm_1, Arm_2, Arm_3, Arm_4, target;
    public airWall_1 airWall;
    public Airwall_2 airWall_2;
    public A4_1 a4_1;
    public float Arm_2_speed = 0.05f;

    public float rotateDuration = 3f;
    private ArmState currentState = ArmState.Idle;
    public Airwall_3 airwall_3;
    public GameObject a4;
    


    private void Update()
    {

        switch (currentState)
        {
            case ArmState.Idle:
                if (airWall != null && airWall.isTriggered == false && a4_1.is_1)
                {
                    currentState = ArmState.GoingDown;
                }
                break;

            case ArmState.GoingDown:
                Down();
                if (airWall.is_2 == false) // 下降触发空气墙
                {
                    currentState = ArmState.GoingUp;
                }
                break;

            case ArmState.GoingUp:
                Up();
                if (airWall_2 != null && airWall_2.isTriggered_2 == false) // 上升触发空气墙2
                {
                    currentState = ArmState.RotatingRight;
                    StartCoroutine(RotateArm1Coroutine(-90, rotateDuration));
                    airWall_2.isTriggered_2 = true;
                }
                break;

            case ArmState.RotatingRight:
                // 旋转完成后由协程内部回调进入下一步
                break;

            case ArmState.GoingDownAgain:
                Down();
                if (airwall_3.isTriggered_3 == false) // 最后一次下降也触发空气墙
                {
                    currentState = ArmState.Done;
                    target.transform.SetParent(null);
                }
                break;

            case ArmState.Done:
                StartCoroutine(RotateArm1Coroutine(0, rotateDuration));
                StopAndRecordPose(); // 最终停止并记录姿态
                
                break;
        }
    }


    void Down()
    {
        Arm_2.transform.position -= Vector3.up * Arm_2_speed * Time.deltaTime;
    }

    void Up()
    {
        Arm_2.transform.position += Vector3.up * Arm_2_speed * Time.deltaTime;
        target.transform.SetParent(Arm_4.transform);
    }

    IEnumerator RotateArm1Coroutine(float targetAngleY, float duration)
    {
        Quaternion startRotation = Arm_1.transform.localRotation;
        Quaternion targetRotation = Quaternion.Euler(0, targetAngleY, 0);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            Arm_1.transform.localRotation = Quaternion.Lerp(startRotation, targetRotation, t);
            yield return null;
        }

        Arm_1.transform.localRotation = targetRotation;
        currentState = ArmState.GoingDownAgain; // 旋转完后进入下一阶段
    }

    void StopAndRecordPose()
    {
        Arm_2_speed = 0f;
        Debug.Log("姿态记录完成");
    }
}

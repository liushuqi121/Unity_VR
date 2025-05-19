using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airwall_2 : MonoBehaviour
{
    public bool isTriggered_2 = true; // 是否触发的状态
    
    void Start()
    {
                // 检查是否正确设置了 Collider
        if (!GetComponent<Collider>())
        {
            Debug.LogError("No Collider attached to airWall_1!");
        }

        // 确保 Collider 是触发器
        if (GetComponent<Collider>() && !GetComponent<Collider>().isTrigger)
        {
            Debug.LogWarning("Collider is not set as a trigger on airWall_1!");
        }
    }
            void OnTriggerEnter(Collider other)
            {
                if (other.gameObject.CompareTag("arm")) // 使用 CompareTag 避免硬编码错误
                {
                    isTriggered_2 = false;
                }
            }

}

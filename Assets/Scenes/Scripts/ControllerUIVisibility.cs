using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class ControllerUIVisibility : MonoBehaviour
{
    public GameObject uiObject;
    public bool isUIVisible = true;

    void Start (){

    }
    void Update()
    {
    foreach (XRNode node in new XRNode[] { XRNode.LeftHand, XRNode.RightHand })
        {
            InputDevice device = InputDevices.GetDeviceAtXRNode(node);

            if (device.isValid)
            {
                bool pressed;
                // 检测菜单键（MenuButton）是否按下
                if (device.TryGetFeatureValue(CommonUsages.primaryButton, out pressed) && pressed)
                {
                    ToggleUI(); // 切换 UI 显示状态
                }
            }
    }
}
    void ToggleUI()
    {
        isUIVisible = !isUIVisible; // 切换状态
        //uiPanel.SetActive(isUIVisible); // 显示或隐藏 UI
        Debug.Log("UI 现在的状态: " + (isUIVisible ? "显示" : "隐藏"));
    }
}


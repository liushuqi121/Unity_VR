using UnityEngine;
using UnityEngine.UI;

public class Button_replace : MonoBehaviour
{
    public bool Replace_1 = false; // 给外部访问用的开关

    public Button replaceButton;   // 关联到 Unity 场景里的按钮

    void Start()
    {
        replaceButton.onClick.AddListener(OnReplaceButtonClicked);
    }

    void OnReplaceButtonClicked()
    {
        Replace_1 = true; // 按下按钮，标记为要复位
    }
}

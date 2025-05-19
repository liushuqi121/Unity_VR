using UnityEngine;
using UnityEngine.UI;

public class ScaraArmController : MonoBehaviour
{
    public Slider arm1Slider; // 控制 arm1 绕 Y 轴旋转
    public Slider arm2Slider; // 控制 arm2 沿 Y 轴上下移动
    public Slider arm3Slider; // 控制 arm3 伸缩
    public Slider arm4Slider; // 控制 arm4 末端关节角度

    public GameObject arm1;
    public GameObject arm2;
    public GameObject arm3;
    public GameObject arm4;
    float arm2MoveSpeed = 0.05f;

    void Start()
    {
        arm1Slider.onValueChanged.AddListener(delegate { UpdateArm1(); });
        arm2Slider.onValueChanged.AddListener(delegate { UpdateArm2(); });
        arm3Slider.onValueChanged.AddListener(delegate { UpdateArm3(); });
        arm4Slider.onValueChanged.AddListener(delegate { UpdateArm4(); });
    }

    void UpdateArm1()
    {
        arm1.transform.localEulerAngles = new Vector3(0, arm1Slider.value, 0);
    }

    void UpdateArm2()
    {
        Vector3 pos = arm2.transform.localPosition;
        pos.y = arm2Slider.value;
        arm2.transform.localPosition = pos;
    }

    void UpdateArm3()
    {
        arm3.transform.localEulerAngles = new Vector3(0, arm3Slider.value,0 );
    }

    void UpdateArm4()
    {
        arm4.transform.localEulerAngles = new Vector3(0, arm4Slider.value,0 );
    }
}

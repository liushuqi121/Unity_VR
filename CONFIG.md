
---

### 📁 配置说明（另可放入 `docs/CONFIG.md`）

```markdown
# Unity 项目配置说明

## 1. Unity 编辑器配置

- 渲染管线：使用内置渲染管线（Built-in RP）
- 插件依赖：
  - Oculus XR Plugin (用于 VR 支持)
  - ROS-TCP-Connector (用于 ROS 通信)
- 层级结构建议命名清晰，如：
  - `Arm_Base`, `Joint_1`, `Joint_2`, `End_Effector` 等

## 2. VR 控制配置

- 控制方式：VR 控制器绑定 UI 滑动条，控制机械臂运动
- 推荐插件：
  - `XR Interaction Toolkit`
  - `Oculus Integration`（需在 Unity Asset Store 获取）

## 3. ROS 通信配置（可选）

- Unity IP：设置为 Linux 主机可访问的地址
- 通信端口：默认 `10000`，可在 `ROSConnection` 组件中修改
- 消息类型：
  - 发布：`pos_rot`（自定义消息）
  - 订阅：`coordinates`（geometry_msgs/Point）

## 4. 注意事项

- 若项目打开后界面为空，请重新导入场景 `MainScene`。
- 若 VR 无法启动，检查 Oculus 驱动和 USB 连接。
- 建议将 `.log` 日志文件添加 `.gitignore`，避免版本控制污染。

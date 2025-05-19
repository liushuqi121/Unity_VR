# 基于 Unity3D 的 VR 仿真教学平台

本项目为本科毕业设计的一部分，旨在基于 Unity3D 实现一个 SCARA 机械臂的虚拟仿真教学平台，结合 VR 技术提升用户交互体验。系统支持滑动条控制、动画状态机、物理碰撞反馈等功能，并与 ROS2 实现跨设备通信。

## 📁 项目结构

.
├── Assets/ # 项目核心资源，包括模型、脚本、UI 等
├── Packages/ # Unity Package 管理器依赖
├── ProjectSettings/ # Unity 工程设置
├── .vscode/ # VSCode 编辑器配置
├── README.md # 项目说明文档
├── .gitignore # Git 忽略文件配置
├── ps_driver_sdk*.log # 外设或驱动日志（可选上传）

## 🔧 环境要求

- Unity 版本：**2021.3 LTS** 或兼容版本
- 操作系统：Windows 10/11
- VR设备支持：Oculus Rift / Quest（使用 Oculus Link 模式）
- ROS2 版本：Foxy / Humble（根据平台配置）
- .NET Framework：4.x 或 Unity 默认支持的版本

## 🚀 如何运行项目

1. 克隆或下载项目到本地：

   ```bash
   git clone https://github.com/liushuqi121/your-project-name.git
使用 Unity Hub 打开项目所在文件夹。

若使用 VR 功能，请确保已连接 VR 头显设备并安装 Oculus/SteamVR 支持插件。

点击 Unity 播放按钮即可进入模拟环境。  

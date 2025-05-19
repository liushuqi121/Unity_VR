# ⚙️ config.md - 树莓派与ROS2环境配置说明

本配置文档用于指导在树莓派（Raspberry Pi 4）上搭建 ROS 2 通信环境，以实现 Unity 与 ROS2 的跨设备通信。

---

## 🧾 系统要求

| 项目         | 要求版本                   |
|--------------|----------------------------|
| 硬件         | Raspberry Pi 4 (4GB/8GB)   |
| 操作系统     | Ubuntu 20.04 64-bit        |
| ROS 2版本    | ROS 2 Foxy Fitzroy         |
| Python版本   | Python ≥ 3.8               |
| 网络连接     | 与 Unity 主机同局域网      |

---

## 📦 安装 ROS 2 Foxy

### 步骤 1：配置源

```bash
sudo locale-gen en_US en_US.UTF-8
sudo update-locale LC_ALL=en_US.UTF-8 LANG=en_US.UTF-8
bash
复制
编辑
sudo apt update && sudo apt install curl gnupg2 lsb-release
sudo curl -sSL https://raw.githubusercontent.com/ros/rosdistro/master/ros.key -o /usr/share/keyrings/ros-archive-keyring.gpg
bash
复制
编辑
echo "deb [arch=arm64 signed-by=/usr/share/keyrings/ros-archive-keyring.gpg] http://packages.ros.org/ros2/ubuntu $(lsb_release -cs) main" | sudo tee /etc/apt/sources.list.d/ros2.list > /dev/null
步骤 2：安装 ROS 2 Foxy
bash
复制
编辑
sudo apt update
sudo apt install ros-foxy-desktop
步骤 3：配置环境变量
bash
复制
编辑
echo "source /opt/ros/foxy/setup.bash" >> ~/.bashrc
source ~/.bashrc
🔧 创建工作区并编译消息包
bash
复制
编辑
mkdir -p ~/ros2_ws/src
cd ~/ros2_ws
colcon build
source install/setup.bash
若需自定义消息（例如 PosRotMsg）
在 src/ 目录下创建 ROS2 接口包：

bash
复制
编辑
ros2 pkg create --build-type ament_cmake --dependencies std_msgs geometry_msgs custom_interface
添加 .msg 文件至 msg/ 文件夹中（例如 PosRotMsg.msg）：

plaintext
复制
编辑
float64 pos_x
float64 pos_y
float64 pos_z
float64 rot_y
修改 CMakeLists.txt 和 package.xml 支持自定义消息。

编译并安装：

bash
复制
编辑
cd ~/ros2_ws
colcon build
source install/setup.bash
🌐 配置 ROS 与 Unity 通信
Unity 通过 ROS-TCP-Connector 插件连接树莓派。你需要在树莓派上运行 TCP 服务端节点。

步骤 1：安装 ros_tcp_endpoint
bash
复制
编辑
cd ~/ros2_ws/src
git clone https://github.com/Unity-Technologies/ROS-TCP-Endpoint.git
cd ~/ros2_ws
colcon build
source install/setup.bash
步骤 2：启动 ROS TCP 服务端
bash
复制
编辑
ros2 run ros_tcp_endpoint default_server_endpoint --ros-args -p ROS_IP:=<树莓派IP地址>
例如：

bash
复制
编辑
ros2 run ros_tcp_endpoint default_server_endpoint --ros-args -p ROS_IP:=192.168.3.25
🔁 自动启动 TCP 服务（可选）
编辑 ~/.bashrc：

bash
复制
编辑
echo "ros2 run ros_tcp_endpoint default_server_endpoint --ros-args -p ROS_IP:=192.168.3.25" >> ~/.bashrc
或者使用 systemd 设置为服务启动项。

📶 网络配置建议
Unity 主机与树莓派需位于同一局域网；

可通过 ifconfig 或 ip a 获取树莓派的实际 IP 地址；

可使用如下命令测试端口连通性：

bash
复制
编辑
telnet <树莓派IP> 10000
✅ 验证通信状态
Unity端：

在 Unity 中配置 ROSConnection；

设置 IP 地址为树莓派 IP；

配置 ROS 服务端口为 10000；

启动播放模式查看连接状态；

使用控制UI发布消息查看终端订阅是否成功。

树莓派端：

在终端中查看 ROS2 服务端是否接收消息；

可编写 ROS2 节点订阅 Unity 发布的 pos_rot，或向 Unity 发布 coordinates。

🔚 故障排查
问题	可能原因与解决方案
Unity端提示“connection refused”	确认树莓派 TCP 服务端已启动，IP 配置无误
ROS2 接收不到消息	检查话题名是否一致，ROSConnection 是否发布到正确话题
无法解析消息	确认 Unity 与树莓派编译了相同的自定义消息类型
端口被占用	更换通信端口或重启 ros_tcp_endpoint 服务

📜 附录：命令清单
bash
复制
编辑
# 编译工作区
cd ~/ros2_ws && colcon build && source install/setup.bash

# 启动 TCP 服务
ros2 run ros_tcp_endpoint default_server_endpoint --ros-args -p ROS_IP:=192.168.3.25

# 测试消息
ros2 topic list
ros2 topic echo /pos_rot
ros2 topic pub /coordinates geometry_msgs/Point "{x: 1.0, y: 2.0, z: 3.0}"
📌 作者说明
本配置用于本科毕设《基于Unity与ROS2的SCARA机械臂VR虚拟仿真教学平台》配套文档，旨在提供完整的跨平台部署说明。

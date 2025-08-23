# 安装

有两种方式进行安装。

在安装之前，请确保你的电脑上有你想要版本的跑跑卡丁车客户端。
对于跑跑卡丁车客户端，不限制版本，只要是国服均可。

## 1. 下载二进制文件

前往 [Release](https://github.com/TheMagicFlute/Launcher_V2/latest "latest release") 或 [Yany Release](https://github.com/yanygm/Launcher_V2/releases/latest) 并根据操作系统下载最新的二进制文件压缩包，解压即可。

此方法**容易报毒**！提前关闭 Windows Defender 对相关目录的扫描，或者添加信任。

## 2. 命令行

1. 确保已经安装了 [.NET 8.0](https://dotnet.microsoft.com/download/dotnet/8.0 ".NET 8.0") 或以上版本的运行时。
2. 确保已经安装了 [Git](https://git-scm.com/downloads "Git")。
3. 运行以下命令

```sh
# 克隆仓库
git clone https://github.com/TheMagicFlute/Launcher_V2.git --depth=1
cd Launcher_V2
```

- 如果你的系统是 64 位的，运行以下命令：

```sh
# 构建 x64
dotnet publish Launcher.csproj --runtime win-x64 -p:PublishSingleFile=true -p:AssemblyName=Launcher -o dist -c Release --no-self-contained
# 运行
./dist/Launcher.exe
```

- 如果你的系统是 32 位的，运行以下命令：

```sh
# 构建 x86
dotnet publish Launcher.csproj --runtime win-x86 -p:PublishSingleFile=true -p:AssemblyName=Launcher -o dist -c Release --no-self-contained
# 运行
./dist/Launcher.exe
```

- 如果你的系统是 Arm64 的，运行以下命令：

```sh
# 构建 Arm64
dotnet publish Launcher.csproj --runtime win-arm64 -p:PublishSingleFile=true -p:AssemblyName=Launcher -o dist -c Release --no-self-contained
# 运行
./dist/Launcher.exe
```

此后，你可以将 `Launcher.exe` 移动到任意目录运行。

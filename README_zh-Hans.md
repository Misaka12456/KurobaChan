<div align="center">

# KurobaChan

[![Misaka Castle Member Project](https://img.shields.io/badge/Misaka%20Castle-Memeber%20Project-fuchsia)](https://misakacastle.moe) ![123 Open-Source Organization 10 Years Appointment Member Project](https://img.shields.io/badge/Team123it%2010%20Years%20Appointment-Member%20Project-brightgreen)

轻量级，快速，易用的Windows全局老板键(Bosskey)实用工具，使用纯WPF和C#编写。

[English](README.md) | **&gt; 简体中文 &lt;**
</div>

## 免责声明
Project KurobaChan, including its developers, contributors, and affiliated individuals or entities, hereby explicitly disclaim any association with, support for, or endorsement of any form of illegal behavior, including but not limited to piracy, hacking, and unauthorized access to computer systems. This disclaimer extends to any use or application of the Project KurobaChan that may be contrary to local, national, or international laws, regulations, or ethical guidelines.

Also, this project is not closely related to [Locale Emulator](https://github.com/xupefei/Locale-Emulator) project. KurobaChan *only* support the technology for the software(s) that need Locale Emulator to run properly.

By using or accessing Project KurobaChan, the user acknowledges and agrees to release the developers, contributors, and affiliated individuals or entities from any and all liability arising from the use or misuse of the project, including any legal consequences incurred as a result of their actions.

Please use KurobaChan resposibly and in accordance with the law.

## 快速上手
1. 从[Releases](https://github.com/Misaka12456/KurobaChan/releases)页下载最新版本并解压。
2. 运行 ``KurobaChan.exe`` 启动程序。（系统可能会提示需要UAC权限，请允许——该权限是能够实现系统级低级键盘钩子(Low-Level keyboard hook)的必要权限）
3. 选择 `File`->`Add Software` 添加需要隐藏的软件。（您将会被要求至少启动一次软件）
4. 添加完成后，点击KurobaChan主窗口按F5刷新运行中的已记录软件列表。
5. 现在当您需要隐藏窗口时，只需在目标软件窗口中按 `Esc` 键即可。  
   要恢复窗口，双击KurobaChan中对应窗口信息的所在行即可。

## 构建提示
- KurobaChan使用.NET 8.0构建，因此需要安装.NET 8.0 SDK才能构建项目。
- 请注意：由于Win32 API兼容性、WPF兼容性及基于ETW(Event Tracing for Windows,适用于Windows的事件跟踪机制)的进程实时检测支持情况等原因，KurobaChan仅支持Windows 10 1903版本及以后的版本。

## 为KurobaChan企划献一份力
欢迎任何形式的对KurobaChan的贡献，包括但不限于：
- 问题(Bug)反馈 (Issue)
- 功能请求 (Issue)
- 多语言翻译支持 (PR)
- 项目代码贡献 (PR)
- 对内置的**Locale Emulator依赖软件列表**的完善 (Issue/PR)

## 开源协议
KurobaChan基于GNU 通用公共许可证(Genral Public License) v3.0发布。更多细节请参见[LICENSE](LICENSE)文档。
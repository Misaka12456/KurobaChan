<div align="center">

# KurobaChan

[![Misaka Castle Member Project](https://img.shields.io/badge/Misaka%20Castle-Memeber%20Project-fuchsia)](https://misakacastle.moe) ![123 Open-Source Organization 10 Years Appointment Member Project](https://img.shields.io/badge/Team123it%2010%20Years%20Appointment-Member%20Project-brightgreen)

Light-weight, fast, and easy-to-use global bosskey utility for Windows, written by pure WPF and C#.

**&gt; English &lt;** | [简体中文](README_zh-Hans.md)
</div>

## Disclaimer
Project KurobaChan, including its developers, contributors, and affiliated individuals or entities, hereby explicitly disclaim any association with, support for, or endorsement of any form of illegal behavior, including but not limited to piracy, hacking, and unauthorized access to computer systems. This disclaimer extends to any use or application of the Project KurobaChan that may be contrary to local, national, or international laws, regulations, or ethical guidelines.

Also, this project is not closely related to [Locale Emulator](https://github.com/xupefei/Locale-Emulator) project. KurobaChan *only* support the technology for the software(s) that need Locale Emulator to run properly.

By using or accessing Project KurobaChan, the user acknowledges and agrees to release the developers, contributors, and affiliated individuals or entities from any and all liability arising from the use or misuse of the project, including any legal consequences incurred as a result of their actions.

Please use KurobaChan resposibly and in accordance with the law.

## Quick Start
1. Download the [release](https://github.com/Misaka12456/KurobaChan/releases) and unzip it. 
2. Run 'KurobaChan.exe' to launch the program. (needs UAC privilege for Global Low-Level keyboard hook)
3. Select `File`->`Add Software` to add the software you want to hide. (You will be required to start the software at least once during the wizard process)
4. When added, click F5 on KurobaChan main window to refresh the active software list.
5. Now, if you need to hide the window, just click `Esc` in the target software window.  
   To resume the window, double-click the window info row in KurobaChan.

## Build Hint
- KurobaChan was built with .NET 8.0 with WPF support. You need to install the .NET 8.0 SDK to build the project.
- Please note that due to Win32 API compatibility, WPF compatibility & ETW(Event Tracing for Windows)-based process real-time detection support, KurobaChan only supports Windows 10 1903 and later versions.

## Contribute to KurobaChan
Welcome any kind of contribution to KurobaChan, including but not limited to:
- Bug report (Issue)
- Feature request (Issue)
- Translation Support (PR)
- Code Contribution (PR)
- Built-In Locale Emulator Required Software List Completion (Issue/PR)

## License
Licensed under GNU General Public License v3.0. See [LICENSE](LICENSE) for more details.
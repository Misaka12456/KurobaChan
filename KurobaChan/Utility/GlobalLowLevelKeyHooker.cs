/** Global Low-Level Key Hooker - By Misaka12456 (from Misaka Castle)
 *  Licensed under GNU General Public License (AGPL) v3.0.
 *  Due to possible sensitive information obtain by Low-Level Keyboard Hooker, this class is not allowed to be used in any malicious way.
 *  See https://github.com/Misaka12456/KurobaChan/blob/main/PrivacyPolicy.md for more information.
 */
// ReSharper disable LocalizableElement

using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Input;
using KurobaChan.Utility.Extension;
using KurobaChan.Enumerator;
using static KurobaChan.Utility.NativeMethods.User32;
using ModifierKeys = KurobaChan.Enumerator.ModifierKeys;

namespace KurobaChan.Utility;

internal sealed class GlobalLowLevelKeyHooker : IDisposable
{
	/// <summary>
	/// <b>
	/// CAUTION: Please notice that this trigger function WILL NOT CATCH ANY EXCEPTION.<br />
	/// If you want more security, please HANDLE EXCEPTIONS BY TRY-CATCH EXPLICITLY, or the global lowlevel keyboard hook may be left over and out of control.<br />
	/// </b>
	/// To manually unhook the hooker outside KurobaChan, please use 'OpenARK' and select 'Kernel'->'Enter Kernel Mode'->(Restart tool (UAC required))->'Kernel'->'System Hotkeys' and remove the hooker.
	/// </summary>
	public event HookTriggeredEventHandler? OnHookTriggered;
	
	public int HookId => hookId.ToInt32();

	private LowLevelKeyboardProc proc;
	private readonly IntPtr hookId;
	private readonly Key keyToHook;
	private readonly ModifierKeys modifiers;
	private bool disposed;

	public GlobalLowLevelKeyHooker(Key hotkey, ModifierKeys modifiers = ModifierKeys.None)
	{
		keyToHook = hotkey;
		this.modifiers = modifiers;
		proc = HookCallback;
		hookId = SetHook(proc);
		string logStr = $"[Debug/Trace] Hooker with id {hookId} has been hooked, as key {hotkey}";
		if (modifiers != ModifierKeys.None)
		{
			string modifiersStr = string.Join(" + ", modifiers.UnpackFlags().Select(f => f.ToString()));
			logStr = $" (with modifiers {modifiersStr})";
		}
	}

	private static IntPtr SetHook(LowLevelKeyboardProc proc)
	{
		using var current = Process.GetCurrentProcess();
		using var currModule = current.MainModule ?? throw new InvalidOperationException("Failed to initialize hook because the main module of hook host process is null.");
		return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(currModule.ModuleName), 0);
	}

	private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
	{
		if (nCode >= 0 && wParam == WM_KEYDOWN)
		{
			var hookStruct = Marshal.PtrToStructure<KeyBoardLowLevelHookStruct>(lParam);
			int vkCode = hookStruct.VKCode;
			
			var currentModifier = ModifierKeys.None;
			if ((GetKeyState(VK_CONTROL) & 0x8000) != 0)
			{
				currentModifier |= ModifierKeys.Control;
			}
			if ((GetKeyState(VK_MENU) & 0x8000) != 0)
			{
				currentModifier |= ModifierKeys.Alt;
			}
			if ((GetKeyState(VK_SHIFT) & 0x8000) != 0)
			{
				currentModifier |= ModifierKeys.Shift;
			}
			if ((GetKeyState(VK_LWIN) & 0x8000) != 0 || (GetKeyState(VK_RWIN) & 0x8000) != 0)
			{
				currentModifier |= ModifierKeys.Win;
			}
			
			if (vkCode == KeyInterop.VirtualKeyFromKey(keyToHook) && currentModifier == modifiers)
			{
				var currentWindowHandle = GetForegroundWindow(); // 获取实际最前窗口句柄(极大概率不是当前KurobaChan的主窗口)
				try
				{
					if (OnHookTriggered != null && OnHookTriggered(this, new HookTriggeredEventArgs
					    {
						    Key = keyToHook, WindowHandle = currentWindowHandle, KeyEventResult = hookStruct,
						    FromProcess = GetHostProcessByWindow(currentWindowHandle)
					    }))
					{
						return 1; // 拦截按键消息
					}
				}
				catch (Exception ex)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine($"[Error] Exception occurred in hook trigger event handler: {ex}");
					Console.WriteLine("[Error] Will ignore any hook event handler and directly pass the key event.");
					return CallNextHookEx(hookId, nCode, wParam, lParam); // 正常传递按键消息
				}
			}
		}
		return CallNextHookEx(hookId, nCode, wParam, lParam); // 正常传递按键消息
	}

	private void ReleaseUnmanagedResources()
	{
		UnhookWindowsHookEx(hookId);
		Console.WriteLine($"[Trace/Debug] Hooker with id {hookId} has been unhooked and disposed.");
	}

	private void Dispose(bool disposing)
	{
		if (disposed) return;
		disposed = true;
		ReleaseUnmanagedResources();
		if (disposing)
		{
			proc = null!;
		}
	}

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	~GlobalLowLevelKeyHooker()
	{
		Dispose(false);
	}
}
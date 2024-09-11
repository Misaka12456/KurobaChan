using System.ComponentModel;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
// ReSharper disable LocalizableElement

namespace KurobaChan.Utility;

// "Heavyweight" S**T Code Warning
// I'm very recommend to just use the function(s) from this class. I STRONGLY don't recommend to modify this class, cause this class is too "heavyweight" to refactor. 
/// <summary>
/// Provides Win32 API functions for the Windows operating system, without wrapping them by programmers themselves.
/// </summary>
internal static class NativeMethods
{
	/// <summary>
	/// Provides access to functions from the user32.dll Windows library (which is used for <b>User GUI</b> (commonly)).
	/// </summary>
	public static class User32
	{
		/// <summary>
		/// Represents the action to restore the window (if it is minimized) and activate the window.
		/// </summary>
		public const int SW_RESTORE = 9;
		/// <summary>
		/// Represents the low-level (LL) keyboard hook.
		/// </summary>
		public const int WH_KEYBOARD_LL = 13;
		/// <summary>
		/// Represents the message that a key has been pressed.
		/// </summary>
		public const int WM_KEYDOWN = 0x0100;
		/// <summary>
		/// Virtual key code Control (Ctrl) key (supports both LCtrl and RCtrl).
		/// This is a modifier key.
		/// </summary>
		public const int VK_CONTROL = 0x11;
		/// <summary>
		/// Virtual key code Shift key (supports both LShift and RShift).
		/// This is a modifier key.
		/// </summary>
		public const int VK_SHIFT = 0x10;
		/// <summary>
		/// Virtual key code Alt key (supports both LAlt and RAlt).
		/// This is a modifier key.
		/// </summary>
		public const int VK_MENU = 0x12;
		/// <summary>
		/// Virtual key code Left 'Windows logo' key (LWin).
		/// This is a modifier key.
		/// </summary>
		public const int VK_LWIN = 0x5B;
		/// <summary>
		/// Virtual key code Right 'Windows logo' key (RWin).
		/// This is a modifier key.
		/// </summary>
		public const int VK_RWIN = 0x5C;
		/// <summary>
		/// To hide the window.
		/// </summary>
		public const int SW_HIDE = 0;
		/// <summary>
		/// To show the window in its current state.
		/// </summary>
		public const int SW_SHOW = 5;
		/// <summary>
		/// To minimize the window. <br />
		/// For Full-screen softwares (e.g. full-screen games), it's recommended to firstly minimize the window, then hide it.
		/// </summary>
		public const int SW_MINIMIZE = 6;
		
		private const uint FLASHW_ALL = 3;
		private const uint FLASHW_TIMERNOFG = 12;
		
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
		// Fetch the class name of the window

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetWindowText(IntPtr handle, StringBuilder lpString, int nMaxCount);
		
		[DllImport("user32.dll")]
		public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);
		
		[DllImport("user32.dll")]
		public static extern bool IsWindowVisible(IntPtr hWnd);
		
		[DllImport("user32.dll")]
		public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
		
		[DllImport("user32.dll")]
		public static extern IntPtr GetForegroundWindow();
		
		[DllImport("user32.dll")]
		public static extern bool SetForegroundWindow(IntPtr hWnd);
		
		[DllImport("user32.dll")]
		public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
		
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);
	
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);
	
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool UnhookWindowsHookEx(IntPtr hhk);
	
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);
	
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr GetModuleHandle(string lpModuleName);
	
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr FindWindowEx(IntPtr hWndParent, IntPtr hWndChildAfter, string lpszClass, string lpszWindow);
		
		[DllImport("user32.dll")]
		public static extern short GetKeyState(int nVirtKey); // for modifier keys (i.e. Ctrl, Alt, Shift, Win, etc.) state checking
		
		[DllImport("user32.dll", SetLastError = true)]
		public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
		
		[DllImport("user32.dll", SetLastError = true)]
		public static extern int GetWindowLong(IntPtr hWnd, int nIndex);
		
		[StructLayout(LayoutKind.Sequential)]
		public struct KeyBoardLowLevelHookStruct // KEYBDLLHOOKSTRUCT = [Keyb]oar[d] [L]ow [L]evel [Hook] [Struct]ure
		{
			public int VKCode;
			public int ScanCode;
			public int Flags;
			public int Time;
			public IntPtr ExtraInfo;

			public override string ToString()
			{
				return $"VKCode={VKCode}, ScanCode={ScanCode}, Flags={Flags}, Time={Time}, ExtraInfo={ExtraInfo}";
			}
		}
		
		[StructLayout(LayoutKind.Sequential)]
		public struct FlashWindowInfo // FLASHWINFO wrapped
		{
			public uint cbSize;
			public IntPtr hWnd;
			public uint dwFlags;
			public uint uCount;
			public uint dwTimeout;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct RECT
		{
			public int Left;
			public int Top;
			public int Right;
			public int Bottom;
			
			public Size Size => new Size(Right - Left, Bottom - Top);
		}
		
		public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
		
		public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

		public class WindowInfo : IComparable, IComparable<WindowInfo>
		{
			public Process HostProcess { get; init; } = null!;
			public IntPtr Handle { get; init; }
			public string ClassName { get; init; } = string.Empty;
			public string Title { get; init; } = string.Empty;
			public Vector2 Size { get; init; }
			public bool IsFullScreen => Math.Approximately(Size.X, SystemParameters.PrimaryScreenWidth) && Math.Approximately(Size.Y, SystemParameters.PrimaryScreenHeight);

			public void Flash()
			{
				ShowWindow(Handle, SW_RESTORE); // Restore the window if it's minimized
				SetForegroundWindow(Handle);
				// var flashInfo = new FlashWindowInfo
				// {
				// 	cbSize = (uint)Marshal.SizeOf(typeof(FlashWindowInfo)),
				// 	hWnd = Handle,
				// 	dwFlags = FLASHW_ALL | FLASHW_TIMERNOFG,
				// 	uCount = 0,
				// 	dwTimeout = 500
				// };
				// FlashWindowEx(ref flashInfo);
			}

			public int CompareTo(object? obj)
			{
				if (obj is WindowInfo other)
				{
					return CompareTo(other);
				}
				throw new ArgumentNullException(nameof(obj), "Cannot compare window (size) with a non-window info object.");
			}

			public int CompareTo(WindowInfo? other)
			{
				if (other != null)
				{
					float sizeSquare = Size.X * Size.Y;
					float otherSizeSquare = other.Size.X * other.Size.Y;
					return sizeSquare.CompareTo(otherSizeSquare);
				}
				throw new ArgumentNullException(nameof(other), "Cannot compare window (size) with a null window info.");
			}
		}

		public static string GetWindowClassName(IntPtr hWnd)
		{
			const int MAX_CLASS_NAME_LENGTH = 256;
			var className = new StringBuilder(MAX_CLASS_NAME_LENGTH);
			int hResult = GetClassName(hWnd, className, MAX_CLASS_NAME_LENGTH);
			return hResult != 0 ? className.ToString() : string.Empty;
		}

		public static string GetWindowTitle(IntPtr hWindow)
		{
			var sb = new StringBuilder(256);
			int hResult = User32.GetWindowText(hWindow, sb, sb.Capacity);
			return hResult != 0 ? sb.ToString() : string.Empty;
		}

		public static Vector2 GetWindowSize(IntPtr hWnd)
		{
			var rect = new RECT();
			GetWindowRect(hWnd, ref rect);
			return new Vector2(rect.Right - rect.Left, rect.Bottom - rect.Top);
		}
		
		public static IEnumerable<WindowInfo> GetVisibleWindows(string processName)
		{
			if (processName.EndsWith(".exe"))
			{
				processName = processName[..^4]; // win32 api 接收的进程名是不带.exe后缀的
			}
			var windows = new List<WindowInfo>();
			EnumWindows((hWnd, lParam) =>
			{
				if (!IsWindowVisible(hWnd)) return true;
				if (GetWindowThreadProcessId(hWnd, out uint processId) == 0)
				{
					throw new Win32Exception(Marshal.GetLastWin32Error());
				}
				var process = Process.GetProcessById((int)processId);
#if DEBUG
				Console.WriteLine(process.ProcessName);
#endif
				if (process.ProcessName == processName)
				{
					windows.Add(new WindowInfo
					{
						HostProcess = process,
						Handle = hWnd,
						ClassName = GetWindowClassName(hWnd),
						Title = GetWindowTitle(hWnd),
						Size = GetWindowSize(hWnd)
					});
				}
				return true;
			}, IntPtr.Zero);
			return windows;
		}
		
		public static Process GetHostProcessByWindow(IntPtr hWnd)
		{
			GetWindowThreadProcessId(hWnd, out uint processId).ThrowIfFailed();
			return Process.GetProcessById((int)processId);
		}

		public static List<IntPtr> FindAllWindowsByClassName(string className)
		{
			var handles = new List<IntPtr>();
			EnumWindows((hWnd, param) =>
			{
				var classBuffer = new StringBuilder(256);
				if (GetClassName(hWnd, classBuffer, classBuffer.Capacity) > 0)
				{
					if (classBuffer.ToString() == className)
					{
						handles.Add(hWnd);
					}
				}
				return true;
			}, IntPtr.Zero); // params不需要所以设置为IntPtr.Zero
			return handles;
		}

		public static bool IsFullScreen(IntPtr hWnd)
		{
			var rect = new RECT();
			GetWindowRect(hWnd, ref rect);

			var size = rect.Size;
			var screenSize = new Size(SystemParameters.PrimaryScreenWidth, SystemParameters.PrimaryScreenHeight);
			
			return Math.Approximately(size.Width, screenSize.Width) && Math.Approximately(size.Height, screenSize.Height);
		}
	}
	
	public static void ThrowIfFailed(this int hResult, string message = "An error occurred while calling a Win32 API function.")
	{
		if (hResult == 0)
		{
			throw new Win32Exception(Marshal.GetLastWin32Error(), message);
		}
	}
	
	public static void ThrowIfFailed(this uint hResult, string message = "An error occurred while calling a Win32 API function.")
	{
		((int)hResult).ThrowIfFailed(message);
	}
}
using System.Diagnostics;
using System.Windows.Input;

namespace KurobaChan.Utility;

internal class HookTriggeredEventArgs : EventArgs
{
	public Key Key { get; init; }
	public IntPtr WindowHandle { get; init; }
	public Process FromProcess { get; init; } = null!;
	
	public NativeMethods.User32.KeyBoardLowLevelHookStruct KeyEventResult { get; init; }
}
using System.Diagnostics;
using KurobaChan.Enumerator;
using KurobaChan.Utility;

namespace KurobaChan.Data;

public class KrbSoftwareInstance
{
	public KrbSoftwareInfo Metadata { get; init; } = null!;
	public IntPtr WindowHandle { get; init; }
	public long WindowHandleNumeric => WindowHandle.ToInt64();
	public Process ProcInstance { get; init; } = null!;
	
	public WindowStatus Status { get; set; } = WindowStatus.ShowingNormal;
}
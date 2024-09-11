using System.Windows.Input;
using MemoryPack;

namespace KurobaChan.Data;

[MemoryPackable]
public partial class KrbHotkeyConfig
{
	/// <summary>
	/// 隐藏当前焦点窗口。
	/// </summary>
	[MemoryPackOrder(0)]
	public Key[] HideWindowHotkey { get; set; } = [Key.Escape];
	
	/// <summary>
	/// 隐藏所有标记为NSFW(Not safe for work)的窗口，即使对应窗口不在前台也是如此。
	/// </summary>
	[MemoryPackOrder(1)]
	public Key[] HideAllNSFWWindowHotKey { get; set; } = [Key.LeftCtrl, Key.Escape];
	
	/// <summary>
	/// 显示所有被KurobaChan隐藏的窗口(包括NSFW窗口)。
	/// </summary>
	[MemoryPackOrder(2)]
	public Key[] ShowWindowHotKey { get; set; } = [Key.LeftCtrl, Key.LeftShift, Key.Escape];
	
	/// <summary>
	/// 激活KurobaKey(くろば・ケイ)。该组合键可一键实现KurobaChan所支持的预设置的操作(可在设置中设定该键的功能)。
	/// </summary>
	[MemoryPackOrder(3)]
	public Key[] KurobaKey { get; set; } = [Key.LeftCtrl, Key.LeftShift, Key.LeftAlt, Key.K];
	
	/// <summary>
	/// 显示KurobaChan主窗口。
	/// </summary>
	[MemoryPackOrder(4)]
	public Key[] ShowKurobaChanMainWindow { get; set; } = [Key.LeftCtrl, Key.LeftShift, Key.LeftAlt, Key.Escape];
	
	/// <summary>
	/// 隐藏窗口时，阻止将热键事件传递给目标窗口。<br />
	/// 适用于隐藏窗口的热键是常用热键（如Esc）的情况。<br />
	/// 该特性可能会与某些游戏的反作弊机制冲突。如遇问题请尝试禁用该特性。
	/// </summary>
	[MemoryPackOrder(5)]
	public bool BlockHotKeyWhenHidingWindow { get; set; } = true;
	
	/// <summary>
	/// 启用KurobaKey。若该值为 <see langword="false" /> 则按 <see cref="KurobaKey" /> 所设置的组合键将不会有任何效果。
	/// </summary>
	[MemoryPackOrder(6)]
	public bool EnableKurobaKey { get; set; } = false;
}
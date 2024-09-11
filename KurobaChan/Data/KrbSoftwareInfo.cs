using MemoryPack;

namespace KurobaChan.Data;

/// <summary>
/// Represents a KurobaChan-imported software (games/video players/etc.) information.
/// </summary>
[MemoryPackable]
public partial class KrbSoftwareInfo
{
	[MemoryPackOrder(0)]
	public Guid Id { get; set; }

	[MemoryPackOrder(1)]
	public string Name { get; set; } = string.Empty;

	[MemoryPackOrder(2)]
	public string ProcessImageName { get; set; } = string.Empty;

	[MemoryPackOrder(3)]
	public string ProcessPath { get; set; } = string.Empty;
	
	[MemoryPackOrder(4)]
	public string MainWindowClassName { get; set; } = string.Empty; // to identify the main window of the software
}
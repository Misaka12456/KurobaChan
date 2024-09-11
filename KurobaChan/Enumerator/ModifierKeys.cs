namespace KurobaChan.Enumerator;

[Flags] // allow to use | to combine multiple values
public enum ModifierKeys
{
	None = 0x0000,
	Alt = 0x0001,
	Control = 0x0002,
	Shift = 0x0004,
	Win = 0x0008,
}
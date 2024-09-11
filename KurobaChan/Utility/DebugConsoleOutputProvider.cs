using System.Diagnostics;
using System.IO;

namespace KurobaChan.Utility;

public class DebugConsoleOutputProvider : TextWriter
{
	public DebugConsoleOutputProvider()
	{
		Debug.WriteLine($"Enabled redirecting console output to {nameof(DebugConsoleOutputProvider)}.", nameof(DebugConsoleOutputProvider));
	}
	
	public override void Write(char value)
	{
		Debug.Write(value);
	}

	public override void Write(string? value)
	{
		Debug.Write(value);
	}
	
	public override void WriteLine(string? value)
	{
		Debug.WriteLine(value);
	}
	
	public override void WriteLine()
	{
		Debug.WriteLine(string.Empty);
	}
	
	public override System.Text.Encoding Encoding => System.Text.Encoding.UTF8;
}
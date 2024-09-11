using System.ComponentModel;
using System.Runtime.CompilerServices;
using KurobaChan.Data;

namespace KurobaChan.Models;

public class SoftwareAddDataModel : INotifyPropertyChanged
{
	private string name, processImageName, processPath, mainWindowClassName;
	private bool enableLESupport;
	
	public string Name
	{
		get => name;
		set
		{
			name = value;
			OnPropertyChanged(nameof(Name));
		}
	}
	
	public string ProcImageName
	{
		get => processImageName;
		set
		{
			processImageName = value;
			OnPropertyChanged(nameof(ProcImageName));
		}
	}
	
	public string ProcPath
	{
		get => processPath;
		set
		{
			processPath = value;
			OnPropertyChanged(nameof(ProcPath));
		}
	}
	
	public string MainWindowClassName
	{
		get => mainWindowClassName;
		set
		{
			mainWindowClassName = value;
			OnPropertyChanged(nameof(MainWindowClassName));
		}
	}

	public bool EnableLESupport
	{
		get => enableLESupport;
		set
		{
			enableLESupport = value;
			OnPropertyChanged(nameof(EnableLESupport));
		}
	}

	public event PropertyChangedEventHandler? PropertyChanged;

	public SoftwareAddDataModel()
	{
		name = processImageName = processPath = mainWindowClassName = string.Empty;
	}
	
	protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
	
	public void Reset()
	{
		name = processImageName = processPath = mainWindowClassName = string.Empty;
		OnPropertyChanged(nameof(Name));
		OnPropertyChanged(nameof(ProcImageName));
		OnPropertyChanged(nameof(ProcPath));
		OnPropertyChanged(nameof(MainWindowClassName));
	}
	
	public KrbSoftwareInfo Save()
	{
		if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(processImageName) || string.IsNullOrWhiteSpace(processPath) || string.IsNullOrWhiteSpace(mainWindowClassName))
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine($"[Warning] At least one of the fields is empty. Consider filling them before saving for better identification.");
			Console.ResetColor();
		}
		return new KrbSoftwareInfo()
		{
			Name = name,
			ProcessImageName = processImageName,
			ProcessPath = processPath,
			MainWindowClassName = mainWindowClassName
		};
	}
}
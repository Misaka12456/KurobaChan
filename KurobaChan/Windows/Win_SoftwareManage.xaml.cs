using System.ComponentModel;
using System.Windows;

namespace KurobaChan.Windows;

public partial class Win_SoftwareManage : Window
{
	public static Win_SoftwareManage? Instance { get; private set; }
	
	public Win_SoftwareManage()
	{
		Instance = this;
		InitializeComponent();
	}

	private void Win_SoftwareManage_OnClosing(object? sender, CancelEventArgs e)
	{
		Instance = null;
	}

	public void RefreshSoftwareList()
	{
		
	}
}
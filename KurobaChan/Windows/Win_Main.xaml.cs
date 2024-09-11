// ReSharper disable LocalizableElement
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.Input;
using KurobaChan.Data;
using KurobaChan.Enumerator;
using KurobaChan.Translations;
using KurobaChan.Translations.Resources;
using KurobaChan.Utility;
using KurobaChan.Utility.Extension;
using static KurobaChan.Utility.NativeMethods.User32; // import all predefined User32 Win32 API functions

namespace KurobaChan.Windows;

public partial class Win_Main : Window
{
	public static Win_Main Instance { get; private set; } = null!;
	public ObservableCollection<KrbSoftwareInstance> SoftwareInstanceList { get; } = [];
	
	public ICommand HotKeyRefreshCommand { get; init; }
	
	private GlobalLowLevelKeyHooker? mainHooker = null;
	// private ManagementEventWatcher? processWatcher = null;
	// private ManagementEventWatcher? deletionWatcher = null;
	private ProcessMonitor? processMonitor = null;

	private static bool requireQuitting = false;

	public Win_Main()
	{
		Instance = this;
		HotKeyRefreshCommand = new RelayCommand(OnHotkeyRefreshRequested);
		InitializeComponent();
		Title = $"KurobaChan {Assembly.GetExecutingAssembly().GetName().Version:3}";
		RefreshSoftwareList();
		InitProcessMonitor();
		InitHooker();
	}

	public void RequestSoftwareListRefresh(object? sender, KrbSoftwareInfo newSoftware)
	{
		newSoftware.Id = Guid.NewGuid();
		Profile.Instance.SoftwareList.Add(newSoftware);
		Profile.Save();
		RefreshSoftwareList();
	}

	private void RefreshSoftwareList()
	{
		Dispatcher.Invoke(() =>
		{
			Sbi_Status.Content = "Refreshing software list";
		});
		Win_SoftwareManage.Instance?.Dispatcher.Invoke(() =>
		{
			if (Win_SoftwareManage.Instance.Visibility == Visibility.Visible)
			{
				Win_SoftwareManage.Instance.RefreshSoftwareList();
			}
		});
		RefreshActiveSoftwareList();
		RefreshSoftwareStatus();
	}
	
	private void RefreshActiveSoftwareList()
	{
		Dispatcher.Invoke(() =>
		{
			Sbi_Status.Content = "Refreshing active software list";
		});
		var tempList = new List<KrbSoftwareInstance>();
		foreach (var software in Profile.Instance.SoftwareList)
		{
			var procList = Process.GetProcessesByName(software.ProcessImageName.Replace(".exe", string.Empty));
			if (procList.Length == 0) continue;
			var procWindows = GetVisibleWindows(software.ProcessImageName).ToList();
			if (procWindows.Count == 0) continue;
			var targetWindow = procWindows.FirstOrDefault(w => w.ClassName == software.MainWindowClassName);
			if (targetWindow == null) continue;

			var proc = targetWindow.HostProcess;
			var inst = new KrbSoftwareInstance
			{
				Metadata = software,
				ProcInstance = proc,
				WindowHandle = targetWindow.Handle,
				Status = IsFullScreen(targetWindow.Handle) ? WindowStatus.ShowingFullScreen : WindowStatus.ShowingNormal
			};
			tempList.Add(inst);
		}
		foreach (var activeInst in SoftwareInstanceList)
		{
			if (activeInst.Status is WindowStatus.HiddenFromNormal or WindowStatus.HiddenFromFullScreen && !activeInst.ProcInstance.HasExited)
			{
				tempList.Add(activeInst); // 隐藏的窗口是无法通过GetVisibleWindows获取的，所以需要保留，除非这个进程不在了
			}
		}
		// 一次性批量更新，防止用户看到不完整的列表
		SoftwareInstanceList.Clear();
		tempList.ForEach(inst => SoftwareInstanceList.Add(inst));
		Dispatcher.Invoke(() =>
		{
			Sbi_Status.Content = "Ready";
		});
	}

	private void RefreshSoftwareStatus()
	{
		Dispatcher.Invoke(() =>
		{
			Sbi_ProgramStatus.Content = $"Program(s): {SoftwareInstanceList.Count} Running, {Profile.Instance.SoftwareList.Count} Total";
		});
	}

	private int InitHooker()
	{
		mainHooker?.Dispose();
		var (hotkeys, modifiers) = Profile.Instance.HotkeyConfig.HideWindowHotkey.ExtractHotKey();
		mainHooker = new GlobalLowLevelKeyHooker(hotkeys, modifiers);
		mainHooker.OnHookTriggered += MainHooker_OnHookTriggered;
		return mainHooker.HookId;
	}

	private void InitProcessMonitor()
	{
		processMonitor = new ProcessMonitor(Profile.Instance.SoftwareList.Select(s => new MonitorProcessInfo()
		{
			ProcName = s.ProcessImageName,
			MainWindowClassName = s.MainWindowClassName
		}));
		processMonitor.ProcessStarted += ProcessMonitor_ProcessStarted;
		processMonitor.ProcessExited += ProcessMonitor_ProcessExited;
		processMonitor.Start();
	}
	
	private void ProcessMonitor_ProcessStarted(object? sender, ProcessStartedEventArgs args)
	{
		Dispatcher.Invoke(() =>
		{
			if (SoftwareInstanceList.All(sInst => sInst.ProcInstance.Id != args.Proc.Id)
			    && Profile.Instance.SoftwareList.Any(s => s.ProcessImageName.Equals($"{args.Proc.ProcessName}.exe", StringComparison.OrdinalIgnoreCase)))
			{
				SoftwareInstanceList.Add(new KrbSoftwareInstance()
				{
					Metadata = Profile.Instance.SoftwareList.First(s => s.ProcessImageName.Equals($"{args.Proc.ProcessName}.exe", StringComparison.OrdinalIgnoreCase)),
					ProcInstance = args.Proc
				});
			}
			RefreshSoftwareStatus();
		});
	}

	private void ProcessMonitor_ProcessExited(object? sender, ProcessExitedEventArgs args)
	{
		Dispatcher.Invoke(() =>
		{
			if (SoftwareInstanceList.Any(sInst => sInst.ProcInstance.Id == args.ProcessId))
			{
				SoftwareInstanceList.Remove(SoftwareInstanceList.First(sInst => sInst.ProcInstance.Id == args.ProcessId));
			}
			RefreshSoftwareStatus();
		});
	}

	private bool MainHooker_OnHookTriggered(object? sender, HookTriggeredEventArgs args)
	{
		var targetInst = SoftwareInstanceList.FirstOrDefault(sInst => sInst.ProcInstance.Id == args.FromProcess.Id);
		if (targetInst == null)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine($"[Warning] There is no software instance found for process {args.FromProcess.ProcessName} (which may not a recognized software). Ignoring the hide window request.");
			Console.ResetColor();
			return false; // ignore key event
		}
		if (IsWindowVisible(args.WindowHandle))
		{
			var status = WindowStatus.HiddenFromNormal;
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.WriteLine($"[Debug/Trace] Hiding window {args.WindowHandle} of software {targetInst.Metadata.Name} ({args.FromProcess.Id})...\n" +
			                  "To resume the hidden window, please double-click the program instance in list or right-click the instance and select 'Show/Hide Window'.");
			if (IsFullScreen(args.WindowHandle))
			{
				status = WindowStatus.HiddenFromFullScreen;
				ShowWindow(args.WindowHandle, SW_MINIMIZE);
				Thread.Sleep(100); // wait for the window to minimize
			}
			ShowWindow(args.WindowHandle, SW_HIDE);

			if (targetInst.Status != status)
			{
				int index = SoftwareInstanceList.IndexOf(targetInst);
				SoftwareInstanceList[index] = new KrbSoftwareInstance()
				{
					Metadata = targetInst.Metadata,
					ProcInstance = targetInst.ProcInstance,
					WindowHandle = targetInst.WindowHandle,
					Status = status
				};
			}
			return Profile.Instance.HotkeyConfig.BlockHotKeyWhenHidingWindow; // block key event if needed
		}
		else
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine($"[Warning] There is a hide window request for window {args.WindowHandle} of software {targetInst.Metadata.Name} ({args.FromProcess.Id}), but the window is already hidden.\n" +
							  "This may indicates that user pressed the hotkey again just after hiding window (whose window is still focusing).\n" +
							  "Ignoring the hide window request.\n" +
							  "To resume the hidden window, please double-click the program instance in list or right-click the instance and select 'Show/Hide Window'.");
			Console.ResetColor();
			return false; // ignore key event
		}
	}

	private void DataGrid_SoftwareInstance_DoubleClick(object? sender, MouseButtonEventArgs args)
	{
		if (args.ChangedButton == MouseButton.Left)
		{
			// 非常简单 —— SelectItem直接就是DataContext
			if (Dgrd_RunningPrograms.SelectedItem is not KrbSoftwareInstance inst)
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("[Warning] Failed to get selected software instance from DataGrid. Ignoring the double-click event.");
				Console.ResetColor();
				return;
			}
			// MessageBox.Show($"Selected: {inst.Metadata}");
			
			ShowWindow(inst.WindowHandle, SW_RESTORE);
			// ShowWindow(inst.WindowHandle, SW_SHOW);
			SetForegroundWindow(inst.WindowHandle);

			if (inst.Status is not (WindowStatus.ShowingNormal or WindowStatus.ShowingFullScreen))
			{
				int index = SoftwareInstanceList.IndexOf(inst);
				var status = IsFullScreen(inst.WindowHandle) ? WindowStatus.ShowingFullScreen : WindowStatus.ShowingNormal;
				SoftwareInstanceList[index] = new KrbSoftwareInstance()
				{
					Metadata = inst.Metadata,
					ProcInstance = inst.ProcInstance,
					WindowHandle = inst.WindowHandle,
					Status = status
				};
			}
		}
	}

	private void Win_Main_OnClosing(object? sender, CancelEventArgs args)
	{
		if (requireQuitting)
		{
			args.Cancel = false;
			requireQuitting = false;
			return;
		}
		args.Cancel = true;
		var ask = MessageBox.Show(UIMsgs.Globals_Quit, "KurobaChan", MessageBoxButton.YesNo, MessageBoxImage.Question);
		if (ask != MessageBoxResult.Yes) return;
		requireQuitting = true;
		mainHooker?.Dispose();
		processMonitor?.Stop();
		processMonitor?.Dispose();
		RestoreAllHiddenWindows();
		Application.Current.Shutdown();
	}

	private void AddProgram_OnClick(object sender, RoutedEventArgs args)
	{
		if (Win_SoftwareAdd.Instance != null && Win_SoftwareAdd.Instance.Visibility == Visibility.Visible)
		{
			Win_SoftwareAdd.Instance.Activate();
			return;
		}
		var softAdd = new Win_SoftwareAdd();
		softAdd.Show();
	}

	private void OnHotkeyRefreshRequested()
	{
		RefreshActiveSoftwareList();
		RefreshSoftwareStatus();
	}

	private void RestoreAllHiddenWindows()
	{
		var hiddenInstances = SoftwareInstanceList.Where(sInst => sInst.Status is WindowStatus.HiddenFromNormal or WindowStatus.HiddenFromFullScreen);
		foreach (var inst in hiddenInstances)
		{
			ShowWindow(inst.WindowHandle, SW_RESTORE);
		}
	}
}
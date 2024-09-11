using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using KurobaChan.Models;
using KurobaChan.Translations;
using KurobaChan.Utility;
using KurobaChan.Utility.Extension;
using KurobaChan.Windows;

// ReSharper disable LocalizableElement

namespace KurobaChan.Pages.SoftwareAdd;


public partial class Pge_3WindowLocate : Page, IWizardPage
{
	public Button ParentWizBtnPrevious { get; set; }
	public Button ParentWizBtnNext { get; set; }
	
	private SoftwareAddDataModel model => (SoftwareAddDataModel)DataContext;
	
	public Pge_3WindowLocate(SoftwareAddDataModel model, Button btnPrev, Button btnNext)
	{
		InitializeComponent();
		DataContext = model;
		ParentWizBtnPrevious = btnPrev;
		ParentWizBtnNext = btnNext;
		Tbk_Content.Text = UIMsgs.Pge_3WindowLocate_Content;
		Tbk_ClassName_Title.Text = UIMsgs.Pge_3WindowLocate_MainWindowClassName;
		UpdateOpButtonStatus();
	}

	private void UpdateOpButtonStatus()
	{
		ParentWizBtnNext.IsEnabled = !string.IsNullOrWhiteSpace(Tbx_ClassName.Text);
	}

	private void Btn_Refresh_OnClick(object? sender, RoutedEventArgs e)
	{
		Btn_Refresh.IsEnabled = false;
		Lbx_WindowList.IsEnabled = false;
		string procImageName = model.ProcImageName;
		bool leSupportEnabled = model.EnableLESupport;
		Task.Run(() => UpdateWindowListAsync(procImageName, leSupportEnabled));
	}

	private async Task UpdateWindowListAsync(string procImageName, bool leSupportEnabled)
	{
		try
		{
			var windowList = NativeMethods.User32.GetVisibleWindows(procImageName).ToList();
			if (leSupportEnabled)
			{
				// Locale Emulator的进程名是LEProc.exe，并且所有通过LE启动的程序的父进程都是LEProc.exe (可通过Process Explorer查看)，因此这里也需要添加上Locale Emulator的窗口
				windowList.AddRange(NativeMethods.User32.GetVisibleWindows("LEProc.exe"));
			}
			await Dispatcher.InvokeAsync(() =>
			{
				Lbx_WindowList.Items.Clear();
				// 根据窗口尺寸倒序排列，其中尺寸最大的那个窗口最可能是主窗口(加粗标记)
				bool isBiggest = true;
				foreach (var window in windowList.OrderByDescending(w => w.Size.X * w.Size.Y))
				{
					var item = new ListBoxItem()
					{
						Content = $"{window.Title} [0x{(int)window.Handle:X8}] [{window.HostProcess.Id}]",
						Tag = window // 用Tag保存窗口信息的struct
					};
					if (isBiggest)
					{
						item.FontWeight = FontWeights.Bold;
						isBiggest = false;
					}

					Lbx_WindowList.Items.Add(item);
				}

				Lbx_WindowList.IsEnabled = true;
			});
		}
		catch (Exception ex)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine($"[Warning] An unexpected error occurred during fetching window list: {ex}");
			Console.WriteLine($"[Warning] This may cause the window list to be incomplete.");
			Console.Clear();
		}
		finally
		{
			await Dispatcher.InvokeAsync(() =>
			{
				Btn_Refresh.IsEnabled = true;
			});
		}
	}

	private void Lbx_WindowList_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
	{
		if (Lbx_WindowList.Items.Count == 0) return;
		var item = (ListBoxItem)Lbx_WindowList.SelectedItem;
		var window = (NativeMethods.User32.WindowInfo)item.Tag;
		Task.Run(async () =>
		{
			// 将指定窗口置于前台提示用户，然后2s后自动切回本窗口
			window.Flash();
			await Task.Delay(2000);
			await Dispatcher.InvokeAsync(() =>
			{
				Tbx_ClassName.Text = window.ClassName;
				model.MainWindowClassName = window.ClassName;
				model.Name = window.Title;
				NativeMethods.User32.SetForegroundWindow(this.GetParentWindowHandle());
				UpdateOpButtonStatus();
			});
		});
	}
}
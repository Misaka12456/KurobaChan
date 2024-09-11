// ReSharper disable LocalizableElement
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using KurobaChan.Models;
using KurobaChan.Translations;
using KurobaChan.Utility;
using KurobaChan.Utility.Extension;
using KurobaChan.Windows;
namespace KurobaChan.Pages.SoftwareAdd;

public partial class Pge_4WindowHookTest : Page, IWizardPage
{
	public Button ParentWizBtnPrevious { get; set; }
	public Button ParentWizBtnNext { get; set; }
	private SoftwareAddDataModel model => (SoftwareAddDataModel)DataContext;
	
	private GlobalLowLevelKeyHooker testHooker = null!;
	
	private bool triggered = false;

	private readonly string procImageName;
	private readonly string mainWindowClassName;
	
	public Pge_4WindowHookTest(SoftwareAddDataModel model, Button btnPrev, Button btnNext)
	{
		InitializeComponent();
		DataContext = model;
		ParentWizBtnPrevious = btnPrev;
		ParentWizBtnNext = btnNext;
		ParentWizBtnNext.IsEnabled = !string.IsNullOrEmpty(model.ProcImageName) && !string.IsNullOrEmpty(model.MainWindowClassName)
		                                                                        && !string.IsNullOrEmpty(Tbk_HookInfo.Text);
		Tbk_Content.Text = UIMsgs.Pge_4WindowHookTest_Content;
		Btn_TestHook.Content = UIMsgs.Pge_4WindowHookTest_Button;

		procImageName = model.ProcImageName.Replace(".exe", string.Empty);
		mainWindowClassName = model.MainWindowClassName;
	}

	private void Btn_TestHook_OnClick(object? sender, RoutedEventArgs e)
	{
		testHooker = new GlobalLowLevelKeyHooker(Key.K);
		testHooker.OnHookTriggered += OnTestHookerHookTriggered;
		Btn_TestHook.IsEnabled = false;
	}

	private bool OnTestHookerHookTriggered(object? sender, HookTriggeredEventArgs args)
	{
		if (triggered) return false; // 直接传递按键事件
		if (args.FromProcess.ProcessName == procImageName && NativeMethods.User32.GetWindowClassName(args.WindowHandle) == mainWindowClassName)
		{
			triggered = true;
			string title = NativeMethods.User32.GetWindowTitle(args.WindowHandle);
			string keyInfoStr = KeyInterop.KeyFromVirtualKey(args.KeyEventResult.VKCode).ToString();
			Dispatcher.Invoke(() =>
			{
				if (Win_SoftwareAdd.Instance != null)
				{
					Win_SoftwareAdd.Instance.Btn_Cancel.IsEnabled = false;
				}
			});
						
			#region Delay Show Info & Dispose
			Task.Run(async () =>
			{
				try
				{
					await Task.Delay(1000);
					// 确保UI更新在主线程中进行
					Dispatcher.Invoke(() =>
					{
						Tbk_HookInfo.Text = "Hook trigger test pass.\n" + $"Target Window Name: {title}\n" + $"Key pressed: {keyInfoStr} ({args.KeyEventResult.ToString()})";
						Tbk_HookInfo.Foreground = Brushes.Green;
						ParentWizBtnNext.IsEnabled = !string.IsNullOrEmpty(model.ProcImageName) && !string.IsNullOrEmpty(model.MainWindowClassName) && !string.IsNullOrEmpty(Tbk_HookInfo.Text);
						NativeMethods.User32.SetForegroundWindow(this.GetParentWindowHandle());
					});
					testHooker.Dispose();
					testHooker = null!;
					Dispatcher.Invoke(() =>
					{
						if (Win_SoftwareAdd.Instance != null)
						{
							Win_SoftwareAdd.Instance.Btn_Cancel.IsEnabled = true;
						}
					});
				}
				catch (Exception ex)
				{
					// 处理异步任务中的异常
					Console.WriteLine($"An error occurred while disposing hooker: {ex.Message}");
				}
			});
			#endregion
			return true; // 阻止按键事件
		}
		return false; // 传递按键事件
	}
}
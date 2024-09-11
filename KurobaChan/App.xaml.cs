using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using KurobaChan.Data;
using KurobaChan.Translations.Resources;
using KurobaChan.Windows;

namespace KurobaChan;

public partial class App : Application
{
	private const int LatestUserAgreementVersion = 1;
	
	protected override void OnStartup(StartupEventArgs e)
	{
		base.OnStartup(e);
		
		DispatcherUnhandledException += App_DispatcherUnhandledException;

#if DEBUG
		if (MessageBox.Show("Factory reset KurobaChan profile before starting?", "KurobaChan", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
		{
			// delete the whole configuration folder
			Directory.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Misaka Castle", "KurobaChan"), true);
		}
#endif

		if (Profile.Instance.AgreedUserAgreementVersion < LatestUserAgreementVersion)
		{
			var agreement = MessageBox.Show(GlobalMsgs.FirstStart_UserAgreement, "KurobaChan", MessageBoxButton.YesNo, MessageBoxImage.Information);
			if (agreement != MessageBoxResult.Yes) return;
			Profile.Instance.AgreedUserAgreementVersion = LatestUserAgreementVersion;
			Profile.Save();
		}

		// var mainWindow = new Win_SoftwareAdd();
		var mainWindow = new Win_Main();
		mainWindow.Show();
	}

	private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
	{
		Debug.WriteLine($"Unhandled Exception: {e.Exception.Message}");

		// 可以选择终止应用程序
		e.Handled = true;
		Shutdown();
	}
}
using System.IO;
using System.Windows;
using KurobaChan.Translations.Resources;
using KurobaChan.Utility;
using KurobaChan.Utility.Extension;
using KurobaChan.Windows;

namespace KurobaChan;

public static class Program
{
	[STAThread]
	public static int Main(string[] args)
	{
		#region Abnormal Startup Type Detection (Help --? or Just Show Existing Window --show)
		if (args.Contains("--?", InvariantStringComparer.Default) || args.Contains("--help", InvariantStringComparer.Default))
		{
			MessageBox.Show(GlobalMsgs.CmdArgUsage, GlobalMsgs.CmdArgUsage_Title, MessageBoxButton.OK, MessageBoxImage.Information);
			return 0;
		}
		if (args.Contains("--show", InvariantStringComparer.Default))
		{
			if (!Permissions.IsAdministrator())
			{
				MessageBox.Show(GlobalMsgs.StartError_Permission_UAC, "KurobaChan", MessageBoxButton.OK, MessageBoxImage.Error);
				return 1;
			}
			// logic to show current existing window (by named pipe)
			return 0;
		}
		#endregion

		#region Normal Startup Preparation (app) & Unknown Argument Detection
		var app = new App()
		{
			ShutdownMode = ShutdownMode.OnExplicitShutdown,
			Properties =
			{
				["Mode"] = "User"
			}
		};
		if (args.Contains("--service", InvariantStringComparer.Default))
		{
			app.Properties["Mode"] = "Service";
		}
		else if (args.Length > 0)
		{
			string argConcat = string.Join(" ", args);
			string text = $"{string.Format(GlobalMsgs.CmdArgUsage_UnknownArgPrefix, argConcat)}\n\n{GlobalMsgs.CmdArgUsage}";
			MessageBox.Show(text, GlobalMsgs.CmdArgUsage_Title, MessageBoxButton.OK, MessageBoxImage.Error);
			return 1;
		}
		#endregion
		
		Console.SetOut(new DebugConsoleOutputProvider()); // redirect Console.Write(Line) to Debug.Write(Line) (System.Diagnostics)
		Console.SetError(new DebugConsoleOutputProvider()); // redirect Console.Error.Write(Line) to Debug.Write(Line) (System.Diagnostics)
		
		return app.Run();
	}
}
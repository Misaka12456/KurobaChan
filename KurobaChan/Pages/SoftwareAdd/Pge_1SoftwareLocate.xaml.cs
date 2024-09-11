using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using KurobaChan.FixedData;
using KurobaChan.Models;
using KurobaChan.Translations;
using KurobaChan.Utility.Extension;
using Microsoft.Win32;

namespace KurobaChan.Pages.SoftwareAdd;

public partial class Pge_1SoftwareLocate : Page, IWizardPage
{
	public Button ParentWizBtnPrevious { get; set; }
	public Button ParentWizBtnNext { get; set; }
	
	private SoftwareAddDataModel model => (SoftwareAddDataModel)DataContext;
	
	public Pge_1SoftwareLocate(SoftwareAddDataModel model, Button btnPrev, Button btnNext)
	{
		InitializeComponent();
		DataContext = model;
		ParentWizBtnPrevious = btnPrev;
		ParentWizBtnNext = btnNext;
		Tbk_Content.Text = UIMsgs.Pge_1SoftwareLocate_Content;
		Btn_Browse.Content = UIMsgs.Pge_1SoftwareLocate_Browse;
		Initialize();
	}

	private void Initialize()
	{
		Tbx_SoftwarePath.Text = model.ProcPath;
		ParentWizBtnNext.IsEnabled = !string.IsNullOrEmpty(Tbx_SoftwarePath.Text) && File.Exists(Tbx_SoftwarePath.Text);
	}

	private void Btn_Browse_OnClick(object sender, RoutedEventArgs e)
	{
		var dialog = new OpenFileDialog()
		{
			Filter = "Executable Files (*.exe)|*.exe",
			Title = "Select Software Executable",
			CheckFileExists = true,
			CheckPathExists = true
		};
		if (dialog.ShowDialog() != true) // 不能直接用if (!dialog.ShowDialog())，因为其返回的是bool?类型
		{
			return;
		}
		Tbx_SoftwarePath.Text = dialog.FileName;
		if (LEExecutableCheck(dialog.FileName, out string leInfo))
		{
			Tbk_LEDetected.Text = leInfo;
			Tbk_LEDetected.Visibility = Visibility.Visible;
		}
		else
		{
			Tbk_LEDetected.Text = string.Empty;
			Tbk_LEDetected.Visibility = Visibility.Collapsed;
		}
		model.ProcPath = dialog.FileName;
		model.ProcImageName = Path.GetFileName(dialog.FileName);
		ParentWizBtnNext.IsEnabled = !string.IsNullOrEmpty(Tbx_SoftwarePath.Text) && File.Exists(Tbx_SoftwarePath.Text);
	}

	private static bool LEExecutableCheck(string path, out string leInfo)
	{
		string exeName = Path.GetFileName(path);
		if (FixedSoftwareList.LERequiredList.TryGetValue(exeName, out var leRequiredSoftware)
		    && (int)leRequiredSoftware.RequiredCodePage != Encoding.Default.CodePage)
		{
			// leInfo = "Detected program with different code page from current system, which may require Locale Emulator to start:\n" +
			//          "Program Name: {softwareName}\n" +
			//          "Original Code Page: {codePage}\n" +
			//          "If the above information is incorrect, please submit an issue or a pull request on GitHub. " +
			//          "Your action will help to improve user experience of KurobaChan.";
			leInfo = UIMsgs.Pge_1SoftwareLocate_LENotice;
			leInfo = string.Format(leInfo, leRequiredSoftware.SoftwareName, leRequiredSoftware.RequiredCodePageText);
			return true;
		}
		else
		{
			leInfo = string.Empty;
			return false;
		}
	}
}
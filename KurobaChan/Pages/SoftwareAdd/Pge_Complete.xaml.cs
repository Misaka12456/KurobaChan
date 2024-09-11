using System.Windows.Controls;
using KurobaChan.Models;
using KurobaChan.Translations;
using KurobaChan.Utility.Extension;

namespace KurobaChan.Pages.SoftwareAdd;

public partial class Pge_Complete : Page, IWizardPage
{
	public Button ParentWizBtnPrevious { get; set; }
	public Button ParentWizBtnNext { get; set; }
	
	public Pge_Complete(SoftwareAddDataModel model, Button btnPrev, Button btnNext)
	{
		InitializeComponent();
		DataContext = model;
		ParentWizBtnPrevious = btnPrev;
		ParentWizBtnNext = btnNext;
		Tbk_Content.Text = string.Format(UIMsgs.Pge_Complete_Content, model.Name, model.ProcPath,
			model.MainWindowClassName, model.EnableLESupport ? "Enabled" : "Disabled");
		ParentWizBtnPrevious.IsEnabled = false;
	}
}
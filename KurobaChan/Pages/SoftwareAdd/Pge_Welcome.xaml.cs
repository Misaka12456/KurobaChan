using System.Windows.Controls;
using KurobaChan.Models;
using KurobaChan.Translations;
using KurobaChan.Utility.Extension;

namespace KurobaChan.Pages.SoftwareAdd;

public partial class Pge_Welcome : Page, IWizardPage
{
	public Button ParentWizBtnPrevious { get; set; }
	public Button ParentWizBtnNext { get; set; }
	
	public Pge_Welcome(SoftwareAddDataModel model, Button btnPrev, Button btnNext)
	{
		InitializeComponent();
		DataContext = model;
		ParentWizBtnPrevious = btnPrev;
		ParentWizBtnNext = btnNext;
		Tbk_Content.Text = UIMsgs.Pge_Welcome_Content;
	}
}
using System.Windows;
using System.Windows.Controls;
using KurobaChan.Models;
using KurobaChan.Translations;
using KurobaChan.Utility.Extension;

namespace KurobaChan.Pages.SoftwareAdd;

public partial class Pge_2LESupport : Page, IWizardPage
{
	public Button ParentWizBtnPrevious { get; set; }
	public Button ParentWizBtnNext { get; set; }

	private SoftwareAddDataModel model => (SoftwareAddDataModel)DataContext;
	
	public Pge_2LESupport(SoftwareAddDataModel model, Button btnPrev, Button btnNext)
	{
		InitializeComponent();
		DataContext = model;
		ParentWizBtnPrevious = btnPrev;
		ParentWizBtnNext = btnNext;
		Tbk_Content.Text = UIMsgs.Pge_2LESupport_Content;
		Chk_LESupport.Content = UIMsgs.Pge_2LESupport_CheckBox;
	}

	private void Chk_LESupport_OnChecked(object sender, RoutedEventArgs e)
	{
		model.EnableLESupport = true;
	}

	private void Chk_LESupport_OnUnchecked(object sender, RoutedEventArgs e)
	{
		model.EnableLESupport = false;
	}
}
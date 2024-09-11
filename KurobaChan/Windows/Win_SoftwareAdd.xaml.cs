using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using KurobaChan.Data;
using KurobaChan.Models;
using KurobaChan.Pages.SoftwareAdd;
using KurobaChan.Translations;
using KurobaChan.Translations.Resources;
using KurobaChan.Utility.Extension;

namespace KurobaChan.Windows;

public partial class Win_SoftwareAdd : Window
{
	public static Win_SoftwareAdd? Instance { get; private set; } = null!;
	
	private IWizardPage m_currentPage => m_pages[m_currentStep].Value;
	private readonly List<Lazy<IWizardPage>> m_pages;
	private readonly SoftwareAddDataModel m_model;
	private int m_currentStep;
	
	public Win_SoftwareAdd()
	{
		Instance = this;
		InitializeComponent();
		Title = UIMsgs.Win_SoftwareAdd_Title;
		m_model = new SoftwareAddDataModel();
		m_pages =
		[
			new Lazy<IWizardPage>(() => new Pge_Welcome(m_model, Btn_Prev, Btn_Next)),
			new Lazy<IWizardPage>(() => new Pge_1SoftwareLocate(m_model, Btn_Prev, Btn_Next)),
			new Lazy<IWizardPage>(() => new Pge_2LESupport(m_model, Btn_Prev, Btn_Next)),
			new Lazy<IWizardPage>(() => new Pge_3WindowLocate(m_model, Btn_Prev, Btn_Next)),
			new Lazy<IWizardPage>(() => new Pge_4WindowHookTest(m_model, Btn_Prev, Btn_Next)),
			new Lazy<IWizardPage>(() => new Pge_Complete(m_model, Btn_Prev, Btn_Next))
		];
		
		m_currentStep = 0;
		UpdateButtons();
		Frme_Content.Navigate(m_currentPage);
	}

	private void PreviousStep_Click(object sender, RoutedEventArgs args)
	{
		if (m_currentStep == 0) return;
		if (!m_currentPage.PrePrevious())
		{
			return; // the page doesn't allow to go back
		}
		m_currentStep--;
		UpdateButtons();
		Frme_Content.Navigate(m_currentPage);
	}

	private void NextStep_Click(object sender, RoutedEventArgs args)
	{
		if (m_currentStep == m_pages.Count - 1) return;
		if (!m_currentPage.PreNext())
		{
			return; // the page doesn't allow to go next
		}
		m_currentStep++;
		UpdateButtons();
		Frme_Content.Navigate(m_currentPage);
	}

	private void CancelOrFinish_Click(object sender, RoutedEventArgs args)
	{
		if (m_currentStep != m_pages.Count - 1)
		{
			var r = MessageBox.Show(UIMsgs.Win_SoftwareAdd_Cancel, UIMsgs.Globals_Warning, MessageBoxButton.YesNo, MessageBoxImage.Warning);
			if (r == MessageBoxResult.No) return;
			Close();
		}
		else
		{
			var info = m_model.Save();
			Win_Main.Instance.RequestSoftwareListRefresh(this, info);
			Close();
		}
	}
	
	private void UpdateButtons()
	{
		Btn_Prev.IsEnabled = m_currentStep > 0;
		Btn_Next.IsEnabled = m_currentStep < m_pages.Count - 1;
		Btn_Cancel.Content = m_currentStep == m_pages.Count - 1 ? "Finish" : "Cancel";
	}

	private void Win_SoftwareAdd_OnClosing(object? sender, CancelEventArgs args)
	{
		Instance = null;
	}
}
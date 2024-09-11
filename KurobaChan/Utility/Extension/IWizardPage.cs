using System.Windows.Controls;

namespace KurobaChan.Utility.Extension;

public interface IWizardPage
{
	public Button ParentWizBtnPrevious { get; set; }
	public Button ParentWizBtnNext { get; set; }

	public bool PrePrevious() => true;
	public bool PreNext() => true;
	
	public Page AsPage() => this as Page ?? throw new InvalidOperationException("The current IWizardPage is not a System.Windows.Controls.Page");
}
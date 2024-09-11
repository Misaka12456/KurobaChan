using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace KurobaChan.Utility.Extension;

public static class WPFWindowPageExtension
{
	public static IntPtr GetParentWindowHandle(this DependencyObject obj)
	{
		while (obj is not Window)
		{
			obj = VisualTreeHelper.GetParent(obj);
		}

		if (obj is Window window)
		{
			return new WindowInteropHelper(window).Handle;
		}
		
		return IntPtr.Zero;
	}
}
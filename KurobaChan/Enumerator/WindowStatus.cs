using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using KurobaChan.Utility.Extension;

namespace KurobaChan.Enumerator;

public enum WindowStatus
{
	[Description("隐藏")] HiddenFromNormal = 0,

	[Description("隐藏 (全屏)")] HiddenFromFullScreen = 1,

	[Description("显示")] ShowingNormal = 2,

	[Description("显示 (全屏)")] ShowingFullScreen = 3
}

public class WindowStatusConverter : IValueConverter
{
	public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value is WindowStatus status)
		{
			return status.ToDescription();
		}
		return value?.ToString() ?? string.Empty;
	}

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value is string str)
		{
			return Enum.Parse<WindowStatus>(str);
		}
		return value ?? throw new ArgumentNullException(nameof(value));
	}
}
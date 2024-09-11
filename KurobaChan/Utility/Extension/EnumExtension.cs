using System.ComponentModel;
using System.Reflection;

namespace KurobaChan.Utility.Extension;

public static class EnumExtension
{
	public static IEnumerable<T> UnpackFlags<T>(this T flags) where T : Enum
	{
		foreach (T value in Enum.GetValues(typeof(T)))
		{
			if (Convert.ToInt32(value) != 0 && flags.HasFlag(value))
			{
				yield return value;
			}
		}
	}
	
	public static string ToDescription(this Enum value)
	{
		var field = value.GetType().GetField(value.ToString());
		if (field == null) return value.ToString();
		var attribute = field.GetCustomAttribute<DescriptionAttribute>();
		return attribute?.Description ?? value.ToString();
	}
}
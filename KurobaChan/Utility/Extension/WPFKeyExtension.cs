using System.Windows.Input;
using KurobaChan.Enumerator;
using ModifierKeys = KurobaChan.Enumerator.ModifierKeys;

namespace KurobaChan.Utility.Extension;

public static class WPFKeyExtension
{
	public static (Key, ModifierKeys) ExtractHotKey(this IEnumerable<Key> hotkey)
	{
		var keys = hotkey.ToList();
		var modifier = ModifierKeys.None;
		if (keys.Contains(Key.LeftCtrl) || keys.Contains(Key.RightCtrl))
		{
			modifier |= ModifierKeys.Control;
			keys.Remove(Key.LeftCtrl);
			keys.Remove(Key.RightCtrl);
		}
		if (keys.Contains(Key.LeftShift) || keys.Contains(Key.RightShift))
		{
			modifier |= ModifierKeys.Shift;
			keys.Remove(Key.LeftShift);
			keys.Remove(Key.RightShift);
		}
		if (keys.Contains(Key.LeftAlt) || keys.Contains(Key.RightAlt))
		{
			modifier |= ModifierKeys.Alt;
			keys.Remove(Key.LeftAlt);
			keys.Remove(Key.RightAlt);
		}

		if (keys.Contains(Key.LWin) || keys.Contains(Key.RWin))
		{
			modifier |= ModifierKeys.Win;
			keys.Remove(Key.LWin);
			keys.Remove(Key.RWin);
		}
		
		if (keys.Count is 0 or > 1)
		{
			throw new ArgumentException("Invalid hotkey detected with no or multiple normal keys.");
		}
		
		return (keys[0], modifier);
	}
}
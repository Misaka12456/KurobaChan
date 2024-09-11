using System.Reflection;
using System.Text;
using KurobaChan.FixedData.Data;
using Newtonsoft.Json;

namespace KurobaChan.FixedData;

public static class FixedSoftwareList
{
	/// <summary>
	/// Represents the list of software executables that are required for Locale Emulator to function properly.<br />
	/// This list is loaded from 'KurobaChan.FixedData/Resources/List/SoftwareExecutableList.json'.
	/// </summary>
	public static Dictionary<string, LERequiredSoftware> LERequiredList { get; }

	static FixedSoftwareList()
	{
		using var rs = Assembly.GetExecutingAssembly()!.GetManifestResourceStream("KurobaChan.FixedData.Resources.List.LERequiredSoftwareList.json");
		using var sr = new StreamReader(rs!, Encoding.UTF8);
		var list = JsonConvert.DeserializeObject<List<LERequiredSoftware>>(sr.ReadToEnd())!;
		LERequiredList = list.ToDictionary(s => s.ExecutableName, s => s);
	}
}
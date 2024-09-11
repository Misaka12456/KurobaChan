using System.ComponentModel;
using System.Reflection;
using KurobaChan.FixedData.Enumerators;
using Newtonsoft.Json;

namespace KurobaChan.FixedData.Data;

[Serializable]
public class LERequiredSoftware
{
	[JsonProperty("exe")]
	public string ExecutableName { get; init; } = string.Empty;
	
	[JsonProperty("name")]
	public string SoftwareName { get; init; } = string.Empty;
	
	[JsonProperty("code_page")]
	public CodePage RequiredCodePage { get; init; } = CodePage.UTF8;

	[JsonIgnore]
	public string RequiredCodePageText
	{
		get
		{
			var type = typeof(CodePage);
			var desc = type.GetField(RequiredCodePage.ToString())!.GetCustomAttribute<DescriptionAttribute>();
			string r = desc?.Description ?? RequiredCodePage.ToString();
			r += $"({(int)RequiredCodePage})";
			return r;
		}
	}
}
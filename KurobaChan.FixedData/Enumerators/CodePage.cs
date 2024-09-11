using System.ComponentModel;

namespace KurobaChan.FixedData.Enumerators;

public enum CodePage
{
	[Description("IBM EBCDIC (US-Canada)")]
	LegacyASCII = 37,		// IBM EBCDIC (US-Canada)
	
	[Description("OEM United States")]
	DefaultASCII = 437,		// OEM United States
	
	[Description("ANSI/OEM 日本語; Japanese (Shift-JIS)")]
	ShiftJIS = 932,			// ANSI/OEM Japanese; Japanese (Shift-JIS)
	
	[Description("ANSI/OEM 简体中文 (中国大陆, 新加坡); Chinese Simplified (GB2312)")]
	GB2312 = 936,			// ANSI/OEM Simplified Chinese (PRC, Singapore); Chinese Simplified (GB2312)
	
	[Description("ANSI/OEM 繁體中文 (台灣; 中国香港特別行政區); Chinese Traditional (Big5)")]
	Big5 = 950,				// ANSI/OEM Traditional Chinese (Taiwan Region; Hong Kong SAR, PRC); Chinese Traditional (Big5)
	
	[Description("Unicode (UTF-8)")]
	UTF8 = 65001			// Unicode (UTF-8)
}
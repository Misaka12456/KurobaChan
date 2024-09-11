// ReSharper disable LocalizableElement
using System.IO;
using System.Security.Cryptography;
using System.Text;
using MemoryPack; // from Cysharp/MemoryPack (https://github.com/Cysharp/MemoryPack). Licensed under The MIT License.

namespace KurobaChan.Data;

[MemoryPackable]
public partial class Profile
{
	public static Profile Instance
	{
		get => instance ??= Load();
		set
		{
			instance = value;
			Save(value);
		}
	}

	private static Profile? instance;

	private const string MagicHeader = "KUROBA";
	private const int ProfileVersion = 1;
	private readonly static byte[] AesKey = "KurobaChanDefault0123456789abcde"u8.ToArray();
	
	[MemoryPackOrder(0)]
	public int AgreedUserAgreementVersion = 0;
	
	public List<KrbSoftwareInfo> SoftwareList { get; init; } = [];
	
	public KrbHotkeyConfig HotkeyConfig { get; init; } = new();
	
	private static Profile Load()
	{
		Profile prof;
		try
		{
			string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Misaka Castle", "KurobaChan", "profile.dat");
         	if (!File.Exists(path))
         	{
         		throw new FileNotFoundException("The profile data file does not exist.", path);
         	}
         	byte[] raw = File.ReadAllBytes(path);
         	
         	// #region File Structure & Version Validation
         	// if (raw.Length < 16)
         	// {
         	// 	throw new FileFormatException("The file is too short to be a valid profile data.");
         	// }
         	// if (Encoding.ASCII.GetString(raw[..6]) != MagicHeader)
         	// {
         	// 	throw new FileFormatException("The file is not a valid KurobaChan profile data.");
         	// }
         	// using var br = new BinaryReader(new MemoryStream(raw));
         	// br.BaseStream.Seek(6, SeekOrigin.Begin);
         	// int version = br.ReadInt32();
         	// if (version > ProfileVersion)
         	// {
         	// 	throw new FileFormatException($"The profile data is newer than the program can handle. ({version} > {ProfileVersion})");
         	// }
         	// #endregion
         	//
         	// int dataLen = br.ReadInt32();
         	// byte[] iv = br.ReadBytes(16);
         	// byte[] cipherProfile = br.ReadBytes(dataLen);
         	// 	
         	// #region Decrypt Profile Data
         	// using var aes = Aes.Create();
         	// aes.Key = AesKey;
         	// aes.IV = iv;
         	// aes.Mode = CipherMode.CFB;
         	// aes.Padding = PaddingMode.PKCS7;
          //   aes.BlockSize = 128;
         	// using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
         	// using var msDecrypt = new MemoryStream(cipherProfile);
         	// using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
         	// using var brDecrypt = new BinaryReader(csDecrypt);
         	// byte[] plainProfile = brDecrypt.ReadBytes(dataLen);
         	// brDecrypt.Close();
         	// csDecrypt.Close();
         	// msDecrypt.Close();
         	// #endregion
            
            byte[] plainProfile = raw;
         		
         	prof = MemoryPackSerializer.Deserialize<Profile>(plainProfile) ?? throw new FileFormatException("Failed to deserialize the profile data.");
		}
		catch (Exception ex)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine($"[Warning] Failed to load the profile data: {ex.Message} ({ex.GetType().FullName}). A new profile will be created.");
			Console.ResetColor();
			prof = new Profile();
		}
		return prof;
	}

	private static void Save(Profile prof)
	{
		string basePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Misaka Castle", "KurobaChan");
		if (!Directory.Exists(basePath))
		{
			Directory.CreateDirectory(basePath);
		}
		string path = Path.Combine(basePath, "profile.dat");
		// var bw = new BinaryWriter(new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None));
		// bw.Write(Encoding.ASCII.GetBytes(MagicHeader));
		// bw.Write(ProfileVersion);
		//
		// byte[] plainProfile = MemoryPackSerializer.Serialize(prof);
		// using var aes = Aes.Create();
		// aes.Mode = CipherMode.CFB;
		// aes.Padding = PaddingMode.PKCS7;
		// aes.BlockSize = 128;
		// aes.Key = AesKey;
		// aes.IV = new byte[16];
		// aes.GenerateIV();
		// byte[] iv = aes.IV;
		// using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
		// using var msEncrypt = new MemoryStream();
		// using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write, leaveOpen: true);
		// csEncrypt.Write(plainProfile);
		// csEncrypt.Close();
		// msEncrypt.Seek(0, SeekOrigin.Begin);
		// byte[] cipherProfile = msEncrypt.ToArray();
		// msEncrypt.Close();
		//
		// bw.Write((long)cipherProfile.Length);
		// bw.Write(iv);
		// bw.Write(cipherProfile);
		//
		// bw.Flush();
		// long profLength = bw.BaseStream.Length;
		// bw.Close();
		
		byte[] plainProfile = MemoryPackSerializer.Serialize(prof);
		File.WriteAllBytes(path, plainProfile);
		
		Console.ForegroundColor = ConsoleColor.Green;
		Console.WriteLine($"[Info] Profile saved. Length: {plainProfile.Length} bytes.");
		Console.ResetColor();
	}
	
	public static void Save() => Save(Instance);
}
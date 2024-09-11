using System.Security.Principal;
using KurobaChan.Enumerator;

namespace KurobaChan.Utility.Extension;

public static class Permissions
{
	public static PermissionType Current
	{
		get
		{
			var identity = WindowsIdentity.GetCurrent();
			var principal = new WindowsPrincipal(identity);
			return principal switch
			{
				_ when principal.IsInRole(WindowsBuiltInRole.Administrator) => PermissionType.Administrator,
				_ when principal.IsInRole(WindowsBuiltInRole.PowerUser) => PermissionType.User,
				_ when principal.IsInRole(WindowsBuiltInRole.User) || principal.IsInRole(WindowsBuiltInRole.Guest) => PermissionType.TinyUser,
				_ => PermissionType.NoPermission
			};
		}
	}
	
	public static bool IsAdministrator() => Current >= PermissionType.Administrator;
}
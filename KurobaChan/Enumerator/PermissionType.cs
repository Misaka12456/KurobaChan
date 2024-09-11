namespace KurobaChan.Enumerator;

public enum PermissionType
{
	NoPermission,  // No Permission
	TinyUser,	   // Guest & User	(which have most limited permissions)
	User,		   // Power User	(which is the so-called "Normal User")
	Administrator  // Administrator
}
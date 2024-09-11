namespace KurobaChan.Utility.Extension;

public class InvariantStringComparer : IEqualityComparer<string>
{
	public readonly static InvariantStringComparer Default = new InvariantStringComparer();

	private InvariantStringComparer()
	{
		
	}
	
	public bool Equals(string? x, string? y)
	{
		return string.Equals(x, y, StringComparison.InvariantCulture);
	}

	public int GetHashCode(string obj)
	{
		return obj.GetHashCode();
	}
}
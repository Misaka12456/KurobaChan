namespace KurobaChan.Utility;

public static class Math
{
	// Normally doesn't recommend to directly use [float/double].Epilson, cause it is too small (which may not ignore float-point-rounding-error (usually not a problem in most cases))
	public static bool Approximately(float a, float b, float tolerance = 0.0001f)
	{
		return System.Math.Abs(a - b) < tolerance;
	}
	
	public static bool Approximately(double a, double b, double tolerance = 0.0001)
	{
		return System.Math.Abs(a - b) < tolerance;
	}
}
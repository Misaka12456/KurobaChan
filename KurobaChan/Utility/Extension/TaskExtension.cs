namespace KurobaChan.Utility.Extension;

public static class TaskExtension
{
	public static async Task WaitForConditionOrThrow(Func<bool> condition, int retries = 5, int delayPerRetry = 1000, string customError = "")
	{
		for (int i = 0; i < retries; i++)
		{
			if (condition())
			{
				return;
			}
			await Task.Delay(delayPerRetry);
		}
		
		throw new TimeoutException(!string.IsNullOrEmpty(customError) ? customError : "Operation timed out after waiting for condition to be true.");
	}
}
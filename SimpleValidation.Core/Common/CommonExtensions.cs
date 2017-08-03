namespace SimpleValidation.Core.Common
{
	public static class CommonExtensions
	{
		public static TFail[] AsArray<TFail>(this TFail validationResult)
		{
			return new[]{ validationResult };
		}
	}
}
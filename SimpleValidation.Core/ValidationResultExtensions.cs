namespace SimpleValidation.Core
{
	public static class ValidationResultExtensions
	{
		public static ValidationResult<TFail>[] AsArray<TFail>(this ValidationResult<TFail> validationResult)
		{
			return new[]{ validationResult };
		}
	}
}
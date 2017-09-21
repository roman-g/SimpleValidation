using System;

namespace SimpleValidation.Core.Common
{
	public static class SimpleValidator
	{
		public static Validator<TIn, TFail> Make<TIn, TFail>(Func<TIn, TFail> rule)
		{
			return x =>
				   {
					   var result = rule(x);
					   return result == null ? new TFail[0] : new[] { result };
				   };
		}
	}
}
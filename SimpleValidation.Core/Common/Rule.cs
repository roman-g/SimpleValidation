using System;

namespace SimpleValidation.Core.Common
{
	public static class Rule
	{
		public static Validator<TIn, TFail> Single<TIn, TFail>(Func<TIn, TFail> rule)
		{
			return x => rule(x).AsArray();
		}

		public static Validator<TIn, TFail> Multiple<TIn, TFail>(Func<TIn, TFail[]> rule)
		{
			return x => rule(x);
		}
	}
}
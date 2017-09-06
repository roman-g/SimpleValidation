using System;
using SimpleValidation.Core.Common;

namespace SimpleValidation.Core.PredicateRules
{
	public static class PredicateHelpers
	{
		public static Validator<TIn, TFail> WithPredicate<TIn, TFail>(
			this Validator<TIn, TFail> mapping,
			Func<TIn, bool> predicate)
		{
			return input => !predicate(input) ? mapping(input) : new TFail[0];
		}
	}
}
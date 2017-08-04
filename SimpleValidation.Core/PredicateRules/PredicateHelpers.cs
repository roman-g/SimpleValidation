using System;

namespace SimpleValidation.Core.PredicateRules
{
	public static class PredicateHelpers
	{
		public static Func<TIn, TFail[]> WithPredicate<TIn, TFail>(
			this Func<TIn, TFail[]> mapping,
			Func<TIn, bool> predicate)
		{
			return input => !predicate(input) ? mapping(input) : new TFail[0];
		}
	}
}
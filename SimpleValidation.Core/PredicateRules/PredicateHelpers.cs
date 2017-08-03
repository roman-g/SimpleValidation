using System;

namespace SimpleValidation.Core.PredicateRules
{
	public static class PredicateHelpers
	{
		public static Func<TIn, TOut[]> WithPredicate<TIn, TOut>(
			this Func<TIn, TOut[]> mapping,
			Func<TIn, bool> predicate)
		{
			return input => !predicate(input) ? mapping(input) : new TOut[0];
		}
	}
}
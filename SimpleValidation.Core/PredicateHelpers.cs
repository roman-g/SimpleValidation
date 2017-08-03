using System;

namespace SimpleValidation.Core
{
	public static class PredicateHelpers
	{
		public static Func<TIn, TOut[]> WrapWithPredicate<TIn, TOut>(
			Func<TIn, bool> predicate,
			Func<TIn, TOut[]> mapping)
		{
			return input => !predicate(input) ? mapping(input) : new TOut[0];
		}
	}
}
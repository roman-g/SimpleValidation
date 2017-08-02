using System;

namespace SimpleValidation.Core
{
	public static class PredicateHelpers
	{
		public static Func<TIn, ValidationResult<TOut>[]> WrapWithPredicate<TIn, TOut>(
			Func<TIn, bool> predicate,
			Func<TIn, ValidationResult<TOut>[]> mapping)
		{
			return input => !predicate(input) ? mapping(input) : ValidationResult<TOut>.Success().AsArray();
		}
	}
}
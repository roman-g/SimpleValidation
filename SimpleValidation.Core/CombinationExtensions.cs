using System;

namespace SimpleValidation.Core
{
	public static class CombinationExtensions
	{
		public static Func<TIn, ValidationResult<TOut>> Then<TIn, TOut>(this Func<TIn, ValidationResult<TOut>> first, Func<TIn, ValidationResult<TOut>> second)
		{
			return input =>
			{
				var result = first(input);
				return result.IsFail ? result : second(input);
			};
		}
	}
}
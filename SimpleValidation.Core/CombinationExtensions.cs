using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleValidation.Core
{
	public static class CombinationExtensions
	{
		public static Func<TIn, ValidationResult<TOut>[]> Then<TIn, TOut>(this Func<TIn, ValidationResult<TOut>[]> first, params Func<TIn, ValidationResult<TOut>[]>[] otherRules)
		{
			IEnumerable<ValidationResult<TOut>[]> Apply(IEnumerable<Func<TIn, ValidationResult<TOut>[]>> rules, TIn input)
			{
				foreach (var rule in rules)
				{
					var result = rule(input);
					yield return result;
					if (result.Any(x => x.IsFail))
						yield break;
				}
			}
			return input =>
			{
				var rules = new[] { first }.Concat(otherRules);
				return Apply(rules, input).SelectMany(x => x).ToArray();
			};
		}

		public static Func<TIn, ValidationResult<TOut>[]> Union<TIn, TOut>(this Func<TIn, ValidationResult<TOut>[]> rule, params Func<TIn, ValidationResult<TOut>[]>[] otherRules)
		{
			return input => new []{ rule }.Concat(otherRules).SelectMany(x => x(input)).ToArray();
		}
	}
}
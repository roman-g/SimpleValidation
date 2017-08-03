using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleValidation.Core.Combination
{
	public static class CombinationHelpers
	{
		public static Func<TIn, TOut[]> Then<TIn, TOut>(this Func<TIn, TOut[]> first,
														params Func<TIn, TOut[]>[] otherRules)
		{
			return Order(new[] {first}.Concat(otherRules).ToArray());
		}

		public static Func<TIn, TOut[]> Order<TIn, TOut>(params Func<TIn, TOut[]>[] rules)
		{
			IEnumerable<TOut[]> Apply(TIn input)
			{
				foreach (var rule in rules)
				{
					var result = rule(input);
					yield return result;
					if (result.Any())
						yield break;
				}
			}

			return input => Apply(input).SelectMany(x => x).ToArray();
		}

		public static Func<TIn, TOut[]> Union<TIn, TOut>(params Func<TIn, TOut[]>[] rules)
		{
			return input => rules.SelectMany(x => x(input)).ToArray();
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleValidation.Core.Combination
{
	public static class CombinationHelpers
	{
		public static Func<TIn, TFail[]> Then<TIn, TFail>(this Func<TIn, TFail[]> first,
														params Func<TIn, TFail[]>[] otherRules)
		{
			return Order(new[] {first}.Concat(otherRules).ToArray());
		}

		public static Func<TIn, TFail[]> Order<TIn, TFail>(params Func<TIn, TFail[]>[] rules)
		{
			IEnumerable<TFail[]> Apply(TIn input)
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

		public static Func<TIn, TFail[]> Union<TIn, TFail>(params Func<TIn, TFail[]>[] rules)
		{
			return input => rules.SelectMany(x => x(input)).ToArray();
		}
	}
}
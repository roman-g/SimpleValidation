using System;
using System.Collections.Generic;
using System.Linq;
using SimpleValidation.Core.Common;

namespace SimpleValidation.Core.Combination
{
	public static class CombinationHelpers
	{
		public static Validator<TIn, TFail> Then<TIn, TFail>(this Validator<TIn, TFail> first,
															 params Validator<TIn, TFail>[] otherRules)
		{
			return Order(new[] {first}.Concat(otherRules).ToArray());
		}

		public static Validator<TIn, TFail> Order<TIn, TFail>(params Validator<TIn, TFail>[] rules)
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

		public static Validator<TIn, TFail> Union<TIn, TFail>(params Validator<TIn, TFail>[] rules)
		{
			return input => rules.SelectMany(x => x(input)).ToArray();
		}
	}
}
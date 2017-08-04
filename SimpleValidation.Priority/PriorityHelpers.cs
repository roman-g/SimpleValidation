using System;
using System.Collections.Generic;
using System.Linq;
using SimpleValidation.Core;
using SimpleValidation.Core.Combination;
using SimpleValidation.Core.Common;

namespace SimpleValidation.Priority
{
	public static class PriorityHelpers
	{
		public static IRuleWithPriority<TIn, TFail> WithPriority<TIn, TFail>(this Func<TIn, TFail[]> rule, int priority)
		{
			return new RuleWithPriority<TIn, TFail>
				   {
					   Rule = rule,
					   Priority = priority
				   };
		}

		public static Func<TIn, TFail[]> Collapse<TIn, TFail>(this IEnumerable<IRuleWithPriority<TIn, TFail>> rules)
		{
			var orderedRules = rules.GroupBy(x => x.Priority)
									.OrderBy(x => x.Key)
									.Select(x => Union(x.ToArray()))
									.Select(x => x.Rule)
									.ToArray();

			return CombinationHelpers.Order(orderedRules);
		}

		public static IRuleWithPriority<TIn, TFail> Combine<TIn, TFail>(Func<Func<TIn, TFail[]>[], Func<TIn, TFail[]>> combinator,
			params IRuleWithPriority<TIn, TFail>[] rules)
		{
			var priorities = rules.Select(x => x.Priority).Distinct().ToArray();
			if (priorities.Length > 1)
				throw new ValidationException($"More than one priority found: [{string.Join(", ", priorities)}]");

			return new RuleWithPriority<TIn, TFail>
				   {
					   Priority = priorities.Single(),
					   Rule = combinator(rules.Select(x => x.Rule).ToArray())
				   };
		}

		public static IRuleWithPriority<TIn, TFail> Then<TIn, TFail>(this IRuleWithPriority<TIn, TFail> first,
																   IRuleWithPriority<TIn, TFail> second)
		{
			return Combine(CombinationHelpers.Order, first, second);
		}

		public static IRuleWithPriority<TIn, TFail> Order<TIn, TFail>(params IRuleWithPriority<TIn, TFail>[] rules)
		{
			return Combine(CombinationHelpers.Order, rules);
		}

		public static IRuleWithPriority<TIn, TFail> Union<TIn, TFail>(params IRuleWithPriority<TIn, TFail>[] rules)
		{
			return Combine(CombinationHelpers.Union, rules);
		}
	}
}
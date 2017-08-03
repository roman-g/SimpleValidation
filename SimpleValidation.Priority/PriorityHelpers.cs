using System;
using System.Collections.Generic;
using System.Linq;
using SimpleValidation.Core;
using SimpleValidation.Core.Combination;

namespace SimpleValidation.Priority
{
	public static class PriorityHelpers
	{
		public static IRuleWithPriority<TIn, TOut> WithPriority<TIn, TOut>(this Func<TIn, TOut[]> rule, int priority)
		{
			return new RuleWithPriority<TIn, TOut>
				   {
					   Rule = rule,
					   Priority = priority
				   };
		}

		public static Func<TIn, TOut[]> Collapse<TIn, TOut>(this IEnumerable<IRuleWithPriority<TIn, TOut>> rules)
		{
			var orderedRules = rules.GroupBy(x => x.Priority)
									.OrderBy(x => x.Key)
									.Select(x => Union(x.ToArray()))
									.Select(x => x.Rule)
									.ToArray();

			return CombinationHelpers.Order(orderedRules);
		}

		public static IRuleWithPriority<TIn, TOut> Combine<TIn, TOut>(Func<Func<TIn, TOut[]>[], Func<TIn, TOut[]>> combinator,
			params IRuleWithPriority<TIn, TOut>[] rules)
		{
			var priorities = rules.Select(x => x.Priority).Distinct().ToArray();
			if (priorities.Length > 1)
				throw new ValidationException($"More than one priority found: [{string.Join(", ", priorities)}]");

			return new RuleWithPriority<TIn, TOut>
				   {
					   Priority = priorities.Single(),
					   Rule = combinator(rules.Select(x => x.Rule).ToArray())
				   };
		}

		public static IRuleWithPriority<TIn, TOut> Then<TIn, TOut>(this IRuleWithPriority<TIn, TOut> first,
																   IRuleWithPriority<TIn, TOut> second)
		{
			return Combine(CombinationHelpers.Order, first, second);
		}

		public static IRuleWithPriority<TIn, TOut> Order<TIn, TOut>(params IRuleWithPriority<TIn, TOut>[] rules)
		{
			return Combine(CombinationHelpers.Order, rules);
		}

		public static IRuleWithPriority<TIn, TOut> Union<TIn, TOut>(params IRuleWithPriority<TIn, TOut>[] rules)
		{
			return Combine(CombinationHelpers.Union, rules);
		}
	}
}
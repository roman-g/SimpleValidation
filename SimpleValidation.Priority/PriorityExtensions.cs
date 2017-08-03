using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using SimpleValidation.Core;

namespace SimpleValidation.Priority
{
	public static class PriorityExtensions
	{
		public static IEnumerable<TOut> Apply<TIn, TOut>(
			this IEnumerable<IRuleWithPriority<TIn, TOut>> rules,
			TIn input)
		{
			var orderedRules = rules.GroupBy(x => x.Priority)
									.OrderBy(x => x.Key)
									.Select(x => Union(x.ToArray()))
									.Select(x => x.Rule)
									.ToArray();

			return CombinationHelpers.Order(orderedRules)(input);
		}

		public static IRuleWithPriority<TIn, TOut> Combine<TIn, TOut>(IRuleWithPriority<TIn, TOut>[] rules,
																	  Func<Func<TIn, TOut[]>[], Func<TIn, TOut[]>> combinator)
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
			return Combine(new[] {first, second}, CombinationHelpers.Order);
		}

		public static IRuleWithPriority<TIn, TOut> Order<TIn, TOut>(params IRuleWithPriority<TIn, TOut>[] rules)
		{
			return Combine(rules, CombinationHelpers.Order);
		}

		public static IRuleWithPriority<TIn, TOut> Union<TIn, TOut>(params IRuleWithPriority<TIn, TOut>[] rules)
		{
			return Combine(rules, CombinationHelpers.Union);
		}
	}
}
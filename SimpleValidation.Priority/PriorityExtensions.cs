using System.Collections.Generic;
using System.Linq;
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
									.Select(x => x.Select(y => y.Rule))
									.Select(x => CombinationHelpers.Union(x.ToArray()))
									.ToArray();
			return CombinationHelpers.Order(orderedRules)(input);
		}
	}
}
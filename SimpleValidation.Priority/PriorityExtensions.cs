using System.Collections.Generic;
using System.Linq;
using SimpleValidation.Core;

namespace SimpleValidation.Priority
{
	public static class PriorityExtensions
	{
		public static IEnumerable<ValidationResult<TOut>> Apply<TIn, TOut>(
			this IEnumerable<IRuleWithPriority<TIn, TOut>> rules,
			TIn input)
		{
			return rules.GroupBy(x => x.Priority)
						.OrderBy(x => x.Key)
						.Select(x => x.Select(rule => rule.Rule(input))
									  .Where(result => result.IsFail)
									  .ToArray())
						.FirstOrDefault(x => x.Any(y => y.IsFail));
		}
	}
}
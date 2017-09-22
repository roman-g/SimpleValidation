using System;
using System.Collections.Generic;
using System.Linq;
using SimpleValidation.Core.Combination;
using SimpleValidation.Core.Common;

namespace SimpleValidation.Priority
{
	public static class PriorityHelpers
	{
		public static IValidatorWithPriority<TIn, TFail> WithPriority<TIn, TFail>(
			this Validator<TIn, TFail> rule,
			int priority)
		{
			return new ValidatorWithPriority<TIn, TFail>
				   {
					   Validator = rule,
					   Priority = priority
				   };
		}

		public static Validator<TIn, TFail> Collapse<TIn, TFail>(this IEnumerable<IValidatorWithPriority<TIn, TFail>> rules)
		{
			var orderedRules = rules.GroupBy(x => x.Priority)
									.OrderBy(x => x.Key)
									.Select(x => Union(x.ToArray()))
									.Select(x => x.Validator)
									.ToArray();

			return CombinationHelpers.Order(orderedRules);
		}

		public static IValidatorWithPriority<TIn, TFail> Combine<TIn, TFail>(
			Func<Validator<TIn, TFail>[], Validator<TIn, TFail>> combinator,
			params IValidatorWithPriority<TIn, TFail>[] validators)
		{
			var priorities = validators.Select(x => x.Priority).Distinct().ToArray();
			if (priorities.Length > 1)
				throw new ValidationException($"More than one priority found: [{string.Join(", ", priorities)}]");

			return new ValidatorWithPriority<TIn, TFail>
				   {
					   Priority = priorities.Single(),
					   Validator = combinator(validators.Select(x => x.Validator).ToArray())
				   };
		}

		public static IValidatorWithPriority<TIn, TFail> Then<TIn, TFail>(this IValidatorWithPriority<TIn, TFail> first,
																		  IValidatorWithPriority<TIn, TFail> second)
		{
			return Combine(CombinationHelpers.Order, first, second);
		}

		public static IValidatorWithPriority<TIn, TFail> Union<TIn, TFail>(this IValidatorWithPriority<TIn, TFail> first,
																		   IValidatorWithPriority<TIn, TFail> second)
		{
			return Combine(CombinationHelpers.Union, first, second);
		}

		public static IValidatorWithPriority<TIn, TFail> Order<TIn, TFail>(
			params IValidatorWithPriority<TIn, TFail>[] validators)
		{
			return Combine(CombinationHelpers.Order, validators);
		}

		public static IValidatorWithPriority<TIn, TFail> Union<TIn, TFail>(
			params IValidatorWithPriority<TIn, TFail>[] validators)
		{
			return Combine(CombinationHelpers.Union, validators);
		}
	}
}
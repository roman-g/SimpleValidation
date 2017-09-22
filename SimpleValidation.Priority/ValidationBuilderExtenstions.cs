using System;
using System.Linq;
using SimpleValidation.Core.Builders;

namespace SimpleValidation.Priority
{
	public static class ValidationBuilderExtenstions
	{
		public static IValidatorWithPriority<TIn, TFail> Union<TBuilder, TIn, TFail>(
			this TBuilder builder,
			params Func<TBuilder, IValidatorWithPriority<TIn, TFail>>[] rules)
			where TBuilder : IValidationBuilder<TIn>
		{
			return PriorityHelpers.Union(rules.Select(x => x(builder)).ToArray());
		}

		public static IValidatorWithPriority<TIn, TFail> Order<TBuilder, TIn, TFail>(
			this TBuilder builder,
			params Func<TBuilder, IValidatorWithPriority<TIn, TFail>>[] rules)
			where TBuilder : IValidationBuilder<TIn>
		{
			return PriorityHelpers.Order(rules.Select(x => x(builder)).ToArray());
		}
	}
}
using System;
using System.Linq;
using SimpleValidation.Core.Combination;
using SimpleValidation.Core.Common;

namespace SimpleValidation.Core.Builders
{
	public static class ValidationBuilderExtenstions
	{
		public static Validator<TIn, TFail> Union<TBuilder, TIn, TFail>(
			this TBuilder builder,
			params Func<TBuilder, Validator<TIn, TFail>>[] rules)
			where TBuilder : IValidationBuilder<TIn>
		{
			return CombinationHelpers.Union(rules.Select(x => x(builder)).ToArray());
		}

		public static Validator<TIn, TFail> Order<TBuilder, TIn, TFail>(
			this TBuilder builder,
			params Func<TBuilder, Validator<TIn, TFail>>[] rules)
			where TBuilder : IValidationBuilder<TIn>
		{
			return CombinationHelpers.Order(rules.Select(x => x(builder)).ToArray());
		}

		public static TResult With<TBuilder, TResult>(
			this TBuilder builder,
			Func<TBuilder, TResult> creator)
			where TBuilder : IValidationBuilder
		{
			return creator(builder);
		}
	}
}
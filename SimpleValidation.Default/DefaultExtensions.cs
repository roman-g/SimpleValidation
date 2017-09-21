using System;
using System.Collections.Generic;
using System.Linq;
using SimpleValidation.Core.Builders;
using SimpleValidation.Core.Common;

namespace SimpleValidation.Default
{
	public static class DefaultExtensions
	{
		public static DefaultValidationSummary Apply<TIn>(
			this IEnumerable<Validator<TIn, DefaultValidationInfo>> validationErrors,
			TIn input)
		{
			return new DefaultValidationSummary
				   {
					   Errors = validationErrors.SelectMany(x => x(input)).ToArray()
				   };
		}

		public static Validator<TIn, DefaultValidationInfo> GreaterThan<TIn>(
			this IValidatorBuilderForMember<TIn, int> accessor,
			int threshold)
		{
			return accessor.DefaultPropertRule((_, x) => x > threshold, $"Value should be greater than {threshold}");
		}

		public static Validator<TIn, DefaultValidationInfo> NotEmpty<TIn>(
			this IValidatorBuilderForMember<TIn, string> accessor)
		{
			return accessor.DefaultPropertRule((_, x) => !string.IsNullOrEmpty(x), "String should not be empty");
		}

		public static Validator<TIn, DefaultValidationInfo> DefaultPropertRule<TIn, TProperty>(
			this IValidatorBuilderForMember<TIn, TProperty> accessor,
			Func<TIn, TProperty, bool> predicate,
			string message,
			string customState = null)
		{
			DefaultValidationInfo Mapping(MemberRuleContext<TIn, TProperty> context)
			{
				return new DefaultValidationInfo
					   {
						   PropertyValue = context.MemberValue,
						   PropertyName = context.MemberName,
						   Message = message,
						   CustomState = customState
					   };
			}

			return accessor.Make(predicate, Mapping);
		}
	}
}
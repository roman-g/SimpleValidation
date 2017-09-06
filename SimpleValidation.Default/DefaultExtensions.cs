using System;
using System.Collections.Generic;
using System.Linq;
using SimpleValidation.Core.Common;
using SimpleValidation.Core.MemberRules;

namespace SimpleValidation.Default
{
	public static class DefaultExtensions
	{
		public static DefaultValidationSummary Apply<TIn>(this IEnumerable<Validator<TIn, DefaultValidationInfo>> validationErrors, TIn input)
		{
			return new DefaultValidationSummary
			{
				Errors = validationErrors.SelectMany(x => x(input)).ToArray()
			};
		}

		public static Validator<TIn, DefaultValidationInfo> GreaterThan<TIn>(this MemberAccessor<TIn, int> accessor, int threshold)
		{
			return accessor.DefaultPropertRule((x, _) => x > threshold, $"Value should be greater than {threshold}");
		}

		public static Validator<TIn, DefaultValidationInfo> NotEmpty<TIn>(this MemberAccessor<TIn, string> accessor)
		{
			return accessor.DefaultPropertRule((x, _) => !string.IsNullOrEmpty(x), "String should not be empty");
		}

		public static Validator<TIn, DefaultValidationInfo> DefaultPropertRule<TIn, TProperty>(
			this MemberAccessor<TIn, TProperty> accessor,
			Func<TProperty, TIn, bool> predicate,
			string message,
			string customState = null)
		{
			DefaultValidationInfo[] Mapping(MemberRuleContext<TProperty, TIn> context)
			{
				return new DefaultValidationInfo
					   {
						   PropertyValue = context.MemberValue,
						   PropertyName = context.MemberName,
						   Message = message,
						   CustomState = customState
					   }.AsArray();
			}

			return accessor.Rule(predicate, Mapping);
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using SimpleValidation.Core;

namespace SimpleValidation.Default
{
	public static class DefaultExtensions
	{
		public static DefaultValidationSummary Apply<TIn>(this IEnumerable<Func<TIn, ValidationResult<DefaultValidationInfo>[]>> validationErrors, TIn input)
		{
			return new DefaultValidationSummary
			{
				Errors = validationErrors.SelectMany(x => x(input))
					.Where(x => x.IsFail)
					.Select(x => x.FailValue)
					.ToArray()
			};
		}

		public static Func<TIn, ValidationResult<DefaultValidationInfo>[]> GreaterThan<TIn>(this MemberAccessor<TIn, int> accessor, int threshold)
		{
			return accessor.DefaultPropertRule((x, _) => x > threshold, $"Value should be greater than {threshold}");
		}

		public static Func<TIn, ValidationResult<DefaultValidationInfo>[]> NotEmpty<TIn>(this MemberAccessor<TIn, string> accessor)
		{
			return accessor.DefaultPropertRule((x, _) => !string.IsNullOrEmpty(x), "String should not be empty");
		}

		public static Func<TIn, ValidationResult<DefaultValidationInfo>[]> DefaultPropertRule<TIn, TProperty>(
			this MemberAccessor<TIn, TProperty> accessor,
			Func<TProperty, TIn, bool> predicate,
			string message,
			string customState = null)
		{
			ValidationResult<DefaultValidationInfo>[] Mapping(MemberRuleContext<TProperty, TIn> context)
			{
				return ValidationResult<DefaultValidationInfo>.Fail(new DefaultValidationInfo
				{
					PropertyValue = context.MemberValue,
					PropertyName = context.MemberName,
					Message = message,
					CustomState = customState
				}).AsArray();
			}

			return accessor.Rule(predicate, Mapping);
		}
	}
}
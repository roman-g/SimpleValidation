using System;
using System.Linq;
using SimpleValidation.Core.Combination;
using SimpleValidation.Core.Common;
using SimpleValidation.Core.PredicateRules;

namespace SimpleValidation.Core.Builders
{
	public static class ValidatorBuilderForMemberExtensions
	{
		public static Validator<TIn, TFail> Custom<TIn, TProperty, TFail>(
			this IValidatorBuilderForMember<TIn, TProperty> builder,
			Func<MemberRuleContext<TIn, TProperty>, TFail> mappingToFail)
		{
			return builder.InContext(SimpleValidator.Make(mappingToFail));
		}

		public static Validator<TIn, TFail> Fail<TIn, TProperty, TFail>(
			this IValidatorBuilderForMember<TIn, TProperty> builder,
			TFail fail)
		{
			return builder.InContext(SimpleValidator.Make((MemberRuleContext<TIn, TProperty> _) => fail));
		}

		public static Validator<TIn, TFail> Ensure<TIn, TProperty, TFail>(
			this IValidatorBuilderForMember<TIn, TProperty> builder,
			Func<TIn, TProperty, bool> predicate,
			Func<MemberRuleContext<TIn, TProperty>, TFail> mappingToFail)
		{
			return builder.InContext(SimpleValidator.Make(mappingToFail)
			                                        .WithPredicate(c => predicate(c.Input, c.MemberValue)));
		}

		public static Validator<TIn, TFail> Ensure<TIn, TProperty, TFail>(
			this IValidatorBuilderForMember<TIn, TProperty> builder,
			Func<TProperty, bool> predicate,
			Func<MemberRuleContext<TIn, TProperty>, TFail> mappingToFail)
		{
			return builder.Ensure((i, p) => predicate(p), mappingToFail);
		}

		public static Validator<TIn, TFail> Ensure<TIn, TProperty, TFail>(
			this IValidatorBuilderForMember<TIn, TProperty> builder,
			Func<TIn, TProperty, bool> predicate,
			TFail fail)
		{
			return input => MemberRuleValidator.Make<TIn, TProperty, TFail>(_ => fail)
			                                   .WithPredicate(c => predicate(c.Input, c.MemberValue))(builder.ToContext(input));
		}

		public static Validator<TIn, TFail> Ensure<TIn, TProperty, TFail>(
			this IValidatorBuilderForMember<TIn, TProperty> builder,
			Func<TProperty, bool> predicate,
			TFail fail)
		{
			return builder.Ensure((i, p) => predicate(p), _ => fail);
		}
		
		private static MemberRuleContext<TIn, TProperty> ToContext<TIn, TProperty>(
			this IValidatorBuilderForMember<TIn, TProperty> builder,
			TIn input)
		{
			return new MemberRuleContext<TIn, TProperty>
				   {
					   Input = input,
					   MemberValue = builder.Accessor.Compile()(input),
					   MemberName = builder.Accessor.GetMemberName()
				   };
		}

		internal static Validator<TIn, TFail> InContext<TIn, TProperty, TFail>(
			this IValidatorBuilderForMember<TIn, TProperty> builder,
			Validator<MemberRuleContext<TIn, TProperty>, TFail> validator)
		{
			return input => validator(builder.ToContext(input));
		}

	}
}
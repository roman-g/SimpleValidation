using System;
using SimpleValidation.Core.Common;
using SimpleValidation.Core.PredicateRules;

namespace SimpleValidation.Core.MemberRules
{
	public static class MemberHelpers
	{
		public static Validator<TIn, TFail> Rule<TIn, TProperty, TFail>(
			this MemberAccessor<TIn, TProperty> accessor,
			Func<TProperty, TIn, bool> predicate,
			Validator<MemberRuleContext<TProperty, TIn>, TFail> mapping)
		{
			var mappingWithPredicate = mapping.WithPredicate(context => predicate(context.MemberValue, context.Input));
			return accessor.Rule(mappingWithPredicate);
		}

		public static Validator<TIn, TFail> Rule<TIn, TProperty, TFail>(
			this MemberAccessor<TIn, TProperty> accessor,
			Validator<MemberRuleContext<TProperty, TIn>, TFail> mapping)
		{
			return input =>
			{
				var context = new MemberRuleContext<TProperty, TIn>
				{
					Input = input,
					MemberValue = accessor.Accessor.Compile()(input),
					MemberName = accessor.Accessor.GetMemberName()
				};
				return mapping(context);
			};
		}
    }
}
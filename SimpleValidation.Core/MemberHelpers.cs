using System;

namespace SimpleValidation.Core
{
	public static class MemberHelpers
	{
		public static Func<TIn, ValidationResult<TOut>> Rule<TIn, TProperty, TOut>(
			this MemberAccessor<TIn, TProperty> accessor,
			Func<TProperty, TIn, bool> predicate,
			Func<MemberRuleContext<TProperty, TIn>, ValidationResult<TOut>> mapping)
		{
			var mappingWithPredicate = PredicateHelpers.WrapWithPredicate(
				context => predicate(context.MemberValue, context.Input),
				mapping);
			return accessor.Rule(mappingWithPredicate);
		}

		public static Func<TIn, ValidationResult<TOut>> Rule<TIn, TProperty, TOut>(
			this MemberAccessor<TIn, TProperty> accessor,
			Func<MemberRuleContext<TProperty, TIn>, ValidationResult<TOut>> mapping)
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
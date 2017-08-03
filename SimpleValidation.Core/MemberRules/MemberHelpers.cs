﻿using System;
using SimpleValidation.Core.Common;
using SimpleValidation.Core.PredicateRules;

namespace SimpleValidation.Core.MemberRules
{
	public static class MemberHelpers
	{
		public static Func<TIn, TOut[]> Rule<TIn, TProperty, TOut>(
			this MemberAccessor<TIn, TProperty> accessor,
			Func<TProperty, TIn, bool> predicate,
			Func<MemberRuleContext<TProperty, TIn>, TOut[]> mapping)
		{
			var mappingWithPredicate = mapping.WithPredicate(context => predicate(context.MemberValue, context.Input));
			return accessor.Rule(mappingWithPredicate);
		}

		public static Func<TIn, TOut[]> Rule<TIn, TProperty, TOut>(
			this MemberAccessor<TIn, TProperty> accessor,
			Func<MemberRuleContext<TProperty, TIn>, TOut[]> mapping)
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
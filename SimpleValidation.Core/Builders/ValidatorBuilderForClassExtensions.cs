using System;
using System.Linq.Expressions;
using SimpleValidation.Core.Common;
using SimpleValidation.Core.PredicateRules;

namespace SimpleValidation.Core.Builders
{
	public static class ValidatorBuilderForClassExtensions
	{
		public static Validator<TIn, TFail> Make<TIn, TFail>(this IValidatorBuilderForClass<TIn> _,
															 Func<TIn, bool> predicate,
															 Func<TIn, TFail> mappingToFail)
		{
			return SimpleValidator.Make(mappingToFail).WithPredicate(predicate);
		}

		public static Validator<TIn, TFail> Make<TIn, TFail>(this IValidatorBuilderForClass<TIn> _,
															 Func<TIn, bool> predicate,
															 TFail fail)
		{
			return SimpleValidator.Make((TIn x) => fail).WithPredicate(predicate);
		}

		public static IValidatorBuilderForMember<TIn, TProperty> ForMember<TIn, TProperty>(
			this IValidatorBuilderForClass<TIn> _,
			Expression<Func<TIn, TProperty>> accessor)
		{
			return new AccessorCarrier<TIn, TProperty>(accessor);
		}
		
		private class AccessorCarrier<TIn, TProperty> : IValidatorBuilderForMember<TIn, TProperty>
		{
			public AccessorCarrier(Expression<Func<TIn, TProperty>> accessor)
			{
				Accessor = accessor;
			}

			public Expression<Func<TIn, TProperty>> Accessor { get; }
		}
	}
}
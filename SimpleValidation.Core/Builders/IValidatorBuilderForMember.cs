using System;
using System.Linq.Expressions;

namespace SimpleValidation.Core.Builders
{
	public interface IValidatorBuilderForMember<TIn, TProperty>: IValidationBuilder<TIn>
	{
		Expression<Func<TIn, TProperty>> Accessor { get; }
	}
}
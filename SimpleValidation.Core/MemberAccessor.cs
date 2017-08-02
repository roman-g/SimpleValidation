using System;
using System.Linq.Expressions;

namespace SimpleValidation.Core
{
    public class MemberAccessor<TIn, TProperty>
    {
        public Expression<Func<TIn, TProperty>> Accessor { get; }

        public MemberAccessor(Expression<Func<TIn, TProperty>> accessor)
        {
            Accessor = accessor;
        }
    }

	public class RuleFor<TIn>
	{
		
	}

	public static class RuleForExtensions
	{
		public static MemberAccessor<TIn, TProperty> Member<TIn, TProperty>(this RuleFor<TIn> builder, Expression<Func<TIn, TProperty>> accessor)
		{
			return new MemberAccessor<TIn, TProperty>(accessor);
		}
	}
}
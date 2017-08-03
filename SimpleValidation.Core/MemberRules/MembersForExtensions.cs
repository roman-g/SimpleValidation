using System;
using System.Linq.Expressions;

namespace SimpleValidation.Core.MemberRules
{
	public static class MembersForExtensions
	{
		public static MemberAccessor<TIn, TProperty> Member<TIn, TProperty>(this MembersFor<TIn> builder, Expression<Func<TIn, TProperty>> accessor)
		{
			return new MemberAccessor<TIn, TProperty>(accessor);
		}
	}
}
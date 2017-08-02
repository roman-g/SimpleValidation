using System;
using System.Linq.Expressions;

namespace SimpleValidation.Core
{
	public static class ExpressionExtensions
	{
		public static string GetMemberName<TIn, TOut>(this Expression<Func<TIn, TOut>> expression)
		{
			var memberExpression = (MemberExpression)expression.Body;
			return memberExpression.Member.Name;
		}
	}
}
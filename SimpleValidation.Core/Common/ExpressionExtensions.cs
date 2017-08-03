using System;
using System.Linq.Expressions;

namespace SimpleValidation.Core.Common
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
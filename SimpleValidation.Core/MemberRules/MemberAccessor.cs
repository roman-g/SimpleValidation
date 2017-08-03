using System;
using System.Linq.Expressions;

namespace SimpleValidation.Core.MemberRules
{
    public class MemberAccessor<TIn, TProperty>
    {
        public Expression<Func<TIn, TProperty>> Accessor { get; }

        public MemberAccessor(Expression<Func<TIn, TProperty>> accessor)
        {
            Accessor = accessor;
        }
    }
}
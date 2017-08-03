using System;
using SimpleValidation.Core;

namespace SimpleValidation.Priority
{
    public interface IRuleWithPriority<in TIn, out TOut>
    {
        Func<TIn, TOut[]> Rule { get; }
        int Priority { get; }
    }
}
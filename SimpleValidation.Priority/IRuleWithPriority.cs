using System;

namespace SimpleValidation.Priority
{
    public interface IRuleWithPriority<in TIn, out TFail>
    {
        Func<TIn, TFail[]> Rule { get; }
        int Priority { get; }
    }
}
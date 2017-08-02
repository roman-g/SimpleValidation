using System;
using SimpleValidation.Core;

namespace SimpleValidation.Priority
{
    public interface IRuleWithPriority<in TIn, TOut>
    {
        Func<TIn, ValidationResult<TOut>> Rule { get; }
        int Priority { get; }
    }
}
using System;
using SimpleValidation.Core;

namespace SimpleValidation.Priority
{
    public class RuleWithPriority<TIn, TOut> : IRuleWithPriority<TIn, TOut>
    {
        public Func<TIn, ValidationResult<TOut>> Rule { get; set; }
        public int Priority { get; set; }
    }
}
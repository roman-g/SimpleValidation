using System;

namespace SimpleValidation.Priority
{
    public class RuleWithPriority<TIn, TOut> : IRuleWithPriority<TIn, TOut>
    {
        public Func<TIn, TOut[]> Rule { get; set; }
        public int Priority { get; set; }
    }
}
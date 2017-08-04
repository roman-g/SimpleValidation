using System;

namespace SimpleValidation.Priority
{
    public class RuleWithPriority<TIn, TFail> : IRuleWithPriority<TIn, TFail>
    {
        public Func<TIn, TFail[]> Rule { get; set; }
        public int Priority { get; set; }
    }
}
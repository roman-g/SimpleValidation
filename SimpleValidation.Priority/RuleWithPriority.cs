using System;
using SimpleValidation.Core.Common;

namespace SimpleValidation.Priority
{
    public class RuleWithPriority<TIn, TFail> : IRuleWithPriority<TIn, TFail>
    {
        public Validator<TIn, TFail> Rule { get; set; }
        public int Priority { get; set; }
    }
}
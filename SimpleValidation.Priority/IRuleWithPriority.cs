using System;
using SimpleValidation.Core.Common;

namespace SimpleValidation.Priority
{
    public interface IRuleWithPriority<in TIn, out TFail>
    {
		Validator<TIn, TFail> Rule { get; }
        int Priority { get; }
    }
}
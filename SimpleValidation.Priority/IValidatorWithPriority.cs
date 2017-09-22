using System;
using SimpleValidation.Core.Common;

namespace SimpleValidation.Priority
{
    public interface IValidatorWithPriority<in TIn, out TFail>
    {
		Validator<TIn, TFail> Validator { get; }
        int Priority { get; }
    }
}
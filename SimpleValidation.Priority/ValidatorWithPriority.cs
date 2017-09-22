using SimpleValidation.Core.Common;

namespace SimpleValidation.Priority
{
    public class ValidatorWithPriority<TIn, TFail> : IValidatorWithPriority<TIn, TFail>
    {
        public Validator<TIn, TFail> Validator { get; set; }
        public int Priority { get; set; }
    }
}
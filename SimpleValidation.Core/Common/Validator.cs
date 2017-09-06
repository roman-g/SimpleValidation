namespace SimpleValidation.Core.Common
{
	public delegate TFail[] Validator<in TIn, out TFail>(TIn input);
}
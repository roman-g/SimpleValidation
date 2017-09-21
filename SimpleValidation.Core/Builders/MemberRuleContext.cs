using System;
using SimpleValidation.Core.Common;

namespace SimpleValidation.Core.Builders
{
	public class MemberRuleContext<TIn, TProperty>
	{
		public TProperty MemberValue { get; set; }
		public string MemberName { get; set; }
		public TIn Input { get; set; }
	}

	public static class MemberRuleValidator
	{
		public static Validator<MemberRuleContext<TIn, TProperty>, TFail> Make<TIn, TProperty, TFail>(Func<MemberRuleContext<TIn, TProperty>, TFail> rule)
		{
			return SimpleValidator.Make(rule);
		}
	}
}
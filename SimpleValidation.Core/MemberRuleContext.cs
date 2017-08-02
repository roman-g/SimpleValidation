namespace SimpleValidation.Core
{
	public class MemberRuleContext<TProperty, TIn>
	{
		public TProperty MemberValue { get; set; }
		public string MemberName { get; set; }
		public TIn Input { get; set; }
	}
}
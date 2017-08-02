using System;
using System.Linq;
using Shouldly;
using Xunit;

namespace SimpleValidation.Core.Tests
{
	public class MemberHelpersTest
	{
		[Fact]
		public void Simple()
		{
			var sample = new TestClass
						 {
							 StringProperty = "prop",
							 StringField = "field"
						 };
			var builder = new RuleFor<TestClass>();
			var fieldFail = builder.Member(x => x.StringField).Rule(Fail)(sample).Single();
			fieldFail.IsFail.ShouldBeTrue();
			fieldFail.FailValue.ShouldBe((sample, "StringField", "field"));

			var propFail = builder.Member(x => x.StringProperty).Rule(Fail)(sample).Single();
			propFail.IsFail.ShouldBeTrue();
			propFail.FailValue.ShouldBe((sample, "StringProperty", "prop"));
		}

		[Fact]
		public void Predicate()
		{
			var builder = new RuleFor<TestClass>();
			var rule = builder.Member(x => x.StringField)
							  .Rule((str, input) => str == input.StringProperty, Fail);

			var sampleForFail = new TestClass {StringField = "field", StringProperty = "property"};
			var fail = rule(sampleForFail).Single();
			fail.IsFail.ShouldBeTrue();
			fail.FailValue.ShouldBe((sampleForFail, "StringField", "field"));

			var sampleForSuccess = new TestClass { StringField = "good", StringProperty = "good" };
			var success = rule(sampleForSuccess).Single();
			success.IsFail.ShouldBeFalse();
		}

		private static ValidationResult<(TestClass, string, string)>[] Fail(MemberRuleContext<string, TestClass> context)
		{
			return ValidationResult<(
					TestClass input,
					string memberName,
					string memberValue)>
				.Fail((context.Input, context.MemberName, context.MemberValue)).AsArray();
		}

		public class TestClass
		{
			public string StringProperty { get; set; }
			public string StringField;
		}
	}
}
using System;
using System.Linq;
using System.Linq.Expressions;
using Shouldly;
using SimpleValidation.Core.Common;
using SimpleValidation.Core.MemberRules;
using Xunit;

namespace SimpleValidation.Core.Tests
{
	public class MemberHelpersTest
	{
		[Fact]
		public void Simple()
		{
			Expression<Func<TestClass, object>> e = x => x.StringField;

			var sample = new TestClass
						 {
							 StringProperty = "prop",
							 StringField = "field"
						 };
			var builder = new ValidationFor<TestClass>();
			var fieldFail = builder.Member(x => x.StringField).Rule(Fail)(sample).Single();
			fieldFail.ShouldBe((sample, "StringField", "field"));

			var propFail = builder.Member(x => x.StringProperty).Rule(Fail)(sample).Single();
			propFail.ShouldBe((sample, "StringProperty", "prop"));
		}

		[Fact]
		public void Predicate()
		{
			var builder = new ValidationFor<TestClass>();
			var rule = builder.Member(x => x.StringField)
							  .Rule((str, input) => str == input.StringProperty, Fail);

			var sampleForFail = new TestClass {StringField = "field", StringProperty = "property"};
			var fail = rule(sampleForFail).Single();
			fail.ShouldBe((sampleForFail, "StringField", "field"));

			var sampleForSuccess = new TestClass { StringField = "good", StringProperty = "good" };
			rule(sampleForSuccess).ShouldBeEmpty();
		}

		private static (TestClass, string, string)[] Fail(MemberRuleContext<string, TestClass> context)
		{
			return (context.Input, context.MemberName, context.MemberValue).AsArray();
		}

		public class TestClass
		{
			public string StringProperty { get; set; }
			public string StringField;
		}
	}
}
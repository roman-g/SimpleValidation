using System.Linq;
using Shouldly;
using SimpleValidation.Core.Builders;
using SimpleValidation.Core.Common;
using Xunit;

namespace SimpleValidation.Core.Tests
{
	public class ValidatorBuilderForMemberTest
	{
		[Fact]
		public void InContext()
		{
			var rule = MakeValidator.For<TestClass>()
									.ForMember(x => x.StringField)
									.InContext(MemberRuleValidator.Make<TestClass, string, string>(ctx => ctx.MemberName));

			rule(new TestClass()).Single().ShouldBe("StringField");
		}

		[Fact]
		public void WithoutPredicate()
		{
			var sample = new TestClass
						 {
							 StringProperty = "prop",
							 StringField = "field"
						 };
			var builder = MakeValidator.For<TestClass>();
			var fieldFail = builder.ForMember(x => x.StringField).Make(Fail)(sample).Single();
			fieldFail.ShouldBe((sample, "StringField", "field"));

			var propFail = builder.ForMember(x => x.StringProperty).Make(Fail)(sample).Single();
			propFail.ShouldBe((sample, "StringProperty", "prop"));
			
			var simpleFail = builder.ForMember(x => x.StringProperty).Make("fail")(sample).Single();
			simpleFail.ShouldBe("fail");
		}

		[Fact]
		public void PredicateWithContextAndMapping()
		{
			var builder = MakeValidator.For<TestClass>();
			var rule = builder.ForMember(x => x.StringField)
							  .Make((input, str) => str == input.StringProperty, Fail);

			var sampleForFail = new TestClass {StringField = "field", StringProperty = "property"};
			var fail = rule(sampleForFail).Single();
			fail.ShouldBe((sampleForFail, "StringField", "field"));

			var sampleForSuccess = new TestClass { StringField = "good", StringProperty = "good" };
			rule(sampleForSuccess).ShouldBeEmpty();
		}

		[Fact]
		public void PredicateWithContextAndStaticFail()
		{
			var builder = MakeValidator.For<TestClass>();
			const string expectedFail = "fail";
			var rule = builder.ForMember(x => x.StringField)
							  .Make((input, str) => str == input.StringProperty, expectedFail);

			var sampleForFail = new TestClass { StringField = "field", StringProperty = "property" };

			var fail = rule(sampleForFail).Single();
			fail.ShouldBe(expectedFail);

			var sampleForSuccess = new TestClass { StringField = "good", StringProperty = "good" };
			rule(sampleForSuccess).ShouldBeEmpty();
		}

		[Fact]
		public void PredicateWithoutContextAndMapping()
		{
			var builder = MakeValidator.For<TestClass>();
			const string validStringFieldValue = "good";
			var rule = builder.ForMember(x => x.StringField)
							  .Make(str => str == validStringFieldValue, Fail);

			var sampleForFail = new TestClass {StringField = "bad"};

			var fail = rule(sampleForFail).Single();
			fail.ShouldBe((sampleForFail, "StringField", "bad"));

			var sampleForSuccess = new TestClass {StringField = validStringFieldValue};
			rule(sampleForSuccess).ShouldBeEmpty();
		}

		[Fact]
		public void PredicateWithoutContextAndStaticFail()
		{
			var builder = MakeValidator.For<TestClass>();
			const string validStringFieldValue = "good";
			var expectedFail = "fail";
			var rule = builder.ForMember(x => x.StringField)
							  .Make(str => str == validStringFieldValue, expectedFail);

			var sampleForFail = new TestClass { StringField = "bad" };
			var fail = rule(sampleForFail).Single();
			fail.ShouldBe(expectedFail);

			var sampleForSuccess = new TestClass { StringField = validStringFieldValue };
			rule(sampleForSuccess).ShouldBeEmpty();
		}

		[Fact]
		public void Union()
		{
			var builder = MakeValidator.For<TestClass>();
			var rule = builder.ForMember(x => x.StringField)
							  .Union(x => x.Make("1"),
									 x => x.Make("2"));
			rule(new TestClass()).ShouldBe(new[]{"1", "2"});
		}

		private static (TestClass, string, string) Fail(MemberRuleContext<TestClass, string> context)
		{
			return (context.Input, context.MemberName, context.MemberValue);
		}

		public class TestClass
		{
			public string StringProperty { get; set; }
			public string StringField;
		}
	}
}
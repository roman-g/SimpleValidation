using System.Linq;
using Shouldly;
using SimpleValidation.Core.Builders;
using SimpleValidation.Core.Combination;
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
			var fieldFail = builder.ForMember(x => x.StringField).Rule(Fail)(sample).Single();
			fieldFail.ShouldBe((sample, "StringField", "field"));

			var propFail = builder.ForMember(x => x.StringProperty).Rule(Fail)(sample).Single();
			propFail.ShouldBe((sample, "StringProperty", "prop"));

			var simpleFail = builder.ForMember(x => x.StringProperty).Rule("fail")(sample).Single();
			simpleFail.ShouldBe("fail");
		}

		[Fact]
		public void PredicateWithContextAndMapping()
		{
			var rule = MakeValidator.For<TestClass>()
									.ForMember(x => x.StringField)
									.Rule((input, str) => str == input.StringProperty, Fail);

			var sampleForFail = new TestClass {StringField = "field", StringProperty = "property"};
			var fail = rule(sampleForFail).Single();
			fail.ShouldBe((sampleForFail, "StringField", "field"));

			var sampleForSuccess = new TestClass {StringField = "good", StringProperty = "good"};
			rule(sampleForSuccess).ShouldBeEmpty();
		}

		[Fact]
		public void PredicateWithContextAndStaticFail()
		{
			const string expectedFail = "fail";
			var rule = MakeValidator.For<TestClass>()
									.ForMember(x => x.StringField)
									.Rule((input, str) => str == input.StringProperty, expectedFail);

			var sampleForFail = new TestClass {StringField = "field", StringProperty = "property"};

			var fail = rule(sampleForFail).Single();
			fail.ShouldBe(expectedFail);

			var sampleForSuccess = new TestClass {StringField = "good", StringProperty = "good"};
			rule(sampleForSuccess).ShouldBeEmpty();
		}

		[Fact]
		public void PredicateWithoutContextAndMapping()
		{
			const string validStringFieldValue = "good";
			var rule = MakeValidator.For<TestClass>()
									.ForMember(x => x.StringField)
									.Rule(str => str == validStringFieldValue, Fail);

			var sampleForFail = new TestClass {StringField = "bad"};

			var fail = rule(sampleForFail).Single();
			fail.ShouldBe((sampleForFail, "StringField", "bad"));

			var sampleForSuccess = new TestClass {StringField = validStringFieldValue};
			rule(sampleForSuccess).ShouldBeEmpty();
		}

		[Fact]
		public void PredicateWithoutContextAndStaticFail()
		{
			const string validStringFieldValue = "good";
			const string expectedFail = "fail";
			var rule = MakeValidator.For<TestClass>()
									.ForMember(x => x.StringField)
									.Rule(str => str == validStringFieldValue, expectedFail);

			var sampleForFail = new TestClass {StringField = "bad"};
			var fail = rule(sampleForFail).Single();
			fail.ShouldBe(expectedFail);

			var sampleForSuccess = new TestClass {StringField = validStringFieldValue};
			rule(sampleForSuccess).ShouldBeEmpty();
		}

		[Fact]
		public void Union()
		{
			var rule = MakeValidator.For<TestClass>()
									.ForMember(x => x.StringField)
									.Union(x => x.Rule("1"),
										   x => x.Rule("2"));
			rule(new TestClass()).ShouldBe(new[] {"1", "2"});
		}

		[Fact]
		public void Order()
		{
			var rule = MakeValidator.For<TestClass>()
									.ForMember(x => x.StringField)
									.Order(x => x.Rule(y => y.Length > 0, "1"),
										   x => x.Rule(y => y.Length > 1, "2"));
			rule(new TestClass {StringField = ""}).ShouldBe(new[] {"1"});
			rule(new TestClass {StringField = "1"}).ShouldBe(new[] {"2"});
			rule(new TestClass {StringField = "12"}).ShouldBeEmpty();
		}

		[Fact]
		public void Custom()
		{
			var rule = MakeValidator.For<TestClass>()
									.ForMember(x => x.StringField)
									.Custom(x => x.Rule(y => y.Length > 0, "1")
												  .Then(x.Rule(y => y.Length > 1, "2")));
			rule(new TestClass {StringField = ""}).ShouldBe(new[] {"1"});
			rule(new TestClass {StringField = "1"}).ShouldBe(new[] {"2"});
			rule(new TestClass {StringField = "12"}).ShouldBeEmpty();
		}

		[Fact]
		public void ComplexExample()
		{
			var valdiator = MakeValidator.For<TestClass>()
										 .Custom(b => b.Union(bb => bb.ForMember(x => x.StringField)
																	  .Order(x => x.Rule(s => s != null, "string field should not be null"),
																			 x => x.Rule(s => s.Length >= 2, "string field length should be gte 2")),
															  bb => bb.ForMember(x => x.StringProperty)
																	  .Rule(s => s != null, "string property should not be null"))
													   .Then(b.Rule(i => i.StringField == i.StringProperty, "strings should be equal")));

			valdiator(new TestClass { StringField = null, StringProperty = null})
				.ShouldBe(new[]{ "string field should not be null" , "string property should not be null" });

			valdiator(new TestClass { StringField = "1", StringProperty = null })
				.ShouldBe(new[] { "string field length should be gte 2", "string property should not be null" });

			valdiator(new TestClass { StringField = "1", StringProperty = "2" })
				.ShouldBe(new[] { "string field length should be gte 2" });

			valdiator(new TestClass { StringField = "11", StringProperty = "2" })
				.ShouldBe(new[] { "strings should be equal" });

			valdiator(new TestClass { StringField = "11", StringProperty = "11" })
				.ShouldBeEmpty();
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
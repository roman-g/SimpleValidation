using System;
using System.Linq;
using Shouldly;
using Xunit;

namespace SimpleValidation.Core.Tests
{
	public class CombinationExtensionsTest
	{
		[Fact]
		public void Then()
		{
			var rule1 = PredicateHelpers.WrapWithPredicate(
				str => str != "failInput1",
				new Func<string, ValidationResult<string>[]>(x => ValidationResult<string>.Fail("fail1").AsArray()));

			var rule2 = PredicateHelpers.WrapWithPredicate(
				str => str != "failInput2",
				new Func<string, ValidationResult<string>[]>(x => ValidationResult<string>.Fail("fail2").AsArray()));

			var crashRule = new Func<string, ValidationResult<string>[]>(_ => throw new Exception());

			var rule = rule1.Then(rule2);

			var goodValidationResult = rule("goodInput");
			goodValidationResult.Length.ShouldBe(2);
			goodValidationResult.ShouldAllBe(x => !x.IsFail);

			var fail2ValidationResult = rule("failInput2");
			fail2ValidationResult.Length.ShouldBe(2);
			fail2ValidationResult.Single(x => x.IsFail).FailValue.ShouldBe("fail2");

			var fail1ValidationResult = rule("failInput1");
			fail1ValidationResult.Length.ShouldBe(1);
			fail1ValidationResult.Single().FailValue.ShouldBe("fail1");

			rule1.Then(crashRule)("failInput1").Single().FailValue.ShouldBe("fail1");
		}

		[Fact]
		public void Union()
		{
			var rule1 = PredicateHelpers.WrapWithPredicate(
				str => str != "failInput1",
				new Func<string, ValidationResult<string>[]>(x => ValidationResult<string>.Fail("fail1").AsArray()));

			var rule2 = PredicateHelpers.WrapWithPredicate(
				str => str != "failInput2",
				new Func<string, ValidationResult<string>[]>(x => ValidationResult<string>.Fail("fail2").AsArray()));
			
			var rule = rule1.Union(rule2);

			var goodValidationResult = rule("goodInput");
			goodValidationResult.Length.ShouldBe(2);
			goodValidationResult.ShouldAllBe(x => !x.IsFail);

			var fail2ValidationResult = rule("failInput2");
			fail2ValidationResult.Length.ShouldBe(2);
			fail2ValidationResult.Single(x => x.IsFail).FailValue.ShouldBe("fail2");

			var fail1ValidationResult = rule("failInput1");
			fail1ValidationResult.Length.ShouldBe(2);
			fail1ValidationResult.Single(x => x.IsFail).FailValue.ShouldBe("fail1");
		}
	}
}
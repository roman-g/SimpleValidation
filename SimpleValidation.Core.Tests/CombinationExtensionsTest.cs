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
				new Func<string, string[]>(x => "fail1".AsArray()));

			var rule2 = PredicateHelpers.WrapWithPredicate(
				str => str != "failInput2",
				new Func<string, string[]>(x => "fail2".AsArray()));

			var crashRule = new Func<string, string[]>(_ => throw new Exception());

			var rule = rule1.Then(rule2);

			rule("goodInput").ShouldBeEmpty();
			rule("failInput2").Single().ShouldBe("fail2");
			rule("failInput1").Single().ShouldBe("fail1");
			rule1.Then(crashRule)("failInput1").Single().ShouldBe("fail1");
		}

		[Fact]
		public void Union()
		{
			var rule1 = PredicateHelpers.WrapWithPredicate(
				str => str != "failInput",
				new Func<string, string[]>(x => "fail1".AsArray()));

			var rule2 = PredicateHelpers.WrapWithPredicate(
				str => str != "failInput",
				new Func<string, string[]>(x => "fail2".AsArray()));
			
			var rule = CombinationHelpers.Union(rule1, rule2);

			rule("goodInput").ShouldBeEmpty();

			var validationResult = rule("failInput");
			validationResult.ShouldBe(new[]{"fail1", "fail2"});
		}
	}
}
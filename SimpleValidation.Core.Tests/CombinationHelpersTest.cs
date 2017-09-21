using System;
using System.Collections.Generic;
using System.Linq;
using Shouldly;
using SimpleValidation.Core.Combination;
using SimpleValidation.Core.Common;
using SimpleValidation.Core.PredicateRules;
using Xunit;

namespace SimpleValidation.Core.Tests
{
	public class CombinationHelpersTest
	{
		[Fact]
		public void Then()
		{
			var rule1 = SimpleValidator.Make((string x) => "fail1").WithPredicate(str => str != "failInput1");
			var rule2 = SimpleValidator.Make((string x) => "fail2").WithPredicate(str => str != "failInput2");

			var crashRule = new Validator<string, string>(_ => throw new Exception());

			var rule = rule1.Then(rule2);

			rule("goodInput").ShouldBeEmpty();
			rule("failInput2").Single().ShouldBe("fail2");
			rule("failInput1").Single().ShouldBe("fail1");
			rule1.Then(crashRule)("failInput1").Single().ShouldBe("fail1");
		}

		[Fact]
		public void Union()
		{
			var rule1 = SimpleValidator.Make((string x) => "fail1").WithPredicate(str => str != "failInput");
			var rule2 = SimpleValidator.Make((string x) => "fail2").WithPredicate(str => str != "failInput");
			
			var rule = CombinationHelpers.Union(rule1, rule2);

			rule("goodInput").ShouldBeEmpty();

			var validationResult = rule("failInput");
			validationResult.ShouldBe(new[]{"fail1", "fail2"});
		}
	}
}
using System.Linq;
using Shouldly;
using SimpleValidation.Core.Builders;
using SimpleValidation.Core.Combination;
using SimpleValidation.Core.Common;
using SimpleValidation.Core.PredicateRules;
using Xunit;

namespace SimpleValidation.Priority.Test
{
    public class PriorityHelpersTest
	{
        [Fact]
        public void CollapseDifferentPriorities()
        {
	        var rule1 = MakeValidator.For<string>().Ensure(s => s != "failInput1", "fail1").WithPriority(1);
			var rule2 = MakeValidator.For<string>().Ensure(s => !s.StartsWith("failInput"), "fail2").WithPriority(2);
			
			var rules = new[] { rule1, rule2 };
			var collapsedRule = rules.Collapse();
			collapsedRule("goodInput").ShouldBeEmpty();
			collapsedRule("failInput1").Single().ShouldBe("fail1");
			collapsedRule("failInputX").Single().ShouldBe("fail2");
		}

		[Fact]
		public void CollapseTheSamePriorities()
		{
			var rule1 = MakeValidator.For<string>().Ensure(s => s != "failInput1", "fail1").WithPriority(1);
			var rule2 = MakeValidator.For<string>().Ensure(s => !s.StartsWith("failInput"),"fail2").WithPriority(1);

			var rules = new[] { rule1, rule2 };
			var collapsedRule = rules.Collapse();
			collapsedRule("goodInput").ShouldBeEmpty();
			collapsedRule("failInput1").ShouldBe(new[]{"fail1", "fail2"});
		}

		[Fact]
		public void Combine()
		{
			var rule1 = MakeValidator.For<string>().Ensure(s => s != "failInput1", "fail1").WithPriority(1);
			var rule2 = MakeValidator.For<string>().Ensure(s => !s.StartsWith("failInput"), "fail2").WithPriority(1);
			
			var combinedRule = PriorityHelpers.Combine(CombinationHelpers.Order, rule1, rule2);
			combinedRule.Priority.ShouldBe(1);
			combinedRule.Validator("goodInput").ShouldBeEmpty();
			combinedRule.Validator("failInput1").Single().ShouldBe("fail1");
			combinedRule.Validator("failInputX").Single().ShouldBe("fail2");
		}

		[Fact]
		public void CombineFailsIfPrioritiesDontMatch()
		{
			var rule1 = SimpleValidator.Make((string s) => "fail1").WithPriority(1);
			var rule2 = SimpleValidator.Make((string s) => "fail2").WithPriority(2);
			Should.Throw<ValidationException>(() => PriorityHelpers.Combine(CombinationHelpers.Order, rule1, rule2));
		}


		[Fact]
		public void Union()
		{
			var rule = MakeValidator.For<string>().Union(
				x => x.Ensure(_ => false, "fail1").WithPriority(1),
				x => x.Ensure(_ => false, "fail2").WithPriority(1));
			rule.Priority.ShouldBe(1);
			rule.Validator("").ShouldBe(new[]{"fail1", "fail2"});
		}

		[Fact]
		public void Order()
		{
			var rule = MakeValidator.For<string>().Order(
				x => x.Ensure(_ => false, "fail1").WithPriority(1),
				x => x.Ensure(_ => false, "fail2").WithPriority(1));
			rule.Priority.ShouldBe(1);
			rule.Validator("").ShouldBe(new[] { "fail1" });
		}

		[Fact]
		public void UnionFailsIfPrioritiesDontMatch()
		{
			Should.Throw<ValidationException>(() => MakeValidator.For<string>().Union(
												  x => x.Ensure(_ => false, "fail1").WithPriority(1),
												  x => x.Ensure(_ => false, "fail2").WithPriority(2)));
		}

		[Fact]
		public void OrderFailsIfPrioritiesDontMatch()
		{
			Should.Throw<ValidationException>(() => MakeValidator.For<string>().Order(
												  x => x.Ensure(_ => false, "fail1").WithPriority(1),
												  x => x.Ensure(_ => false, "fail2").WithPriority(2)));
		}
	}
}

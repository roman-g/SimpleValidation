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
	        var rule1 = MakeValidator.For<string>().Make(s => s != "failInput1", "fail1").WithPriority(1);
			var rule2 = MakeValidator.For<string>().Make(s => !s.StartsWith("failInput"), "fail2").WithPriority(2);
			
			var rules = new[] { rule1, rule2 };
			var collapsedRule = rules.Collapse();
			collapsedRule("goodInput").ShouldBeEmpty();
			collapsedRule("failInput1").Single().ShouldBe("fail1");
			collapsedRule("failInputX").Single().ShouldBe("fail2");
		}

		[Fact]
		public void CollapseTheSamePriorities()
		{
			var rule1 = MakeValidator.For<string>().Make(s => s != "failInput1", "fail1").WithPriority(1);
			var rule2 = MakeValidator.For<string>().Make(s => !s.StartsWith("failInput"),"fail2").WithPriority(1);

			var rules = new[] { rule1, rule2 };
			var collapsedRule = rules.Collapse();
			collapsedRule("goodInput").ShouldBeEmpty();
			collapsedRule("failInput1").ShouldBe(new[]{"fail1", "fail2"});
		}

		[Fact]
		public void Combine()
		{
			var rule1 = MakeValidator.For<string>().Make(s => s != "failInput1", "fail1").WithPriority(1);
			var rule2 = MakeValidator.For<string>().Make(s => !s.StartsWith("failInput"), "fail2").WithPriority(1);
			
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
    }
}

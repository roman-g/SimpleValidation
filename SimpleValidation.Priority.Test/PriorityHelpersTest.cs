using System.Linq;
using Shouldly;
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
	        var rule1 = Rule.Single((string s) => "fail1").WithPredicate(s => s != "failInput1").WithPriority(1);
			var rule2 = Rule.Single((string s) => "fail2").WithPredicate(s => !s.StartsWith("failInput")).WithPriority(2);
			
			var rules = new[] { rule1, rule2 };
			var collapsedRule = rules.Collapse();
			collapsedRule("goodInput").ShouldBeEmpty();
			collapsedRule("failInput1").Single().ShouldBe("fail1");
			collapsedRule("failInputX").Single().ShouldBe("fail2");
		}

		[Fact]
		public void CollapseTheSamePriorities()
		{
			var rule1 = Rule.Single((string s) => "fail1").WithPredicate(s => s != "failInput1").WithPriority(1);
			var rule2 = Rule.Single((string s) => "fail2").WithPredicate(s => !s.StartsWith("failInput")).WithPriority(1);

			var rules = new[] { rule1, rule2 };
			var collapsedRule = rules.Collapse();
			collapsedRule("goodInput").ShouldBeEmpty();
			collapsedRule("failInput1").ShouldBe(new[]{"fail1", "fail2"});
		}

		[Fact]
		public void Combine()
		{
			var rule1 = Rule.Single((string s) => "fail1").WithPredicate(s => s != "failInput1").WithPriority(1);
			var rule2 = Rule.Single((string s) => "fail2").WithPredicate(s => !s.StartsWith("failInput")).WithPriority(1);
			
			var combinedRule = PriorityHelpers.Combine(CombinationHelpers.Order, rule1, rule2);
			combinedRule.Priority.ShouldBe(1);
			combinedRule.Rule("goodInput").ShouldBeEmpty();
			combinedRule.Rule("failInput1").Single().ShouldBe("fail1");
			combinedRule.Rule("failInputX").Single().ShouldBe("fail2");
		}

		[Fact]
		public void CombineFailsIfPrioritiesDontMatch()
		{
			var rule1 = new Validator<string, string>(s => "fail1".AsArray()).WithPriority(1);
			var rule2 = new Validator<string, string>(s => "fail2".AsArray()).WithPriority(2);
			Should.Throw<ValidationException>(() => PriorityHelpers.Combine(CombinationHelpers.Order, rule1, rule2));
		}
    }
}

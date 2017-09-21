using System.Linq;
using Shouldly;
using SimpleValidation.Core.Common;
using SimpleValidation.Core.PredicateRules;
using Xunit;

namespace SimpleValidation.Core.Tests
{
    public class PredicateHelpersTest
    {
        [Fact]
        public void Simple()
        {
	        var rule = SimpleValidator.Make((string input) => "Empty string").WithPredicate(x => !string.IsNullOrEmpty(x));

			rule("filled").ShouldBeEmpty();
	        rule("").Single().ShouldBe("Empty string");
		}
    }
}

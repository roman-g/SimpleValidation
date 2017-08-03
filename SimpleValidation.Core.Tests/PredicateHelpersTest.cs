using System.Linq;
using Shouldly;
using Xunit;

namespace SimpleValidation.Core.Tests
{
    public class PredicateHelpersTest
    {
        [Fact]
        public void Simple()
        {
	        var rule = PredicateHelpers.WrapWithPredicate<string, string>(
				x => !string.IsNullOrEmpty(x),
		        input => "Empty string".AsArray());

			rule("filled").ShouldBeEmpty();
	        rule("").Single().ShouldBe("Empty string");
		}
    }
}

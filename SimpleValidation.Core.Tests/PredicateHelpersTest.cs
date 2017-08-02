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
		        input => ValidationResult<string>.Fail("Empty string").AsArray());

			rule("filled").Single().IsFail.ShouldBeFalse();
	        var fail = rule("").Single();
	        fail.IsFail.ShouldBeTrue();
			fail.FailValue.ShouldBe("Empty string");
		}
    }
}
